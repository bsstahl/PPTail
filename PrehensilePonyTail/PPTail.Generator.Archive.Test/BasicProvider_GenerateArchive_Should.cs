using PPTail.Entities;
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

            var settings = (null as Settings).CreateDefault(string.Empty);
            var siteSettings = (null as SiteSettings).Create();

            var target = (null as BasicProvider).Create();
            var actual = target.GenerateArchive(settings, siteSettings, posts, pages, navContent, sidebarContent, pathToRoot);

            foreach (var post in posts)
            {
                var href = System.IO.Path.Combine(pathToRoot, "Posts" , $"{post.Slug}.{settings.outputFileExtension}");
                Assert.Contains(href, actual);
            }
        }
    }
}
