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
    public class TemplateProvider_GenerateContactPage_Should
    {
        [Fact]
        public void ReturnTheProcessedTemplateWithNavContent()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var target = (null as IContactProvider).Create();
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            Assert.Contains(navigationContent, actual);
        }

        [Fact]
        public void ReturnTheProcessedTemplateWithSidebarContent()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var target = (null as IContactProvider).Create();
            var actual = target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot);

            Assert.Contains(sidebarContent, actual);
        }

        [Fact]
        public void ThrowsDependencyNotFoundExceptionIfSiteSettingsAreNotProvided()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var template = (null as Template).Create();
            var templates = new List<Template>() { template };

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);

            var target = (null as IContactProvider).Create(container.BuildServiceProvider());
            Assert.Throws(typeof(DependencyNotFoundException), () => target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot));
        }

        [Fact]
        public void ThrowsTemplateNotFoundExceptionIfContactPageTemplateNotProvided()
        {
            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            var templates = new List<Template>();
            var siteSettings = (null as SiteSettings).Create();

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<SiteSettings>(siteSettings);

            var target = (null as IContactProvider).Create(container.BuildServiceProvider());
            Assert.Throws(typeof(DependencyNotFoundException), () => target.GenerateContactPage(navigationContent, sidebarContent, pathToRoot));
        }
    }
}
