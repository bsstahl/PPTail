using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

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
            string outputFileExtension = "html";
            string pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            Assert.Contains("home", actual.ToLower());
        }

        [Fact]
        public void IncludeAnArchiveMenuItem()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            string outputFileExtension = "html";
            string pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            Assert.Contains("archive", actual.ToLower());
        }

        [Fact]
        public void IncludeAMenuItemForEachPublishedContentPage()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(4);
            string outputFileExtension = "html";
            string pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            foreach (var page in pages)
                Assert.Contains(page.Title, actual);
        }

        [Fact]
        public void ExcludeAnyUnpublishedContentPages()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(4);
            string outputFileExtension = "html";
            string pathToRoot = string.Empty;
            pages.GetRandom().IsPublished = false;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            foreach (var page in pages.Where(p => !p.IsPublished))
                Assert.DoesNotContain(page.Title, actual);
        }

        [Fact]
        public void ExcludeAnyPagesNotMarkedShowInList()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(5);
            string outputFileExtension = "html";
            string pathToRoot = string.Empty;

            pages.GetRandom().ShowInList = false;
            var expected = pages.Count(p => p.ShowInList);
            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);

            foreach (var page in pages.Where(p => !p.ShowInList))
                Assert.DoesNotContain(page.Title, actual);
        }

        [Fact]
        public void IncludeALinkToTheHomePage()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            string outputFileExtension = "html";
            string pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            string href = $"index.html";
            Assert.Contains(href, actual);
        }

        [Fact]
        public void IncludeALinkToTheArchive()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            string outputFileExtension = "html";
            string pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            string href = $"archive.{outputFileExtension}\"";
            Assert.Contains(href, actual);
        }

        [Fact]
        public void IncludeALinkToEachContentPage()
        {
            IServiceProvider serviceProvider = null;
            var target = (null as BasicProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(4);
            string outputFileExtension = "html";
            string pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            foreach (var page in pages)
            {
                string href = $"{page.Slug}.html\"";
                Assert.Contains(href, actual);
            }
        }
    }
}
