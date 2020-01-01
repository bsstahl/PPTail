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
        public void ThrowADependencyNotFoundExceptionIfTheContentRepositoryIsNotProvided()
        {
            String pageTemplateContent = "-----{Title}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IContentRepository>();

            var target = (null as ITemplateProcessor).Create(container);
            Assert.Throws<DependencyNotFoundException>(() => target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot));
        }

        [Fact]
        public void ReturnTheProperTypeNameIfTheContentRepositoryIsNotProvided()
        {
            String pageTemplateContent = "-----{Title}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IContentRepository>();

            var target = (null as ITemplateProcessor).Create(container);

            String actual = string.Empty;
            String expected = nameof(IContentRepository);
            try
            {
                target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot);

            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ReplaceTheTitlePlaceHolderWithTheTitle()
        {
            String pageTemplateContent = "-----{Title}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot);

            Assert.Contains(pageTitle, actual);
        }

        [Fact]
        public void ReplaceTheNavigationContentPlaceHolderWithTheNavigationContent()
        {
            String pageTemplateContent = "-----{NavigationMenu}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot);

            Assert.Contains(navContent, actual);
        }

        [Fact]
        public void ReplaceTheSidebarPlaceHolderWithTheSidebarContent()
        {
            String pageTemplateContent = "-----{Sidebar}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot);

            Assert.Contains(sidebarContent, actual);
        }

        [Fact]
        public void ReplaceTheContentPlaceHolderWithTheContent()
        {
            String pageTemplateContent = "-----{Content}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot);

            Assert.Contains(content, actual);
        }

        [Fact]
        public void ReplaceTheSiteTitlePlaceHolderWithTheSiteSettingValue()
        {
            String pageTemplateContent = "-----{SiteTitle}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);
            var siteSettings = container.BuildServiceProvider().GetService<IContentRepository>().GetSiteSettings();

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot);

            Assert.Contains(siteSettings.Title, actual);
        }

        [Fact]
        public void ReplaceTheSiteDescriptionPlaceHolderWithTheSiteSettingValue()
        {
            String pageTemplateContent = "-----{SiteDescription}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            var siteSettings = container.BuildServiceProvider().GetService<IContentRepository>().GetSiteSettings();

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot);

            Assert.Contains(siteSettings.Description, actual);
        }

        [Fact]
        public void ReplaceThePathToSiteRootPlaceHolderWithTheProperPath()
        {
            String pageTemplateContent = "-----{PathToSiteRoot}-----";
            var pageTemplate = new Entities.Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.ContactPage };
            var templates = new List<Entities.Template>() { pageTemplate };

            String sidebarContent = string.Empty.GetRandom();
            String navContent = string.Empty.GetRandom();
            String pageTitle = string.Empty.GetRandom();
            String content = string.Empty.GetRandom();
            String pathToRoot = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Entities.Template>>(templates);

            var target = (null as ITemplateProcessor).Create(container);
            var actual = target.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, content, pageTitle, pathToRoot);

            Assert.Contains(pathToRoot, actual);
        }

    }
}
