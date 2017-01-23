using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
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
        public void CallTheTemplateProcessorOncePerExecution()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ReturnTheOutputOfTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            string expected = string.Empty.GetRandom();
            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor.Setup(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(expected);
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PassTheCorrectArchiveTemplateToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var searchTemplate = templates.Find(Enumerations.TemplateType.Archive);

            templateProcessor
                .Verify(t => t.Process(searchTemplate, It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectItemTemplateToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var itemTemplate = templates.Find(Enumerations.TemplateType.ArchiveItem);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), itemTemplate, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectSidebarContentToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), sidebarContent, It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectNavigationContentToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), navigationContent, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectContentItemsCollectionToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), posts, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPageNameToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), "Archive", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPathToRootToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), pathToRoot, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectItemSeparatorToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<String>(), string.Empty, It.IsAny<bool>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectXmlEncodeValueToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPostsPerPageValueToTheTemplateProcessor()
        {
            string pathToRoot = ".";
            string navContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create(25.GetRandom(10));
            var pages = (null as IEnumerable<ContentItem>).Create(5.GetRandom(1));

            var container = (null as IServiceCollection).Create();

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var serviceProvider = container.BuildServiceProvider();
            var target = (null as BasicProvider).Create(serviceProvider);
            var actual = target.GenerateArchive(posts, pages, navContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), 0), Times.Once);
        }

    }
}
