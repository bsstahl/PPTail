using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Generator.Navigation.Test
{
    public class BasicProvider_CreateNavigation_Should
    {
        [Fact]
        public void IncludeAHomePageMenuItem()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            string currentUrl = "/";
            string homeUrl = "index.html";

            var actual = target.CreateNavigation(pages, currentUrl, homeUrl);
            Assert.Contains("home", actual.ToLower());
        }

        [Fact]
        public void IncludeAMenuItemForEachContentPage()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(4);
            string currentUrl = "/";
            string homeUrl = "index.html";

            var actual = target.CreateNavigation(pages, currentUrl, homeUrl);
            foreach (var page in pages)
                Assert.Contains(page.Title, actual);
        }

        [Fact]
        public void IncludeALinkToTheHomePage()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            string currentUrl = "/";
            string homeUrl = "index.html";

            var actual = target.CreateNavigation(pages, currentUrl, homeUrl);
            string href = $"{homeUrl}\"";
            Assert.Contains(href, actual);
        }

        [Fact]
        public void IncludeALinkToEachContentPage()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(4);
            string currentUrl = "/";
            string homeUrl = "index.html";

            var actual = target.CreateNavigation(pages, currentUrl, homeUrl);
            foreach (var page in pages)
            {
                string href = $"{page.Slug}.html\"";
                Assert.Contains(href, actual);
            }
        }
    }
}
