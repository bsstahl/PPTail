using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Entities;
using PPTail.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace PPTail.Generator.Syndication.Test
{
    public class SyndicationProvider_GenerateFeed_Should
    {
        [Fact]
        public void ReturnValidXml()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            var xml = XElement.Parse(actual);
        }

        [Fact]
        public void ContainsTheCorrectNumberOfItemsIfPostCountIsGreaterThanPostsPerFeed()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var posts = (null as IEnumerable<ContentItem>).Create(siteSettings.PostsPerFeed + 5);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            var xml = XElement.Parse(actual);
            var itemCount = xml.Descendants().Count(n => n.Name.LocalName == "item");
            Assert.Equal(siteSettings.PostsPerFeed, itemCount);
        }

        [Fact]
        public void ContainsTheCorrectNumberOfItemsIfPostCountIsLessThanPostsPerFeed()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var posts = (null as IEnumerable<ContentItem>).Create(siteSettings.PostsPerFeed - 1);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            var xml = XElement.Parse(actual);
            var itemCount = xml.Descendants().Count(n => n.Name.LocalName == "item");
            Assert.Equal(posts.Count(), itemCount);
        }

        [Fact]
        public void ContainEachOfThePostTitles()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var posts = (null as IEnumerable<ContentItem>).Create(siteSettings.PostsPerFeed - 1);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            foreach (var post in posts)
                Assert.Contains(post.Title, actual);
        }

        [Fact]
        public void RetrieveALinkToEachPost()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var settings = (null as ISettings).Create();
            container.ReplaceDependency<ISettings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var posts = (null as IEnumerable<ContentItem>).Create(siteSettings.PostsPerFeed - 1);

            var linkProvider = new Mock<ILinkProvider>();
            foreach (var post in posts)
                linkProvider.Setup(l => l.GetUrl(".", "Posts", post.Slug)).Verifiable();
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            linkProvider.VerifyAll();

            //foreach (var post in posts)
            //{
            //    string url = $"{post.Slug}.{settings.OutputFileExtension}";
            //    Assert.Contains(url, actual);
            //}
        }

        [Fact]
        public void ContainEachOfThePostLinks()
        {
            IServiceCollection container = (null as IServiceCollection).Create();

            var settings = (null as ISettings).Create();
            container.ReplaceDependency<ISettings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var posts = (null as IEnumerable<ContentItem>).Create(siteSettings.PostsPerFeed - 1);

            var linkProvider = new Mock<ILinkProvider>();
            foreach (var post in posts)
                linkProvider.Setup(l => l.GetUrl(It.IsAny<string>(), It.IsAny<string>(), post.Slug))
                    .Returns(post.Id.ToString());
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            linkProvider.VerifyAll();

            foreach (var post in posts)
                Assert.Contains(post.Id.ToString(), actual);
        }
    }
}
