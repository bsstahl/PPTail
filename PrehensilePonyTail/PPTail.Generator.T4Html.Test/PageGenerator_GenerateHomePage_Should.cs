using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_GenerateHomePage_Should
    {
        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheContentPageTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.HomePage);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");
            var posts = (null as IEnumerable<ContentItem>).Create(25);

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<Settings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var pageData = (null as ContentItem).Create();
            var target = (null as IPageGenerator).Create(templates, settings);
            Assert.Throws(typeof(TemplateNotFoundException), () => target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheItemTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.Item);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");
            var posts = (null as IEnumerable<ContentItem>).Create(25);

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<Settings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var pageData = (null as ContentItem).Create();
            var target = (null as IPageGenerator).Create(templates, settings);
            Assert.Throws(typeof(TemplateNotFoundException), () => target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts));
        }

        [Fact]
        public void ReplaceATitlePlaceholderInTheItemTemplateWithTheItemTitle()
        {
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var pageData = posts.Single();

            string pageTemplate = "-----{Content}-----";
            string itemTemplate = "*****{Title}*****";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IPageGenerator).Create(templates, settings);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts);
            Console.WriteLine(actual);

            Assert.Contains(pageData.Title, actual);
        }

        [Fact]
        public void ReplaceAllTitlePlaceholdersWithTheTitle()
        {
            var posts = (null as IEnumerable<ContentItem>).Create(5);

            string pageTemplate = "-----{Content}-----";
            string itemTemplate = "{Title}";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IPageGenerator).Create(templates, settings);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts);

            Console.WriteLine(actual);
            foreach (var pageData in posts)
                Assert.Contains(pageData.Title, actual);
        }

        [Fact]
        public void RemoveThePlaceholderTextIfTheTitleDataValueIsNull()
        {
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var pageData = posts.Single();

            string pageTemplate = "-----{Content}-----";
            string itemTemplate = "{Title}";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IPageGenerator).Create(templates, settings);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts);

            Console.WriteLine(actual);
            Assert.DoesNotContain(itemTemplate, actual);
        }

    }
}
