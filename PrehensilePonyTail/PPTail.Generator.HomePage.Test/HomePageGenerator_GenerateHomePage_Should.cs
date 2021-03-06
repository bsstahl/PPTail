﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.HomePage.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HomePageGenerator_GenerateHomePage_Should
    {
        [Fact]
        public void CallTheTemplateProcessorOncePerExecution()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void ReturnTheOutputOfTheTemplateProcessor()
        {
            String expected = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()))
                .Returns(expected);
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            var actual = target.GenerateHomepage(sidebarContent, navigationContent, posts);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PassTheProperPageTemplateToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            var homepageTemplate = templates.Find(Enumerations.TemplateType.HomePage);
            templateProcessor.Verify(t => t.Process(homepageTemplate, It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void PassTheProperItemTemplateToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            var itemTemplate = templates.Find(Enumerations.TemplateType.Item);
            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), itemTemplate, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void PassTheProperSidebarContentToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), sidebarContent, It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void PassTheProperNavigationContentToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), navigationContent, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void PassTheProperPostsToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), posts, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }
        
        [Fact]
        public void PassTheProperPageTitleToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            String pageTitle = "Home";
            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), pageTitle, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void PassTheProperPathToRootToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            String pathToRoot = ".";
            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), pathToRoot, It.IsAny<string>(), It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void PassTheProperItemSeparatorToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var siteSettings = new SiteSettings()
            {
                ItemSeparator = string.Empty.GetRandom()
            };

            var container = (null as IServiceCollection).Create(siteSettings);
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), siteSettings.ItemSeparator, It.IsAny<Boolean>(), It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void PassTheProperXmlEncodeContentValueToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            bool xmlEncodeContent = false;
            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), xmlEncodeContent, It.IsAny<Int32>()), Times.Once);
        }

        [Fact]
        public void PassTheProperPostsPerPageValueToTheTemplateProcessor()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();

            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceTemplateRepo(templates);

            var contentRepo = new Mock<IContentRepository>();
            var siteSettings = (null as SiteSettings).Create();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IHomePageGenerator).Create(container);
            target.GenerateHomepage(sidebarContent, navigationContent, posts);

            templateProcessor.Verify(t => t.Process(It.IsAny<Template>(), It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Boolean>(), siteSettings.PostsPerPage), Times.Once);
        }
    }
}
