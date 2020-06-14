using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;
using System.Xml.Linq;

namespace PPTail.SiteGenerator
{
    public class Builder : ISiteBuilder
    {
        const String _additionalFilePathsSettingName = "additionalFilePaths";
        const String _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";
        const String _createDasBlogPostsCompatibilityFileSettingName = "createDasBlogPostsCompatibilityFile";

        const String _providerKey = "Provider";

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
            _serviceProvider.ValidateService<ISettings>();
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
            var settings = this.ServiceProvider.GetService<ISettings>();
            var navProvider = this.ServiceProvider.GetService<INavigationProvider>();
            var archiveProvider = this.ServiceProvider.GetService<IArchiveProvider>();
            var contactProvider = this.ServiceProvider.GetService<IContactProvider>();
            var searchProvider = this.ServiceProvider.GetService<ISearchProvider>();
            var redirectProvider = this.ServiceProvider.GetService<IRedirectProvider>();
            var syndicationProvider = this.ServiceProvider.GetService<ISyndicationProvider>();
            var contentEncoder = this.ServiceProvider.GetService<IContentEncoder>();
            var contentRepo = this.ServiceProvider.GetService<IContentRepository>();

            var siteSettings = contentRepo.GetSiteSettings();
            var posts = contentRepo.GetAllPosts();
            var pages = contentRepo.GetAllPages();
            var widgets = contentRepo.GetAllWidgets();
            var categories = contentRepo.GetCategories();

            // Create Sidebar Content
            var rootLevelSidebarContent = pageGen.GenerateSidebarContent(posts, pages, widgets, "./");
            var childLevelSidebarContent = pageGen.GenerateSidebarContent(posts, pages, widgets, "../");

            // Create navbars
            var rootLevelNavigationContent = navProvider.CreateNavigation(pages, "./", settings.OutputFileExtension);
            var childLevelNavigationContent = navProvider.CreateNavigation(pages, "../", settings.OutputFileExtension);

            // Create bootstrap file
            var bootstrapFile = new SiteFile()
            {
                RelativeFilePath = $"./bootstrap.min.css",
                SourceTemplateType = Enumerations.TemplateType.Bootstrap,
                Content = pageGen.GenerateBootstrapPage()
            };

            if (!string.IsNullOrEmpty(bootstrapFile.Content))
                result.Add(bootstrapFile);


            // Create Style page
            result.Add(new SiteFile()
            {
                RelativeFilePath = $"./Style.css",
                SourceTemplateType = Enumerations.TemplateType.Style,
                Content = pageGen.GenerateStylesheet()
            });

            // Create home page
            result.Add(new SiteFile()
            {
                RelativeFilePath = $"./index.html",
                SourceTemplateType = Enumerations.TemplateType.HomePage,
                Content = homePageGen.GenerateHomepage(rootLevelSidebarContent, rootLevelNavigationContent, posts)
            });

            // Create Archive
            result.Add(new SiteFile()
            {
                RelativeFilePath = $"./archive.html",
                SourceTemplateType = Enumerations.TemplateType.Archive,
                Content = archiveProvider.GenerateArchive(posts, pages, rootLevelNavigationContent, rootLevelSidebarContent, "./")
            });

            // Create Contact Page
            result.Add(new SiteFile()
            {
                RelativeFilePath = $"./contact.html",
                SourceTemplateType = Enumerations.TemplateType.ContactPage,
                Content = contactProvider.GenerateContactPage(rootLevelNavigationContent, rootLevelSidebarContent, "./")
            });

            // Create RSS Feed
            var syndicationContent = syndicationProvider.GenerateFeed(posts);
            result.Add(new SiteFile()
            {
                RelativeFilePath = "./syndication.xml",
                SourceTemplateType = Enumerations.TemplateType.Syndication,
                Content = syndicationContent
            });

