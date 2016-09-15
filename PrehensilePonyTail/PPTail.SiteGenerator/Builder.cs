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
        private readonly IServiceProvider _serviceProvider;

        public Builder(IServiceCollection container)
        {
            _serviceProvider = container.BuildServiceProvider();
        }

        public IEnumerable<SiteFile> Build()
        {
            var result = new List<SiteFile>();

            var contentRepo = _serviceProvider.GetService<IContentRepository>();
            var pageGen = _serviceProvider.GetService<IPageGenerator>();
            var settings = _serviceProvider.GetService<Settings>();

            var posts = contentRepo.GetAllPosts();
            foreach (var post in posts)
            {
                // All all published content pages to the results
                if (post.IsPublished)
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = $".\\Posts\\{post.Slug}.{settings.outputFileExtension}",
                        Content = pageGen.GeneratePostPage(post)
                    });
            }

            var pages = contentRepo.GetAllPages();
            foreach (var page in pages)
            {
                // All all published content pages to the results
                if (page.IsPublished)
                    result.Add(new SiteFile()
                    {
                        RelativeFilePath = $".\\Pages\\{page.Slug}.{settings.outputFileExtension}",
                        Content = pageGen.GenerateContentPage(page)
                    });
            }

            return result;
        }
    }
}
