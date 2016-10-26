using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.Archive.Test
{
    public class BasicProvider_GenerateArchive_Should
    {
        [Fact]
        public void GenerateAtLeastOneLinkToEveryPublishedPost()
        {
            string pathToRoot = "/";
            string navContent = "Place Navigation Here";
            string sidebarContent = "Place Sidebar Here";

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();
            var serviceProvider = container.BuildServiceProvider();

            var settings = serviceProvider.GetService<ISettings>();
            var siteSettings = serviceProvider.GetService<SiteSettings>();

            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            foreach (var post in posts)
            {
                var href = System.IO.Path.Combine(pathToRoot, "Posts" , $"{post.Slug}.{settings.OutputFileExtension}");
                Assert.Contains(href, actual);
            }
        }
    }
}
