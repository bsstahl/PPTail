using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using TestHelperExtensions;
using PPTail.Exceptions;
using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;

namespace PPTail.Generator.Template.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TemplateProcessor_Process_Should
    {
        [Fact]
        public void ThrowDependencyNotFoundExceptionIfTheLinkProviderIsNotProvided()
        {
            var templates = (null as IEnumerable<Entities.Template>).Create();
            var pageTemplate = templates.Find(Enumerations.TemplateType.HomePage); // Could be any
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item); // Could be any

            var posts = (null as IEnumerable<ContentItem>).Create();

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            Int32 maxPostCount = 25.GetRandom(3);

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ILinkProvider>();

            var target = (null as ITemplateProcessor).Create(container);
            Assert.Throws<DependencyNotFoundException>(() => target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfTheLinkProviderIsNotProvided()
        {
            var templates = (null as IEnumerable<Entities.Template>).Create();
            var pageTemplate = templates.Find(Enumerations.TemplateType.HomePage); // Could be any
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item); // Could be any

            var posts = (null as IEnumerable<ContentItem>).Create();

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            Int32 maxPostCount = 25.GetRandom(3);

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ILinkProvider>();

            var target = (null as ITemplateProcessor).Create(container);

            String expected = typeof(ILinkProvider).Name;
            String actual = string.Empty;
            try
            {
                target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UseTheProperItemSeparatorToDelimitTheItems()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = string.Empty.GetRandom();
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            Int32 maxPostCount = posts.Count() + 10.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var siteSettings = new SiteSettings()
            {
                ItemSeparator = string.Empty.GetRandom()
            };
            var contentRepo = (null as IContentRepository).Create(siteSettings);
            container.ReplaceDependency<IContentRepository>(contentRepo);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, siteSettings.ItemSeparator, xmlEncodeContent, maxPostCount);

            var expectedSeparatorCount = System.Math.Min(maxPostCount, publishedPosts.Count()) - 1;
            var actualSeparatorCount = System.Text.RegularExpressions.Regex.Matches(actual, siteSettings.ItemSeparator).Count;
            Assert.Equal(expectedSeparatorCount, actualSeparatorCount);
        }

        [Fact]
        public void LimitTheNumberOfPostsUsedTotheValueOfMaxPostCount()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            var posts = (null as IEnumerable<ContentItem>).Create(50.GetRandom(30));
            var publishedPosts = posts.Where(p => p.IsPublished);

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            // maxPostCount will be less than the # of published posts
            Int32 maxPostCount = (publishedPosts.Count() - 5).GetRandom(5);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            // Verify that none of the published posts beyond maxPostCount are in the output
            var excludedPosts = publishedPosts.OrderByDescending(p => p.PublicationDate).Skip(maxPostCount);
            foreach (var post in excludedPosts)
                Assert.DoesNotContain(post.Content, actual);
        }

        [Fact]
        public void NotLimitTheNumberOfPostsUsedIfTheMaxPostCountIsZero()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            var posts = (null as IEnumerable<ContentItem>).Create(50.GetRandom(30));
            var publishedPosts = posts.Where(p => p.IsPublished);

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            // Verify that all of the published posts are in the output
            foreach (var post in publishedPosts)
                Assert.Contains(post.Content, actual);
        }

        [Fact]
        public void NotPublishUnpublishedPosts()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            var posts = (null as IEnumerable<ContentItem>).Create(50.GetRandom(30));
            var publishedPosts = posts.Where(p => p.IsPublished);

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            Int32 maxPostCount = 0;

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            // Verify that none of the unpublished posts are in the output
            var excludedPosts = posts.Where(p => !p.IsPublished);
            foreach (var post in excludedPosts)
                Assert.DoesNotContain(post.Content, actual);
        }

        [Fact]
        public void ReplaceTheTitlePlaceHolderWithTheTitleOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Title}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.Title, actual);
        }

        [Fact]
        public void ReplaceTheContentPlaceHolderWithTheContentOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.Content, actual);
        }

        [Fact]
        public void ReplaceTheAuthorPlaceHolderWithTheAuthorOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Author}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.Author, actual);
        }

        [Fact]
        public void ReplaceTheDescriptionPlaceHolderWithTheDescriptionOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Description}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.Description, actual);
        }

        [Fact]
        public void ReplaceTheByLinePlaceHolderWithTheByLineOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{ByLine}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.ByLine, actual);
        }

        [Fact]
        public void ReplaceThePublicationDatePlaceHolderWithThePubDateOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{PublicationDate}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var mockContentRepo = new Mock<IContentRepository>();
            var siteSettings = new SiteSettings()
            {
                DateFormatSpecifier = "yyyymmddHmm"
            };

            var container = (null as IServiceCollection).Create(mockContentRepo, siteSettings);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
            {
                String expected = post.PublicationDate.ToString(siteSettings.DateFormatSpecifier);
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        public void ReplaceThePublicationDateTimePlaceHolderWithThePubDateTimeOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{PublicationDateTime}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var mockContentRepo = new Mock<IContentRepository>();
            var siteSettings = new SiteSettings()
            {
                DateFormatSpecifier = "yyyymmddHmm"
            };

            var container = (null as IServiceCollection).Create(mockContentRepo, siteSettings);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
            {
                String expected = post.PublicationDate.ToString(siteSettings.DateTimeFormatSpecifier);
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        public void ReplaceTheLastModificationDatePlaceHolderWithTheLastModDateOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{LastModificationDate}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var mockContentRepo = new Mock<IContentRepository>();
            var siteSettings = new SiteSettings()
            {
                DateFormatSpecifier = "yyyymmddHmm"
            };

            var container = (null as IServiceCollection).Create(mockContentRepo, siteSettings);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
            {
                String expected = post.LastModificationDate.ToString(siteSettings.DateFormatSpecifier);
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        public void ReplaceTheLastModificationDateTimePlaceHolderWithTheLastModDateTimeOfEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{LastModificationDateTime}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var mockContentRepo = new Mock<IContentRepository>();
            var siteSettings = new SiteSettings()
            {
                DateFormatSpecifier = "yyyymmddHmm"
            };

            var container = (null as IServiceCollection).Create(mockContentRepo, siteSettings);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
            {
                String expected = post.LastModificationDate.ToString(siteSettings.DateTimeFormatSpecifier);
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        public void CallTheLinkProviderWithTheCorrectDataToCalculateTheLinkForEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Link}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                linkProvider.Verify(l => l.GetUrl(pathToRoot, "Posts", post.Slug), Times.Once);
        }

        [Fact]
        public void ReplaceTheLinkPlaceHolderWithTheOutputOfTheLinkProvider()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Link}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();
            foreach (var post in publishedPosts)
                linkProvider
                    .Setup(l => l.GetUrl(It.IsAny<string>(), It.IsAny<String>(), post.Slug))
                    .Returns(post.Id.ToString());

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                Assert.Contains(post.Id.ToString(), actual);
        }

        [Fact]
        public void CallTheLinkProviderWithTheCorrectDataToCalculateThePermalinkForEachPost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Permalink}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                linkProvider.Verify(l => l.GetUrl(pathToRoot, "Permalinks", post.Id.ToString()), Times.Once);
        }

        [Fact]
        public void ReplaceThePermalinkPlaceHolderWithThePermalinkText()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Permalink}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            IEnumerable<ContentItem> posts = new List<ContentItem>();
            while (!posts.Any(p => p.IsPublished))
                posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var linkProvider = new Mock<ILinkProvider>();
            foreach (var post in publishedPosts)
                linkProvider
                    .Setup(l => l.GetUrl(It.IsAny<string>(), It.IsAny<String>(), post.Id.ToString()))
                    .Returns(post.Slug);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            var expectedCount = publishedPosts.Count();
            var actualCount = System.Text.RegularExpressions.Regex.Matches(actual, ">Permalink<").Count;

            System.Diagnostics.Debug.Assert(expectedCount > 0);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ReplaceThePermalinkUrlPlaceHolderWithTheOutputOfTheLinkProvider()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{PermalinkUrl}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();
            foreach (var post in publishedPosts)
                linkProvider
                    .Setup(l => l.GetUrl(It.IsAny<string>(), It.IsAny<String>(), post.Id.ToString()))
                    .Returns(post.Slug);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                Assert.Contains(post.Slug, actual);
        }

        [Fact]
        public void CallTheContentEncoderOnceWithTheProperDataToGetTheFilenameForEachTagLink()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Tags}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();
            Func<string, string, string, string> linkValueFunction = (a, b, c) => c;
            foreach (var post in publishedPosts)
                linkProvider
                    .Setup(l => l.GetUrl(It.IsAny<string>(), It.IsAny<String>(), It.IsAny<string>()))
                    .Returns(linkValueFunction);

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            foreach (var post in publishedPosts)
                foreach (var tag in post.Tags)
                    contentEncoder.Setup(e => e.UrlEncode(tag))
                        .Returns(valueFunction)
                        .Verifiable();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            contentEncoder.VerifyAll();
        }

        [Fact]
        public void CallTheLinkProviderOnceWithTheProperDataToGetTheCorrectLinkToEachTagPage()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Tags}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();
            var contentEncoder = new Mock<IContentEncoder>();

            Func<string, string> valueFunction = p => p;
            foreach (var post in publishedPosts)
                foreach (var tag in post.Tags)
                {
                    contentEncoder.Setup(e => e.UrlEncode(tag)).Returns(valueFunction);
                    linkProvider.Setup(l => l.GetUrl(pathToRoot, "Search", tag))
                        .Returns(post.Slug).Verifiable();
                }

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            linkProvider.VerifyAll();
        }

        [Fact]
        public void ReturnsTheOutputOfTheLinkProviderWithinEachTagPageLink()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Tags}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();
            var contentEncoder = new Mock<IContentEncoder>();

            Func<string, string> valueFunction = p => p;
            foreach (var post in publishedPosts)
                foreach (var tag in post.Tags)
                {
                    contentEncoder.Setup(e => e.UrlEncode(tag)).Returns(valueFunction);
                    linkProvider.Setup(l => l.GetUrl(pathToRoot, "Search", tag))
                        .Returns(post.Id.ToString());
                }

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                Assert.Contains(post.Id.ToString(), actual);
        }

        [Fact]
        public void CallTheContentEncoderOnceWithTheProperDataToGetTheFilenameForEachCategoryLink()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Categories}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var categoryIdList = posts.SelectMany(p => p.CategoryIds);
            var publishedCategoryIdList = publishedPosts.SelectMany(p => p.CategoryIds);

            var categoryList = categoryIdList.CreateCategories();

            var linkProvider = new Mock<ILinkProvider>();
            Func<string, string, string, string> linkValueFunction = (a, b, c) => $"{a}_{b}_{c}";
            Func<string, string> encoderValueFunction = p => p;
            foreach (var post in publishedPosts)
                linkProvider
                    .Setup(l => l.GetUrl(It.IsAny<string>(), It.IsAny<String>(), It.IsAny<string>()))
                    .Returns(linkValueFunction);

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> categoryValueFunction = p => p;
            foreach (var post in publishedPosts)
                foreach (var categoryId in post.CategoryIds)
                {
                    var categoryName = categoryId.GetCategoryName(categoryList);
                    contentEncoder
                        .Setup(e => e.UrlEncode(categoryName))
                        .Returns(categoryValueFunction)
                        .Verifiable();
                }

            var contentRepo = new Mock<IContentRepository>();
            contentRepo
                .Setup(r => r.GetCategories())
                .Returns(categoryList);
            contentRepo
                .Setup(r => r.GetSiteSettings())
                .Returns((null as SiteSettings).Create());

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            contentEncoder.VerifyAll();
        }

        [Fact]
        public void CallTheLinkProviderOnceWithTheProperDataToGetTheCorrectLinkToEachCategoryPage()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Categories}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var categoryIdList = posts.SelectMany(p => p.CategoryIds);
            var publishedCategoryIdList = publishedPosts.SelectMany(p => p.CategoryIds);

            var categoryList = categoryIdList.CreateCategories();

            var linkProvider = new Mock<ILinkProvider>();
            var contentEncoder = new Mock<IContentEncoder>();

            Func<string, string, string, string> linkValueFunction = (a, b, c) => $"{a}_{b}_{c}";
            Func<string, string> encoderValueFunction = p => p;
            foreach (var post in publishedPosts)
                foreach (var categoryId in post.CategoryIds)
                {
                    String categoryName = categoryId.GetCategoryName(categoryList);
                    contentEncoder.Setup(e => e.UrlEncode(categoryName)).Returns(encoderValueFunction);
                    linkProvider.Setup(l => l.GetUrl(pathToRoot, "Search", categoryName))
                        .Returns(linkValueFunction).Verifiable();
                }

            var contentRepo = new Mock<IContentRepository>();
            contentRepo
                .Setup(r => r.GetCategories())
                .Returns(categoryList);
            contentRepo
                .Setup(r => r.GetSiteSettings())
                .Returns((null as SiteSettings).Create());

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            linkProvider.VerifyAll();
        }

        [Fact]
        public void ReturnsTheOutputOfTheLinkProviderWithinEachCategoryPageLink()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Categories}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var categoryIdList = posts.SelectMany(p => p.CategoryIds);
            var publishedCategoryIdList = publishedPosts.SelectMany(p => p.CategoryIds);

            var categoryList = categoryIdList.CreateCategories();

            var linkProvider = new Mock<ILinkProvider>();
            var contentEncoder = new Mock<IContentEncoder>();

            Func<string, string> encoderValueFunction = p => p;
            foreach (var post in publishedPosts)
                foreach (var categoryId in post.CategoryIds)
                {
                    String name = categoryId.GetCategoryName(categoryList);
                    contentEncoder.Setup(e => e.UrlEncode(name))
                        .Returns(encoderValueFunction);

                    linkProvider.Setup(l => l.GetUrl(pathToRoot, "Search", name))
                        .Returns(post.Id.ToString());
                }

            var contentRepo = new Mock<IContentRepository>();
            contentRepo
                .Setup(r => r.GetCategories())
                .Returns(categoryList);
            contentRepo
                .Setup(r => r.GetSiteSettings())
                .Returns((null as SiteSettings).Create());

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                Assert.Contains(post.Id.ToString(), actual);
        }

        [Fact]
        public void NotThrowAnExceptionIfNoCategoriesAreOnThePost()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Categories}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;
            post.CategoryIds = new List<Guid>();

            var categoryIdList = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            var categoryList = categoryIdList.CreateCategories();

            var linkProvider = new Mock<ILinkProvider>();
            var contentEncoder = new Mock<IContentEncoder>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);
        }

        [Fact]
        public void PassesTheContentToTheEncoderIfEncodingIsSelected()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            var encoder = new Mock<IContentEncoder>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<IContentEncoder>(encoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            encoder.Verify(e => e.XmlEncode(post.Content), Times.Once);
        }

        [Fact]
        public void ReplaceTheContentPlaceHolderWithTheEncoderOutputIfEncodingIsSelected()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;
            String expected = string.Empty.GetRandom();

            var encoder = new Mock<IContentEncoder>();
            encoder.Setup(e => e.XmlEncode(post.Content)).Returns(expected);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<IContentEncoder>(encoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void PassesTheDescriptionToTheEncoderIfEncodingIsSelected()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Description}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            var encoder = new Mock<IContentEncoder>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<IContentEncoder>(encoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            encoder.Verify(e => e.XmlEncode(post.Description), Times.Once);
        }

        [Fact]
        public void ReplaceTheDescriptionPlaceHolderWithTheEncoderOutputIfEncodingIsSelected()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Description}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;
            String expected = string.Empty.GetRandom();

            var encoder = new Mock<IContentEncoder>();
            encoder.Setup(e => e.XmlEncode(post.Description)).Returns(expected);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<IContentEncoder>(encoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void UsesTheXmlFormatForPubDateIfEncodingIsSelected()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{PublicationDate}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            String expected = post.PublicationDate.Date.ToString("o");

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void UsesTheXmlFormatForPubDateTimeIfEncodingIsSelected()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{PublicationDateTime}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            String expected = post.PublicationDate.ToString("o");

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void UsesTheXmlFormatForLastModDateIfEncodingIsSelected()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{LastModificationDate}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            String expected = post.LastModificationDate.Date.ToString("o");

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void UsesTheXmlFormatForLastModDateTimeIfEncodingIsSelected()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{LastModificationDateTime}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            Int32 maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            String expected = post.LastModificationDate.ToString("o");

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void ReplaceAPageLinkWithTheProperLinkTagContents()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Content}*****";

            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            String pageLinkSlug = string.Empty.GetRandom();
            String pageLinkDescription = string.Empty.GetRandom();
            String content = $"======={{PageLink:{pageLinkSlug}|{pageLinkDescription}}}=======";

            var linkedContentItem = (null as ContentItem).Create();
            linkedContentItem.Slug = pageLinkSlug;
            linkedContentItem.IsPublished = true;

            var linkedItemRepo = new Mock<IContentRepository>();
            linkedItemRepo
                .Setup(r => r.GetAllPages())
                .Returns(new[] { linkedContentItem });

            var contentItem = (null as ContentItem).Create();
            contentItem.IsPublished = true;
            contentItem.Content = content;
            var posts = new List<ContentItem>() { contentItem };

            var container = (null as IServiceCollection).Create(linkedItemRepo);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            string expected = $">{pageLinkDescription}<";
            Assert.Contains(expected, actual);
        }

        [Fact]
        public void ReplaceAPageLinkWithTheProperLinkTagAnchor()
        {
            String pageTemplateContent = "-----{Content}-----";
            String itemTemplateContent = "*****{Content}*****";

            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            Int32 maxPostCount = 0;

            String pageLinkSlug = string.Empty.GetRandom();
            String pageLinkDescription = string.Empty.GetRandom();
            String content = $"======={{PageLink:{pageLinkSlug}|{pageLinkDescription}}}=======";

            var linkedContentItem = (null as ContentItem).Create();
            linkedContentItem.Slug = pageLinkSlug;
            linkedContentItem.IsPublished = true;

            var linkedItemRepo = new Mock<IContentRepository>();
            linkedItemRepo
                .Setup(r => r.GetAllPages())
                .Returns(new[] { linkedContentItem });

            var contentItem = (null as ContentItem).Create();
            contentItem.IsPublished = true;
            contentItem.Content = content;
            var posts = new List<ContentItem>() { contentItem };

            string expectedLink = string.Empty.GetRandom();
            var linkProvider = new Mock<ILinkProvider>();
            linkProvider
                .Setup(l => l.GetUrl(pathToRoot, "Pages", linkedContentItem.Slug))
                .Returns(expectedLink);

            var container = (null as IServiceCollection).Create(linkedItemRepo);
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, ";", xmlEncodeContent, maxPostCount);

            string expected = $"<a href=\"{expectedLink}\">";
            Assert.Contains(expected, actual);
        }

    }
}
