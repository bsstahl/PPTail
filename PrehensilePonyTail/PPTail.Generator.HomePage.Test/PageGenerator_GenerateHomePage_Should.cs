using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.HomePage.Test
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
            container.AddSingleton<ISettings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var pageData = (null as ContentItem).Create();
            var target = (null as IHomePageGenerator).Create(templates, settings);
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
            container.AddSingleton<ISettings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var pageData = (null as ContentItem).Create();
            var target = (null as IHomePageGenerator).Create(templates, settings);
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

            var target = (null as IHomePageGenerator).Create(templates, settings);

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
            string itemTemplate = "*{Title}*";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IHomePageGenerator).Create(templates, settings);

            var siteSettings = (null as SiteSettings).Create(string.Empty.GetRandom(), string.Empty.GetRandom(), posts.Count());
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

            var target = (null as IHomePageGenerator).Create(templates, settings);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts);

            Console.WriteLine(actual);
            Assert.DoesNotContain(itemTemplate, actual);
        }

        [Fact]
        public void ReplaceTheTagPlaceholderWithEachTag()
        {
            const string placeholderText = "{Tags}";

            int tagCount = 8.GetRandom(3);
            var tagList = new List<string>();
            for (int i = 0; i < tagCount; i++)
                tagList.Add(string.Empty.GetRandom());

            var pageData = (null as ContentItem).Create(tagList);
            var posts = new List<ContentItem>() { pageData };

            string itemTemplate = $"*****{placeholderText}*****";
            string pageTemplate = "-----{Content}-----";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IHomePageGenerator).Create(templates, settings);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts);
            Console.WriteLine(actual);

            foreach (string tag in tagList)
                Assert.Contains(tag, actual);
        }

        [Fact]
        public void ReplaceTheTagPlaceholderWithALinkToEachTagPage()
        {
            const string placeholderText = "{Tags}";

            int tagCount = 8.GetRandom(3);
            var tagList = new List<string>();
            for (int i = 0; i < tagCount; i++)
                tagList.Add(string.Empty.GetRandom());

            var pageData = (null as ContentItem).Create(tagList);
            var posts = new List<ContentItem>() { pageData };

            string itemTemplate = $"*****{placeholderText}*****";
            string pageTemplate = "-----{Content}-----";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IHomePageGenerator).Create(templates, settings);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts);
            Console.WriteLine(actual);

            foreach (string tag in tagList)
            {
                string href = $"\\search\\{tag}.{settings.OutputFileExtension}";
                Assert.Contains(href.ToLower(), actual.ToLower());
            }
        }

        [Fact]
        public void ReplaceTheCategoriesPlaceholderWithEachCategory()
        {
            const string placeholderText = "{Categories}";

            var categoryList = new List<Category>();
            for (int i = 0; i < 10; i++)
                categoryList.Add((null as Category).Create());

            var categoryIds = categoryList.GetRandomCategoryIds();
            var pageData = (null as ContentItem).Create(categoryIds);
            var posts = new List<ContentItem>() { pageData };

            string itemTemplate = $"*****{placeholderText}*****";
            string pageTemplate = "-----{Content}-----";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IHomePageGenerator).Create(templates, settings, categoryList);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts);
            Console.WriteLine(actual);

            var selectedCategories = categoryList.Where(c => categoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
                Assert.Contains(category.Name, actual);
        }

        [Fact]
        public void ReplaceTheCategoriesPlaceholderWithALinkToEachSearchPage()
        {
            const string placeholderText = "{Categories}";

            var categoryList = new List<Category>();
            for (int i = 0; i < 10; i++)
                categoryList.Add((null as Category).Create());

            var categoryIds = categoryList.GetRandomCategoryIds();
            var pageData = (null as ContentItem).Create(categoryIds);
            var posts = new List<ContentItem>() { pageData };

            string itemTemplate = $"*****{placeholderText}*****";
            string pageTemplate = "-----{Content}-----";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IHomePageGenerator).Create(templates, settings, categoryList);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, siteSettings, posts);
            Console.WriteLine(actual);

            var selectedCategories = categoryList.Where(c => categoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
            {
                string href = $"\\search\\{category.Name}.{settings.OutputFileExtension}";
                Assert.Contains(href.ToLower(), actual.ToLower());
            }
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheHomePageTemplateIsNotProvided()
        {
            var target = (null as IHomePageGenerator).Create(Enumerations.TemplateType.HomePage);
            Assert.Throws<TemplateNotFoundException>(() => target.GenerateHomepage(string.Empty, string.Empty, Mock.Of<SiteSettings>(), new List<ContentItem>()));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheItemPageTemplateIsNotProvided()
        {
            var target = (null as IHomePageGenerator).Create(Enumerations.TemplateType.Item);
            Assert.Throws<TemplateNotFoundException>(() => target.GenerateHomepage(string.Empty, string.Empty, Mock.Of<SiteSettings>(), new List<ContentItem>()));
        }

    }
}
