using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using PPTail.Extensions;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using PPTail.Exceptions;

namespace PPTail.Generator.Template.Test
{
    public class TemplateProcessor_ProcessNonContentItemTemplate_Should
    {

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSiteSettingsAreNotProvided()
        {
            string pageTemplateContent = "-----{Title}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string content = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<SiteSettings>();

            var target = (null as ITemplateProcessor).Create(container);
            Assert.Throws<DependencyNotFoundException>(() => target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle));
        }

        [Fact]
        public void ReturnTheProperTypeNameIfTheSiteSettingsAreNotProvided()
        {
            string pageTemplateContent = "-----{Title}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string content = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<SiteSettings>();

            var target = (null as ITemplateProcessor).Create(container);

            string actual = string.Empty;
            try
            {
                target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle);

            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal("SiteSettings", actual);
        }


        [Fact]
        public void ReplaceTheTitlePlaceHolderWithTheTitle()
        {
            string pageTemplateContent = "-----{Title}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string content = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle);

            Assert.Contains(pageTitle, actual);
        }

        [Fact]
        public void ReplaceTheNavigationContentPlaceHolderWithTheNavigationContent()
        {
            string pageTemplateContent = "-----{NavigationMenu}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string content = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle);

            Assert.Contains(navContent, actual);
        }

        [Fact]
        public void ReplaceTheSidebarPlaceHolderWithTheSidebarContent()
        {
            string pageTemplateContent = "-----{Sidebar}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string content = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle);

            Assert.Contains(sidebarContent, actual);
        }

        [Fact]
        public void ReplaceTheContentPlaceHolderWithTheContent()
        {
            string pageTemplateContent = "-----{Content}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string content = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle);

            Assert.Contains(content, actual);
        }

        [Fact]
        public void ReplaceTheSiteTitlePlaceHolderWithTheSiteSettingValue()
        {
            string pageTemplateContent = "-----{SiteTitle}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string content = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle);

            Assert.Contains(siteSettings.Title, actual);
        }

        [Fact]
        public void ReplaceTheSiteDescriptionPlaceHolderWithTheSiteSettingValue()
        {
            string pageTemplateContent = "-----{SiteDescription}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            string content = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var siteSettings = (null as SiteSettings).Create();
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle);

            Assert.Contains(siteSettings.Description, actual);
        }
    }
}
