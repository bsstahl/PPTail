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
using PPTail.Extensions;

namespace PPTail.Generator.Syndication.Test
{
    public class SyndicationProvider_GenerateFeed_Should
    {
        [Fact]
        public void CallTheTemplateProcessorOncePerExecution()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Verifiable();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            templateProcessor.VerifyAll();
        }

        [Fact]
        public void ReturnTheOutputOfTheTemplateProcessor()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            string expected = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(expected);
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PassTheCorrectSyndicationTemplateToTheTemplateProcessor()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var searchTemplate = templates.Find(Enumerations.TemplateType.Syndication);

            templateProcessor
                .Verify(t => t.Process(searchTemplate, It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectItemTemplateToTheTemplateProcessor()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var itemTemplate = templates.Find(Enumerations.TemplateType.SyndicationItem);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), itemTemplate, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectContentItemsCollectionToTheTemplateProcessor()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), posts, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPageNameToTheTemplateProcessor()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), "Syndication", It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPathToRootToTheTemplateProcessor()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), ".", It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectXmlEncodeValueToTheTemplateProcessor()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), true, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPostsPerFeedValueToTheTemplateProcessor()
        {
            var posts = new List<ContentItem>() { (null as ContentItem).Create() };
            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISyndicationProvider).Create(container);
            var actual = target.GenerateFeed(posts);

            var siteSettings = container.BuildServiceProvider().GetService<SiteSettings>();
            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), siteSettings.PostsPerFeed), Times.Once);
        }

    }
}
