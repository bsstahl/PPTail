using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_GeneratePostPage_Should
    {
        [Fact]
        public void ThrowATemplateNotFoundExceptionIfThePostPageTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.PostPage);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");
            var siteSettings = (null as SiteSettings).Create();

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(settings);

            var pageData = (null as ContentItem).Create();
            var target = (null as IPageGenerator).Create(templates, settings);
            Assert.Throws<TemplateNotFoundException>(() => target.GeneratePostPage(string.Empty, string.Empty, pageData));
        }

        [Fact]
        public void ReturnTheProperTemplateTypeIfThePostPageTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.PostPage);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");
            var siteSettings = (null as SiteSettings).Create();

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(settings);

            var pageData = (null as ContentItem).Create();
            var target = (null as IPageGenerator).Create(templates, settings);

            Enumerations.TemplateType actual = Enumerations.TemplateType.Raw;
            try
            {
                target.GeneratePostPage(string.Empty, string.Empty, pageData);
            }
            catch (TemplateNotFoundException ex)
            {
                actual = ex.TemplateType;
            }

            Assert.Equal(Enumerations.TemplateType.PostPage, actual);
        }

        [Fact]
        public void CallTheTemplateProcessorOncePerExecution()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GeneratePostPage(sidebarContent, navigationContent, pageData);

            // templateProcessor.Verify(t => t.ProcessContentItemTemplate(template, pageData, sidebarContent, navigationContent, "..", false), Times.Once);
            // templateProcessor.Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            templateProcessor.Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void ReturnTheOutputOfTheTemplateProcessor()
        {
            string expected = string.Empty.GetRandom();

            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(expected);
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GeneratePostPage(sidebarContent, navigationContent, pageData);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PassTheProperTemplateToTheTemplateProcessor()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GeneratePostPage(sidebarContent, navigationContent, pageData);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var template = templates.Find(Enumerations.TemplateType.PostPage);
            templateProcessor.Verify(t => t.ProcessContentItemTemplate(template, It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void PassTheProperContentItemToTheTemplateProcessor()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GeneratePostPage(sidebarContent, navigationContent, pageData);

            templateProcessor.Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), pageData, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void PassTheProperSidebarContentToTheTemplateProcessor()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GeneratePostPage(sidebarContent, navigationContent, pageData);

            templateProcessor.Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), sidebarContent, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void PassTheProperNavigationContentToTheTemplateProcessor()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GeneratePostPage(sidebarContent, navigationContent, pageData);

            templateProcessor.Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), navigationContent, It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void PassTheProperPathToRootToTheTemplateProcessor()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GeneratePostPage(sidebarContent, navigationContent, pageData);

            string pathToRoot = "..";
            templateProcessor.Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), pathToRoot, It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void PassTheProperXmlEncodeValueToTheTemplateProcessor()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GeneratePostPage(sidebarContent, navigationContent, pageData);

            bool xmlEncode = false;
            templateProcessor.Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), xmlEncode), Times.Once);
        }
    }
}
