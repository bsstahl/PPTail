using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class TemplateProcessor_Process_Should
    {
        [Fact]
        public void ThrowDependencyNotFoundExceptionIfSettingsAreNotProvided()
        {
            var templates = (null as IEnumerable<Entities.Template>).Create();
            var pageTemplate = templates.Find(Enumerations.TemplateType.HomePage); // Could be any
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item); // Could be any

            var posts = (null as IEnumerable<ContentItem>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            int maxPostCount = 25.GetRandom(3);

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ISettings>();

            var target = (null as ITemplateProcessor).Create(container);
            Assert.Throws<DependencyNotFoundException>(() => target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfSettingsAreNotProvided()
        {
            var templates = (null as IEnumerable<Entities.Template>).Create();
            var pageTemplate = templates.Find(Enumerations.TemplateType.HomePage); // Could be any
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item); // Could be any

            var posts = (null as IEnumerable<ContentItem>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            int maxPostCount = 25.GetRandom(3);

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ISettings>();

            var target = (null as ITemplateProcessor).Create(container);

            string expected = typeof(ISettings).Name;
            string actual = string.Empty;
            try
            {
                target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfTheLinkProviderIsNotProvided()
        {
            var templates = (null as IEnumerable<Entities.Template>).Create();
            var pageTemplate = templates.Find(Enumerations.TemplateType.HomePage); // Could be any
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item); // Could be any

            var posts = (null as IEnumerable<ContentItem>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            int maxPostCount = 25.GetRandom(3);

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ILinkProvider>();

            var target = (null as ITemplateProcessor).Create(container);
            Assert.Throws<DependencyNotFoundException>(() => target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfTheLinkProviderIsNotProvided()
        {
            var templates = (null as IEnumerable<Entities.Template>).Create();
            var pageTemplate = templates.Find(Enumerations.TemplateType.HomePage); // Could be any
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item); // Could be any

            var posts = (null as IEnumerable<ContentItem>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            int maxPostCount = 25.GetRandom(3);

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ILinkProvider>();

            var target = (null as ITemplateProcessor).Create(container);

            string expected = typeof(ILinkProvider).Name;
            string actual = string.Empty;
            try
            {
                target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);
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
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = string.Empty.GetRandom();
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            int maxPostCount = posts.Count() + 10.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var settings = (null as ISettings).Create();
            settings.ItemSeparator = string.Empty.GetRandom();
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            var expectedSeparatorCount = System.Math.Min(maxPostCount, publishedPosts.Count()) - 1;
            var actualSeparatorCount = System.Text.RegularExpressions.Regex.Matches(actual, settings.ItemSeparator).Count;
            Assert.Equal(expectedSeparatorCount, actualSeparatorCount);
        }

        [Fact]
        public void LimitTheNumberOfPostsUsedTotheValueOfMaxPostCount()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            var posts = (null as IEnumerable<ContentItem>).Create(50.GetRandom(30));
            var publishedPosts = posts.Where(p => p.IsPublished);

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            // maxPostCount will be less than the # of published posts
            int maxPostCount = (publishedPosts.Count() - 5).GetRandom(5);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            // Verify that none of the published posts beyond maxPostCount are in the output
            var excludedPosts = publishedPosts.OrderByDescending(p => p.PublicationDate).Skip(maxPostCount);
            foreach (var post in excludedPosts)
                Assert.DoesNotContain(post.Content, actual);
        }

        [Fact]
        public void NotLimitTheNumberOfPostsUsedIfTheMaxPostCountIsZero()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            var posts = (null as IEnumerable<ContentItem>).Create(50.GetRandom(30));
            var publishedPosts = posts.Where(p => p.IsPublished);

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            // Verify that all of the published posts are in the output
            foreach (var post in publishedPosts)
                Assert.Contains(post.Content, actual);
        }

        [Fact]
        public void NotPublishUnpublishedPosts()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            var posts = (null as IEnumerable<ContentItem>).Create(50.GetRandom(30));
            var publishedPosts = posts.Where(p => p.IsPublished);

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            int maxPostCount = 0;

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            // Verify that none of the unpublished posts are in the output
            var excludedPosts = posts.Where(p => !p.IsPublished);
            foreach (var post in excludedPosts)
                Assert.DoesNotContain(post.Content, actual);
        }

        [Fact]
        public void ReplaceTheTitlePlaceHolderWithTheTitleOfEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Title}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.Title, actual);
        }

        [Fact]
        public void ReplaceTheContentPlaceHolderWithTheContentOfEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.Content, actual);
        }

        [Fact]
        public void ReplaceTheAuthorPlaceHolderWithTheAuthorOfEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Author}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.Author, actual);
        }

        [Fact]
        public void ReplaceTheDescriptionPlaceHolderWithTheDescriptionOfEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Description}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.Description, actual);
        }

        [Fact]
        public void ReplaceTheByLinePlaceHolderWithTheByLineOfEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{ByLine}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
                Assert.Contains(post.ByLine, actual);
        }

        [Fact]
        public void ReplaceThePublicationDatePlaceHolderWithThePubDateOfEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{PublicationDate}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
            {
                string expected = post.PublicationDate.ToString(settings.DateTimeFormatSpecifier);
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        public void ReplaceTheLastModificationDatePlaceHolderWithTheLastModDateOfEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{LastModificationDate}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in posts.Where(p => p.IsPublished))
            {
                string expected = post.LastModificationDate.ToString(settings.DateTimeFormatSpecifier);
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        public void CallTheLinkProviderWithTheCorrectDataToCalculateTheLinkForEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Link}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                linkProvider.Verify(l => l.GetUrl(pathToRoot, "Posts", post.Slug), Times.Once);
        }

        [Fact]
        public void ReplaceTheLinkPlaceHolderWithTheOutputOfTheLinkProvider()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Link}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

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
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                Assert.Contains(post.Id.ToString(), actual);
        }

        [Fact]
        public void CallTheLinkProviderWithTheCorrectDataToCalculateThePermalinkForEachPost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Permalink}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                linkProvider.Verify(l => l.GetUrl(pathToRoot, "Permalinks", post.Id.ToString()), Times.Once);
        }

        [Fact]
        public void ReplaceThePermalinkPlaceHolderWithThePermalinkText()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Permalink}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            IEnumerable<ContentItem> posts = new List<ContentItem>();
            while (!posts.Any(p => p.IsPublished))
                posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var linkProvider = new Mock<ILinkProvider>();
            foreach (var post in publishedPosts)
                linkProvider
                    .Setup(l => l.GetUrl(It.IsAny<string>(), It.IsAny<String>(), post.Id.ToString()))
                    .Returns(post.Slug);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            var expectedCount = publishedPosts.Count();
            var actualCount = System.Text.RegularExpressions.Regex.Matches(actual, ">Permalink<").Count;

            System.Diagnostics.Debug.Assert(expectedCount > 0);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void ReplaceThePermalinkUrlPlaceHolderWithTheOutputOfTheLinkProvider()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{PermalinkUrl}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

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
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                Assert.Contains(post.Slug, actual);
        }

        [Fact]
        public void CallTheContentEncoderOnceWithTheProperDataToGetTheFilenameForEachTagLink()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Tags}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

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
                    contentEncoder.Setup(e => e.UrlEncode(tag)).Returns(valueFunction).Verifiable();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            contentEncoder.VerifyAll();
        }

        [Fact]
        public void CallTheLinkProviderOnceWithTheProperDataToGetTheCorrectLinkToEachTagPage()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Tags}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();
            var contentEncoder = new Mock<IContentEncoder>();

            Func<string, string> valueFunction = p => p;
            foreach (var post in publishedPosts)
                foreach (var tag in post.Tags)
                {
                    contentEncoder.Setup(e => e.UrlEncode(tag)).Returns(valueFunction);
                    linkProvider.Setup(l => l.GetUrl(pathToRoot, "search", tag))
                        .Returns(post.Slug).Verifiable();
                }

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            linkProvider.VerifyAll();
        }

        [Fact]
        public void ReturnsTheOutputOfTheLinkProviderWithinEachTagPageLink()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Tags}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create();
            var publishedPosts = posts.Where(p => p.IsPublished);

            var linkProvider = new Mock<ILinkProvider>();
            var contentEncoder = new Mock<IContentEncoder>();

            Func<string, string> valueFunction = p => p;
            foreach (var post in publishedPosts)
                foreach (var tag in post.Tags)
                {
                    contentEncoder.Setup(e => e.UrlEncode(tag)).Returns(valueFunction);
                    linkProvider.Setup(l => l.GetUrl(pathToRoot, "search", tag))
                        .Returns(post.Id.ToString());
                }

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                Assert.Contains(post.Id.ToString(), actual);
        }

        [Fact]
        public void CallTheContentEncoderOnceWithTheProperDataToGetTheFilenameForEachCategoryLink()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Categories}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

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
                    contentEncoder.Setup(e => e.UrlEncode(categoryId.GetCategoryName(categoryList)))
                        .Returns(categoryValueFunction).Verifiable();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            contentEncoder.VerifyAll();
        }

        [Fact]
        public void CallTheLinkProviderOnceWithTheProperDataToGetTheCorrectLinkToEachCategoryPage()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Categories}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

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
                    string categoryName = categoryId.GetCategoryName(categoryList);
                    contentEncoder.Setup(e => e.UrlEncode(categoryName)).Returns(encoderValueFunction);
                    linkProvider.Setup(l => l.GetUrl(pathToRoot, "search", categoryName))
                        .Returns(linkValueFunction).Verifiable();
                }

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            linkProvider.VerifyAll();
        }

        public void ReturnsTheOutputOfTheLinkProviderWithinEachCategoryPageLink()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Categories}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

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
                    string name = categoryId.GetCategoryName(categoryList);
                    contentEncoder.Setup(e => e.UrlEncode(name)).Returns(encoderValueFunction);
                    linkProvider.Setup(l => l.GetUrl(pathToRoot, "search", name))
                        .Returns(post.Id.ToString());
                }

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            foreach (var post in publishedPosts)
                Assert.Contains(post.Id.ToString(), actual);
        }

        [Fact]
        public void NotThrowAnExceptionIfNoCategoriesAreOnThePost()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Categories}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };
            var settings = (null as ISettings).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = false;
            int maxPostCount = 0;

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
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);
        }

        [Fact]
        public void PassesTheContentToTheEncoderIfEncodingIsSelected()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            var encoder = new Mock<IContentEncoder>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<IContentEncoder>(encoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            encoder.Verify(e => e.XmlEncode(post.Content), Times.Once);
        }

        [Fact]
        public void ReplaceTheContentPlaceHolderWithTheEncoderOutputIfEncodingIsSelected()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Content}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;
            string expected = string.Empty.GetRandom();

            var encoder = new Mock<IContentEncoder>();
            encoder.Setup(e => e.XmlEncode(post.Content)).Returns(expected);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<IContentEncoder>(encoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void PassesTheDescriptionToTheEncoderIfEncodingIsSelected()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Description}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            var encoder = new Mock<IContentEncoder>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<IContentEncoder>(encoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            encoder.Verify(e => e.XmlEncode(post.Description), Times.Once);
        }

        [Fact]
        public void ReplaceTheDescriptionPlaceHolderWithTheEncoderOutputIfEncodingIsSelected()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{Description}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;
            string expected = string.Empty.GetRandom();

            var encoder = new Mock<IContentEncoder>();
            encoder.Setup(e => e.XmlEncode(post.Description)).Returns(expected);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            container.ReplaceDependency<IContentEncoder>(encoder.Object);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void UsesTheXmlFormatForPubDateIfEncodingIsSelected()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{PublicationDate}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            string expected = post.PublicationDate.ToString("o");

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void UsesTheXmlFormatForLastModDateIfEncodingIsSelected()
        {
            string pageTemplateContent = "-----{Content}-----";
            string itemTemplateContent = "*****{LastModificationDate}*****";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var itemTemplate = new Entities.Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };
            var templates = new List<Entities.Template>() { pageTemplate, itemTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true;
            int maxPostCount = 0;

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();
            post.IsPublished = true;

            string expected = post.LastModificationDate.ToString("o");

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);

            Assert.Contains(expected, actual);
        }


    }
}
