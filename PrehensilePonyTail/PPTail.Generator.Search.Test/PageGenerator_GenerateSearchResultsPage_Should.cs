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
using PPTail.Extensions;

namespace PPTail.Generator.Search.Test
{
    public class PageGenerator_GenerateSearchResultsPage_Should
    {
        [Fact]
        public void CallTheTemplateProcessorOncePerExecution()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Verifiable();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor.VerifyAll();
        }

        [Fact]
        public void ReturnTheOutputOfTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            string expected = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(expected);
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PassTheCorrectSearchTemplateToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var searchTemplate = templates.Find(Enumerations.TemplateType.SearchPage);

            templateProcessor
                .Verify(t => t.Process(searchTemplate, It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectItemTemplateToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), itemTemplate, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectSidebarContentToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), sidebarContent, It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectNavigationContentToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), navigationContent, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectContentItemsCollectionToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), posts, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPageNameToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), $"Tag: {tag}", It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPathToRootToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), pathToRoot, It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectXmlEncodeValueToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), false, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPostsPerPageValueToTheTemplateProcessor()
        {
            string tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            var siteSettings = container.BuildServiceProvider().GetService<SiteSettings>();
            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), siteSettings.PostsPerPage), Times.Once);
        }

    }
}
