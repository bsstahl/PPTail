using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.SiteGenerator
{
    public class Builder
    {
        private IServiceProvider _serviceProvider;

        public Builder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private IServiceProvider ServiceProvider { get { return _serviceProvider; } }

        public IEnumerable<SiteFile> Build()
        {
            var result = new List<SiteFile>();

            var contentRepo = ServiceProvider.GetService<IContentRepository>();
            var pageGen = ServiceProvider.GetService<IPageGenerator>();
            var settings = ServiceProvider.GetService<Settings>();

            var siteSettings = contentRepo.GetSiteSettings();
            var posts = contentRepo.GetAllPosts();
            var pages = contentRepo.GetAllPages();

            var widgets = contentRepo.GetAllWidgets();

            var sidebarContent = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);

            // Create bootstrap file
            result.Add(new SiteFile()
            {
                RelativeFilePath = $".\\bootstrap.min.css",
                SourceTemplateType = Enumerations.TemplateType.Bootstrap,
                Content = pageGen.GenerateBootstrapPage()
            });

            // Create Style page
            result.Add(new SiteFile()
            {
                RelativeFilePath = $".\\Style.css",
                SourceTemplateType = Enumerations.TemplateType.Style,
                Content = pageGen.GenerateStylesheet(siteSettings)
            });

            // Create home page
            result.Add(new SiteFile()
            {
                RelativeFilePath = $".\\index.html",
                SourceTemplateType = Enumerations.TemplateType.HomePage,
                Content = pageGen.GenerateHomepage(sidebarContent, siteSettings, posts)
            });

            foreach (var post in posts)
            {
                // Add all published content pages to the results
                if (post.IsPublished)
                {
                    if (string.IsNullOrWhiteSpace(post.Slug))
                        post.Slug = post.Title.CreateSlug();

                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = $".\\Posts\\{post.Slug.HTMLEncode()}.{settings.outputFileExtension}",
                        SourceTemplateType = Enumerations.TemplateType.PostPage,
                        Content = pageGen.GeneratePostPage(sidebarContent, siteSettings, post)
                    });
                }
            }

            foreach (var page in pages)
            {
                // Add all published content pages to the results
                if (page.IsPublished)
                {
                    if (string.IsNullOrWhiteSpace(page.Slug))
                        page.Slug = page.Title.CreateSlug();

                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = $".\\Pages\\{page.Slug.HTMLEncode()}.{settings.outputFileExtension}",
                        SourceTemplateType = Enumerations.TemplateType.ContentPage,
                        Content = pageGen.GenerateContentPage(sidebarContent, siteSettings, page)
                    });
                }
            }

            return result;
        }
    }
}
