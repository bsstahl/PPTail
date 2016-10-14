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
    public class TemplateProvider_Ctor_Should
    {
        [Fact]
        public void ThrowDependencyNotFoundExceptionIfServiceProviderNotProvided()
        {
            Assert.Throws(typeof(DependencyNotFoundException), () => new TemplateProvider(null));
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfSiteSettingsAreNotProvided()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var template = (null as Template).Create();
            var templates = new List<Template>() { template };

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(Mock.Of<ISettings>());

            Assert.Throws(typeof(DependencyNotFoundException), () => new TemplateProvider(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfSettingsAreNotProvided()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var template = (null as Template).Create();
            var templates = new List<Template>() { template };

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<SiteSettings>(Mock.Of<SiteSettings>());

            Assert.Throws(typeof(DependencyNotFoundException), () => new TemplateProvider(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowTemplateNotFoundExceptionIfContactPageTemplateNotProvided()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var templates = new List<Template>();
            var siteSettings = (null as SiteSettings).Create();

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<SiteSettings>(siteSettings);

            Assert.Throws(typeof(TemplateNotFoundException), () => new TemplateProvider(container.BuildServiceProvider()));
        }

    }
}
