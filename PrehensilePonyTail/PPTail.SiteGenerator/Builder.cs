using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace PPTail.SiteGenerator
{
    public class Builder : ISiteBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public Builder(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<IContentRepository>();
            _serviceProvider.ValidateService<IPageGenerator>();
            _serviceProvider.ValidateService<IContentItemPageGenerator>();
            _serviceProvider.ValidateService<IHomePageGenerator>();
            _serviceProvider.ValidateService<INavigationProvider>();
            _serviceProvider.ValidateService<IArchiveProvider>();
            _serviceProvider.ValidateService<IContactProvider>();
            _serviceProvider.ValidateService<ISearchProvider>();
            _serviceProvider.ValidateService<IRedirectProvider>();
            _serviceProvider.ValidateService<ISyndicationProvider>();
            _serviceProvider.ValidateService<IContentEncoder>();
        }

        private IServiceProvider ServiceProvider { get { return _serviceProvider; } }


        public IEnumerable<SiteFile> Build()
        {
            var result = new List<SiteFile>();

            var pageGen = this.ServiceProvider.GetService<IPageGenerator>();
            var contentItemPageGen = this.ServiceProvider.GetService<IContentItemPageGenerator>();
            var homePageGen = this.ServiceProvider.GetService<IHomePageGenerator>();
            var navProvider = this.ServiceProvider.GetService<INavigationProvider>();
            var archiveProvider = this.ServiceProvider.GetService<IArchiveProvider>();
            var contactProvider = this.ServiceProvider.GetService<IContactProvider>();
            var searchProvider = this.ServiceProvider.GetService<ISearchProvider>();
            var redirectProvider = this.ServiceProvider.GetService<IRedirectProvider>();
            var syndicationProvider = this.ServiceProvider.GetService<ISyndicationProvider>();
            var contentEncoder = this.ServiceProvider.GetService<IContentEncoder>();
            var contentRepo = this.ServiceProvider.GetService<IContentRepository>();
            var logger = this.ServiceProvider.GetService<ILogger<Builder>>();

            var siteSettings = contentRepo.GetSiteSettings() ?? new SiteSettings();
            var posts = contentRepo.GetAllPosts();
            var pages = contentRepo.GetAllPages();
            var widgets = contentRepo.GetAllWidgets();
            var categories = contentRepo.GetCategories();

            // Create Sidebar Content
            if (logger.IsNotNull()) logger.LogInformation("Generating sidebar content");
            var rootLevelSidebarContent = pageGen.GenerateSidebarContent(posts, pages, widgets, "./");
            var childLevelSidebarContent = pageGen.GenerateSidebarContent(posts, pages, widgets, "../");

            // Create navbars
            if (logger.IsNotNull()) logger.LogInformation("Generating navbar content");
            var rootLevelNavigationContent = navProvider.CreateNavigation(pages, "./", siteSettings.OutputFileExtension);
            var childLevelNavigationContent = navProvider.CreateNavigation(pages, "../", siteSettings.OutputFileExtension);

            // Create bootstrap file
            if (logger.IsNotNull()) logger.LogInformation("Generating bootstrap file");
            var bootstrapFile = new SiteFile()
            {
                RelativeFilePath = $"./bootstrap.min.css",
                SourceTemplateType = Enumerations.TemplateType.Bootstrap,
                Content = pageGen.GenerateBootstrapPage()
            };
            if (!string.IsNullOrEmpty(bootstrapFile.Content))
                result.Add(bootstrapFile);

            // Create Style page
            if (logger.IsNotNull()) logger.LogInformation("Generating style (css) file");
            result.Add(new SiteFile()
            {
                RelativeFilePath = $"./Style.css",
                SourceTemplateType = Enumerations.TemplateType.Style,
                Content = pageGen.GenerateStylesheet()
            });

            // Create home page
            if (logger.IsNotNull()) logger.LogInformation("Generating home page");
            result.Add(new SiteFile()
            {
                RelativeFilePath = $"./index.html",
                SourceTemplateType = Enumerations.TemplateType.HomePage,
                Content = homePageGen.GenerateHomepage(rootLevelSidebarContent, rootLevelNavigationContent, posts)
            });

            // Create Archive
            if (logger.IsNotNull()) logger.LogInformation("Generating archive page");
            result.Add(new SiteFile()
            {
                RelativeFilePath = $"./archive.html",
                SourceTemplateType = Enumerations.TemplateType.Archive,
                Content = archiveProvider.GenerateArchive(posts, pages, rootLevelNavigationContent, rootLevelSidebarContent, "./")
            });

            // Create Contact Page
            if (logger.IsNotNull()) logger.LogInformation("Generating contact page");
            result.Add(new SiteFile()
            {
                RelativeFilePath = $"./contact.html",
                SourceTemplateType = Enumerations.TemplateType.ContactPage,
                Content = contactProvider.GenerateContactPage(rootLevelNavigationContent, rootLevelSidebarContent, "./")
            });

            // Create RSS Feed
            if (logger.IsNotNull()) logger.LogInformation("Generating RSS feed");
            var syndicationContent = syndicationProvider.GenerateFeed(posts);
            result.Add(new SiteFile()
            {
                RelativeFilePath = "./syndication.xml",
                SourceTemplateType = Enumerations.TemplateType.Syndication,
                Content = syndicationContent
            });

            if (logger.IsNotNull()) logger.LogInformation("Generating post pages");
            foreach (var post in posts)
            {
                // Add all published content pages to the results
                if (string.IsNullOrWhiteSpace(post.Slug))
                    post.Slug = contentEncoder.UrlEncode(post.Title);
                String postFileName = $"{post.Slug}.{siteSettings.OutputFileExtension}";

                if (post.IsPublished || post.BuildIfNotPublished)
                {
                    // Add the post page
                    String postFilePath = System.IO.Path.Combine("Posts", postFileName);
                    var postPageTemplateType = Enumerations.TemplateType.PostPage;
                    if (logger.IsNotNull()) logger.LogInformation("Generating post page for PostId: {PostId}", post.Id);
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = postFilePath,
                        SourceTemplateType = postPageTemplateType,
                        Content = contentItemPageGen.Generate(childLevelSidebarContent, childLevelNavigationContent, post, postPageTemplateType, "..", false)
                    });
                }

                if (post.IsPublished)
                {
                    // Add the permalink page
                    String permalinkFileName = $"{contentEncoder.HTMLEncode(post.Id.ToString())}.{siteSettings.OutputFileExtension}";
                    String permalinkFilePath = System.IO.Path.Combine("Permalinks", permalinkFileName);
                    String redirectFilePath = $"../Posts/{postFileName}"; // System.IO.Path.Combine("..", postFilePath);
                    if (logger.IsNotNull()) logger.LogInformation("Publishing post page for PostId: {PostId}", post.Id);
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = permalinkFilePath,
                        SourceTemplateType = Enumerations.TemplateType.Redirect,
                        Content = redirectProvider.GenerateRedirect(redirectFilePath)
                    });
                }
            }

            if (logger.IsNotNull()) logger.LogInformation("Generating content pages");
            foreach (var page in pages)
            {
                // Add all published content pages to the results
                if (page.IsPublished)
                {
                    if (logger.IsNotNull()) logger.LogInformation("Generating content page for Id: {Id}", page.Id);

                    if (string.IsNullOrWhiteSpace(page.Slug))
                        page.Slug = contentEncoder.UrlEncode(page.Title);

                    var contentPageTemplateType = Enumerations.TemplateType.ContentPage;
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = $"Pages/{page.Slug}.{siteSettings.OutputFileExtension}",
                        SourceTemplateType = contentPageTemplateType,
                        Content = contentItemPageGen.Generate(childLevelSidebarContent, childLevelNavigationContent, page, contentPageTemplateType, "..", false)
                    });
                }
            }

            // Add Search Pages
            if (logger.IsNotNull()) logger.LogInformation("Generating search pages");
            var tags = posts.GetAllTags().Distinct();

            var categoryIds = posts.SelectMany(p => p.CategoryIds).Distinct();
            var usedCategories = categories.Where(c => categoryIds.Contains(c.Id));

            IEnumerable<string> usedCategoryNames = usedCategories.Any()
                ? usedCategories.Select(c => c.Name.ToLower())
                : new List<String>();

            var searchNames = new List<string>();
            searchNames.AddRange(tags);
            searchNames.AddRange(usedCategoryNames);

            var distinctSearchTerms = searchNames
                .Distinct()
                .Where(t => !string.IsNullOrEmpty(t));

            foreach (var name in distinctSearchTerms)
            {
                result.Add(new SiteFile()
                {
                    Content = searchProvider.GenerateSearchResultsPage(name, posts, childLevelNavigationContent, childLevelSidebarContent, "../"),
                    RelativeFilePath = $"Search/{contentEncoder.UrlEncode(name)}.{siteSettings.OutputFileExtension}",
                    SourceTemplateType = Enumerations.TemplateType.SearchPage,
                    IsBase64Encoded = false
                });
            }

            // Add favicon.ico file if it exists
            if (logger.IsNotNull()) logger.LogInformation("Add favicon.ico file");
            var rootFiles = contentRepo.GetFolderContents(".");
            var faviconFile = rootFiles.SingleOrDefault(f => f.FileName == "favicon.ico");
            if (faviconFile.IsNotNull())
            {
                result.Add(new SiteFile()
                {
                    RelativeFilePath = System.IO.Path.Combine("", faviconFile.FileName),
                    SourceTemplateType = Enumerations.TemplateType.Raw,
                    Content = System.Convert.ToBase64String(faviconFile.Contents),
                    IsBase64Encoded = true
                });
            }

            // Add files from Theme if there are any
            if (logger.IsNotNull()) logger.LogInformation("Add files from theme");
            if (!string.IsNullOrWhiteSpace(siteSettings.Theme))
            {
                String path = $".\\themes\\{siteSettings.Theme}";
                var additionalFiles = contentRepo.GetFolderContents(path);
                foreach (var rawFile in additionalFiles)
                {
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = System.IO.Path.Combine("Theme", rawFile.FileName),
                        SourceTemplateType = Enumerations.TemplateType.Raw,
                        Content = System.Convert.ToBase64String(rawFile.Contents),
                        IsBase64Encoded = true
                    });
                }
            }

            // Add additional raw files
            if (logger.IsNotNull()) logger.LogInformation("Add additional raw files");
            if (siteSettings.AdditionalFilePaths.IsNotNull() && siteSettings.AdditionalFilePaths.Any())
            {
                var additionalFiles = contentRepo.GetFoldersContents(siteSettings.AdditionalFilePaths, true);
                foreach (var rawFile in additionalFiles)
                {
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = System.IO.Path.Combine(rawFile.RelativePath, rawFile.FileName),
                        SourceTemplateType = Enumerations.TemplateType.Raw,
                        Content = System.Convert.ToBase64String(rawFile.Contents),
                        IsBase64Encoded = true
                    });
                }
            }

            return result;
        }
    }
}
