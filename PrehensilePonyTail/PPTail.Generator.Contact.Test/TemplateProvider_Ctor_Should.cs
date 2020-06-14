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

namespace PPTail.Generator.Contact.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TemplateProvider_Ctor_Should
    {
        [Fact]
        public void ThrowArgumentNullExceptionIfServiceProviderNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new TemplateProvider(null));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfSiteSettingsAreNotProvided()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var template = (null as Template).Create();
            var templates = new List<Template>() { template };

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(templates);

            var container = new ServiceCollection();
            container.AddSingleton<ITemplateRepository>(templateRepo.Object);
            container.AddSingleton<ISettings>(Mock.Of<ISettings>());

            String expected = typeof(SiteSettings).Name;
            try
            {
                var target = new TemplateProvider(container.BuildServiceProvider());
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowTemplateNotFoundExceptionIfContactPageTemplateNotProvided()
        {
            String navigationContent = string.Empty.GetRandom();
            String sidebarContent = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var templates = new List<Template>();
            var siteSettings = (null as SiteSettings).Create();

            var container = new ServiceCollection();

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(templates);
            container.AddSingleton<ITemplateRepository>(templateRepo.Object);

            Assert.Throws<TemplateNotFoundException>(() => new TemplateProvider(container.BuildServiceProvider()));
        }

    }
}
