using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;
using PPTail.Interfaces;
using PPTail.Enumerations;
using PPTail.Extensions;

namespace PPTail.Generator.ContentPage.Test
{
    public class PageGenerator_Generate_Should
    {
        [Fact]
        public void CallTheTemplateProcessorOncePerExecution()
        {
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();
            var contentItem = (null as ContentItem).Create();
            var templateType = (new List<TemplateType>() { TemplateType.PostPage, TemplateType.ContentPage }).GetRandom();
            var pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContentItemPageGenerator).Create(container);
            var actual = target.Generate(sidebarContent, navigationContent, contentItem, templateType, pathToRoot, xmlEncodeContent);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperTemplate()
        {
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();
            var contentItem = (null as ContentItem).Create();
            var templateType = (new List<TemplateType>() { TemplateType.PostPage, TemplateType.ContentPage }).GetRandom();
            var pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContentItemPageGenerator).Create(container);
            var actual = target.Generate(sidebarContent, navigationContent, contentItem, templateType, pathToRoot, xmlEncodeContent);

            var templates = container.BuildServiceProvider().GetTemplates();
            var template = templates.Find(templateType);
            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(template, It.IsAny<ContentItem>(), It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperContentItem()
        {
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();
            var contentItem = (null as ContentItem).Create();
            var templateType = (new List<TemplateType>() { TemplateType.PostPage, TemplateType.ContentPage }).GetRandom();
            var pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContentItemPageGenerator).Create(container);
            var actual = target.Generate(sidebarContent, navigationContent, contentItem, templateType, pathToRoot, xmlEncodeContent);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), contentItem, It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperSidebarContent()
        {
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();
            var contentItem = (null as ContentItem).Create();
            var templateType = (new List<TemplateType>() { TemplateType.PostPage, TemplateType.ContentPage }).GetRandom();
            var pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContentItemPageGenerator).Create(container);
            var actual = target.Generate(sidebarContent, navigationContent, contentItem, templateType, pathToRoot, xmlEncodeContent);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), sidebarContent, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperNavigationContent()
        {
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();
            var contentItem = (null as ContentItem).Create();
            var templateType = (new List<TemplateType>() { TemplateType.PostPage, TemplateType.ContentPage }).GetRandom();
            var pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContentItemPageGenerator).Create(container);
            var actual = target.Generate(sidebarContent, navigationContent, contentItem, templateType, pathToRoot, xmlEncodeContent);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), navigationContent, It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperPathToRootContent()
        {
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();
            var contentItem = (null as ContentItem).Create();
            var templateType = (new List<TemplateType>() { TemplateType.PostPage, TemplateType.ContentPage }).GetRandom();
            var pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContentItemPageGenerator).Create(container);
            var actual = target.Generate(sidebarContent, navigationContent, contentItem, templateType, pathToRoot, xmlEncodeContent);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), pathToRoot, It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperXmlEncodeValue()
        {
            String sidebarContent = string.Empty.GetRandom();
            String navigationContent = string.Empty.GetRandom();
            var contentItem = (null as ContentItem).Create();
            var templateType = (new List<TemplateType>() { TemplateType.PostPage, TemplateType.ContentPage }).GetRandom();
            var pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContentItemPageGenerator).Create(container);
            var actual = target.Generate(sidebarContent, navigationContent, contentItem, templateType, pathToRoot, xmlEncodeContent);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), xmlEncodeContent), Times.Once);
        }

    }
}
