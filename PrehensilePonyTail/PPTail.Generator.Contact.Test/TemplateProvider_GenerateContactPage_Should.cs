using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Interfaces;
using PPTail.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Extensions;

namespace PPTail.Generator.Contact.Test
{
    public class TemplateProvider_GenerateContactPage_Should
    {
        [Fact]
        public void CallTheTemplateProcessorOncePerExecution()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ReturnTheOutputOfTheTemplateProcessor()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            string expected = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expected);
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PassTheCorrectContactTemplateToTheTemplateProcessor()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var template = templates.Find(Enumerations.TemplateType.ContactPage);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(template, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectSidebarContentToTheTemplateProcessor()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), sidebarContent, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectNavigationContentToTheTemplateProcessor()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), navigationContent, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPageNameToTheTemplateProcessor()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            string expected = "Contact Me";
            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), expected), Times.Once);
        }

    }
}