            String createCompatibilityFileValue = settings.GetExtendedSetting(_createDasBlogSyndicationCompatibilityFileSettingName);
            bool.TryParse(createCompatibilityFileValue, out var createCompatibilityFile);
            if (createCompatibilityFile)
                result.Add(new SiteFile()
                {
                    RelativeFilePath = "./syndication.axd",
                    SourceTemplateType = Enumerations.TemplateType.Syndication,
                    Content = syndicationContent
                });

            foreach (var post in posts)
            {
                // Add all published content pages to the results
                if (post.IsPublished)
                {
                    if (string.IsNullOrWhiteSpace(post.Slug))
                        post.Slug = contentEncoder.UrlEncode(post.Title);

                    // Add the post page
                    String postFileName = $"{post.Slug}.{settings.OutputFileExtension}";
                    String postFilePath = System.IO.Path.Combine("Posts", postFileName);
                    var postPageTemplateType = Enumerations.TemplateType.PostPage;
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = postFilePath,
                        SourceTemplateType = postPageTemplateType,
                        Content = contentItemPageGen.Generate(childLevelSidebarContent, childLevelNavigationContent, post, postPageTemplateType, "..", false)
                    });

                    // Add the permalink page
                    String permalinkFileName = $"{contentEncoder.HTMLEncode(post.Id.ToString())}.{settings.OutputFileExtension}";
                    String permalinkFilePath = System.IO.Path.Combine("Permalinks", permalinkFileName);
                    String redirectFilePath = $"../Posts/{postFileName}"; // System.IO.Path.Combine("..", postFilePath);
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = permalinkFilePath,
                        SourceTemplateType = Enumerations.TemplateType.Redirect,
                        Content = redirectProvider.GenerateRedirect(redirectFilePath)
                    });
                }
            }

            foreach (var page in pages)
            {
                // Add all published content pages to the results
                if (page.IsPublished)
                {
                    if (string.IsNullOrWhiteSpace(page.Slug))
                        page.Slug = contentEncoder.UrlEncode(page.Title);

                    var contentPageTemplateType = Enumerations.TemplateType.ContentPage;
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = $"Pages/{page.Slug}.{settings.OutputFileExtension}",
                        SourceTemplateType = contentPageTemplateType,
                        Content = contentItemPageGen.Generate(childLevelSidebarContent, childLevelNavigationContent, page, contentPageTemplateType, "..", false)
                    });
                }
            }

            // Add Search Pages
            var tags = posts
                .Where(p => p.Tags.IsNotNull())
                .SelectMany(p => p.Tags)
                .Distinct();

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
                    RelativeFilePath = $"Search/{contentEncoder.UrlEncode(name)}.{settings.OutputFileExtension}",
                    SourceTemplateType = Enumerations.TemplateType.SearchPage,
                    IsBase64Encoded = false
                });
            }

            // Add favicon.ico file if it exists
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
            if (!string.IsNullOrWhiteSpace(siteSettings?.Theme))
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
            String relativePathString = settings.GetExtendedSetting(_additionalFilePathsSettingName);
            if (!string.IsNullOrEmpty(relativePathString))
            {
                var additionalFilePaths = relativePathString.Split(',');
                var additionalFiles = contentRepo.GetFoldersContents(additionalFilePaths);
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

            // Create DasBlog Compatibility Data File for Posts.aspx
            String createPostsCompatibilityFileValue = settings.GetExtendedSetting(_createDasBlogPostsCompatibilityFileSettingName);
            bool.TryParse(createPostsCompatibilityFileValue, out var createPostsCompatibilityFile);

            if (createPostsCompatibilityFile)
            {
                String postTemplate = "<post id=\"{0}\" url=\"Posts\\{1}.html\"/>";
                var postDataResults = new List<string>();
                foreach (var post in posts)
                    postDataResults.Add(string.Format(postTemplate, post.Id.ToString(), post.Slug));

                result.Add(new SiteFile()
                {
                    RelativeFilePath = System.IO.Path.Combine("app_data", "posts.xml"),
                    SourceTemplateType = Enumerations.TemplateType.Raw,
                    Content = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><posts>{string.Join("", postDataResults)}</posts>",
                    IsBase64Encoded = false
                });
            }

            return result;
        }
    }
}
