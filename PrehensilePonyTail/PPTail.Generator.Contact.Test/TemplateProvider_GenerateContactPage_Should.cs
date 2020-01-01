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
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ReturnTheOutputOfTheTemplateProcessor()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            String expected = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor
                .Setup(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expected);
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PassTheCorrectContactTemplateToTheTemplateProcessor()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            var templates = container.BuildServiceProvider().GetService<IEnumerable<Template>>();
            var template = templates.Find(Enumerations.TemplateType.ContactPage);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(template, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectSidebarContentToTheTemplateProcessor()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), sidebarContent, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectNavigationContentToTheTemplateProcessor()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), navigationContent, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPageNameToTheTemplateProcessor()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            String expected = "Contact Me";
            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), expected, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void PassTheCorrectPathToRootToTheTemplateProcessor()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var templateProcessor = new Mock<ITemplateProcessor>();
            container.ReplaceDependency<ITemplateProcessor>(templateProcessor.Object);

            var target = (null as IContactProvider).Create(container);
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            templateProcessor
                .Verify(t => t.ProcessNonContentItemTemplate(It.IsAny<Template>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), pathToRoot), Times.Once);
        }

    }
}
