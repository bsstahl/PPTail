using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_GenerateContentPage_Should
    {
        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheContentPageTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.ContentPage);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var pageData = (null as ContentItem).Create();
            var target = (null as IPageGenerator).Create(templates, settings);
            Assert.Throws<TemplateNotFoundException>(() => target.GenerateContentPage(string.Empty, string.Empty, pageData));
        }

        [Fact]
        public void ThrowWithTheProperTemplateTypeIfTheContentPageTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.ContentPage);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var pageData = (null as ContentItem).Create();

            TemplateType expected = TemplateType.ContentPage;
            var target = (null as IPageGenerator).Create(templates, settings);

            try
            {
                target.GenerateContentPage(string.Empty, string.Empty, pageData);
            }
            catch (TemplateNotFoundException ex)
            {
                Assert.Equal(expected, ex.TemplateType);
            }
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
            var actual = target.GenerateContentPage(sidebarContent, navigationContent, pageData);

            // template, pageData, sidebarContent, navigationContent, "..", false
            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperTemplate()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(sidebarContent, navigationContent, pageData);

            // template, pageData, sidebarContent, navigationContent, "..", false
            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var template = templates.Find(TemplateType.ContentPage);
            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(template, It.IsAny<ContentItem>(), It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperContentItem()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(sidebarContent, navigationContent, pageData);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), pageData, It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperSidebarContent()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(sidebarContent, navigationContent, pageData);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), sidebarContent, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperNavigationContent()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(sidebarContent, navigationContent, pageData);

            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), navigationContent, It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperPathToRootContent()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(sidebarContent, navigationContent, pageData);

            string expected = "..";
            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), expected, It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void CallTheTemplateProcessorWithTheProperXmlEncodeValue()
        {
            string sidebarContent = string.Empty.GetRandom();
            string navigationContent = string.Empty.GetRandom();
            var pageData = (null as ContentItem).Create();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(sidebarContent, navigationContent, pageData);

            var expected = false;
            templateProcessor
                .Verify(t => t.ProcessContentItemTemplate(It.IsAny<Template>(), It.IsAny<ContentItem>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), expected), Times.Once);
        }
    }
}
