using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Builders;
using Moq;
using PPTail.Interfaces;

namespace PPTail.Generator.Navigation.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class BootstrapProvider_CreateNavigation_Should
    {
        [Fact]
        public void IncludeAHomePageMenuItemIfDisplayTitleInNavbarIsFalse()
        {
            var siteSettings = new SiteSettingsBuilder()
                .DisplayTitleInNavbar(false)
                .Build();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);

            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository(contentRepo)
                .AddLinkProvider()
                .BuildServiceProvider();

            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            Assert.Contains("home", actual.ToLower());
        }

        [Fact]
        public void IncludeATitleMenuItemIfDisplayTitleInNavbarIsTrue()
        {
            string title = string.Empty.GetRandom();
            var siteSettings = new SiteSettingsBuilder()
                .DisplayTitleInNavbar(true)
                .Title(title)
                .Build();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);

            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository(contentRepo)
                .AddLinkProvider()
                .BuildServiceProvider();

            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            Assert.Contains(title, actual.ToLower());
        }


        [Fact]
        public void IncludeAnArchiveMenuItem()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            Assert.Contains("archive", actual.ToLower());
        }

        [Fact]
        public void IncludeAMenuItemForEachPublishedContentPage()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(4);
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            foreach (var page in pages)
                Assert.Contains(page.Title, actual);
        }

        [Fact]
        public void ExcludeAnyUnpublishedContentPages()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(4);
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;
            pages.GetRandom().IsPublished = false;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            foreach (var page in pages.Where(p => !p.IsPublished))
                Assert.DoesNotContain(page.Title, actual);
        }

        [Fact]
        public void ExcludeAnyPagesNotMarkedShowInList()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(5);
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            pages.GetRandom().ShowInList = false;
            var expected = pages.Count(p => p.ShowInList);
            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);

            foreach (var page in pages.Where(p => !p.ShowInList))
                Assert.DoesNotContain(page.Title, actual);
        }

        [Fact]
        public void IncludeALinkToTheHomePage()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            String href = $"index.html";
            Assert.Contains(href, actual);
        }

        [Fact]
        public void IncludeALinkToTheArchive()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            String href = $"archive.{outputFileExtension}\"";
            Assert.Contains(href, actual);
        }

        [Fact]
        public void IncludeTheNavDropdownIfUseAdditionalPagesDropdownIsTrue()
        {
            var dropdownMenuLabel = string.Empty.GetRandom();
            var siteSettings = new SiteSettings()
            {
                UseAdditionalPagesDropdown = true,
                AdditionalPagesDropdownLabel = dropdownMenuLabel
            };

            var contentRepo = new Mock<IContentRepository>();
            contentRepo
                .Setup(r => r.GetSiteSettings())
                .Returns(siteSettings);

            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository(contentRepo)
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            Assert.Contains(dropdownMenuLabel, actual);
        }

        [Fact]
        public void NotIncludeTheNavDropdownIfUseAdditionalPagesDropdownIsFalse()
        {
            var dropdownMenuLabel = string.Empty.GetRandom();
            var siteSettings = new SiteSettings()
            {
                UseAdditionalPagesDropdown = false,
                AdditionalPagesDropdownLabel = dropdownMenuLabel
            };

            var contentRepo = new Mock<IContentRepository>();
            contentRepo
                .Setup(r => r.GetSiteSettings())
                .Returns(siteSettings);

            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository(contentRepo)
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            Assert.DoesNotContain(dropdownMenuLabel, actual);
        }

        [Fact]
        public void IncludeALinkToTheContactPage()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            String href = $"contact.{outputFileExtension}\"";
            Assert.Contains(href, actual);
        }

        [Fact]
        public void IncludeALinkToTheRSSFeed()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create();
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            String href = $"syndication.xml";
            Assert.Contains(href, actual);
        }

        [Fact]
        public void IncludeALinkToEachContentPage()
        {
            IServiceProvider serviceProvider = (new ServiceCollection())
                .AddContentRepository()
                .AddLinkProvider()
                .BuildServiceProvider();
            var target = (null as BootstrapProvider).Create(serviceProvider);

            var pages = (null as IEnumerable<ContentItem>).Create(4);
            String outputFileExtension = "html";
            String pathToRoot = string.Empty;

            var actual = target.CreateNavigation(pages, pathToRoot, outputFileExtension);
            foreach (var page in pages)
            {
                String href = $"{page.Slug}.html\"";
                Assert.Contains(href, actual);
            }
        }
    }
}
