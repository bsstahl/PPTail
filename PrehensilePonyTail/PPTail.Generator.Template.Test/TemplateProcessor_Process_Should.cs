using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using TestHelperExtensions;
using PPTail.Exceptions;
using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;

namespace PPTail.Generator.Template.Test
{
    public class TemplateProcessor_Process_Should
    {
        [Fact]
        public void ThrowDependencyNotFoundExceptionIfSettingsAreNotProvided()
        {
            var templates = (null as IEnumerable<Entities.Template>).Create();
            var pageTemplate = templates.Find(Enumerations.TemplateType.HomePage); // Could be any
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item); // Could be any

            var posts = (null as IEnumerable<ContentItem>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            int maxPostCount = 25.GetRandom(3);

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ISettings>();

            var target = (null as ITemplateProcessor).Create(container);
            Assert.Throws<DependencyNotFoundException>(() => target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfSettingsAreNotProvided()
        {
            var templates = (null as IEnumerable<Entities.Template>).Create();
            var pageTemplate = templates.Find(Enumerations.TemplateType.HomePage); // Could be any
            var itemTemplate = templates.Find(Enumerations.TemplateType.Item); // Could be any

            var posts = (null as IEnumerable<ContentItem>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();
            bool xmlEncodeContent = true.GetRandom();
            int maxPostCount = 25.GetRandom(3);

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ISettings>();

            var target = (null as ITemplateProcessor).Create(container);

            string expected = typeof(ISettings).Name;
            string actual = string.Empty;
            try
            {
                target.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, pageTitle, pathToRoot, xmlEncodeContent, maxPostCount);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal(expected, actual);
        }

    }
}
