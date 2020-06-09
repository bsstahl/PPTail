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
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Verifiable();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor.VerifyAll();
        }

        [Fact]
        public void ReturnTheOutputOfTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            String expected = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(expected);
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PassTheCorrectSearchTemplateToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            var templates = container.BuildServiceProvider().GetTemplates();
            var searchTemplate = templates.Find(Enumerations.TemplateType.SearchPage);

            templateProcessor
                .Verify(t => t.Process(searchTemplate, It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectItemTemplateToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            var templates = container.BuildServiceProvider().GetTemplates();
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), itemTemplate, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectSidebarContentToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), sidebarContent, It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectNavigationContentToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), navigationContent, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectContentItemsCollectionForTheTagToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var post = (null as ContentItem).Create(tag);
            var posts = new List<ContentItem>() { post, (null as ContentItem).Create(string.Empty.GetRandom(5)) };

            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            var selectedContentItems = new List<ContentItem>() { post };
            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), selectedContentItems, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectContentItemsCollectionForTheCategoryToTheTemplateProcessor()
        {
            var categories = (null as IEnumerable<Category>).Create(5);
            var category = categories.GetRandom();

            var post = (null as ContentItem).Create(category.Id);
            var posts = new List<ContentItem>() { post, (null as ContentItem).Create(Guid.NewGuid()) };

            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create(categories);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(category.Name, posts, navigationContent, sidebarContent, pathToRoot);

            var selectedContentItems = new List<ContentItem>() { post };
            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), selectedContentItems, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPageNameToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), $"Tag: {tag}", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPathToRootToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), pathToRoot, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectXmlEncodeValueToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPostsPerPageValueToTheTemplateProcessor()
        {
            // For search pages, the number of posts should be unlimited, 
            // thus the value passed in PostsPerPage should be zero
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), 0), Times.Once);
        }

        [Fact]
        public void PassTheCorrectItemSeparatorToTheTemplateProcessor()
        {
            String tag = string.Empty.GetRandom();
            var posts = new List<ContentItem>() { (null as ContentItem).Create(tag) };
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var siteSettings = new SiteSettings();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create(contentRepo.Object);
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as ISearchProvider).Create(container);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), settings.ItemSeparator, It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

    }
}
