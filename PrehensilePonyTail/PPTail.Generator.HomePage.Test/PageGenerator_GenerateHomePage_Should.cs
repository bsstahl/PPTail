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
            var posts = (null as IEnumerable<ContentItem>).Create(25);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);

            var target = (null as IHomePageGenerator).Create(container);
            Assert.Throws(typeof(TemplateNotFoundException), () => target.GenerateHomepage(string.Empty, string.Empty, posts));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheItemTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.Item);
            var posts = (null as IEnumerable<ContentItem>).Create(25);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);

            var target = (null as IHomePageGenerator).Create(container);
            Assert.Throws(typeof(TemplateNotFoundException), () => target.GenerateHomepage(string.Empty, string.Empty, posts));
        }

        [Fact]
        public void ReplaceATitlePlaceholderInTheItemTemplateWithTheItemTitle()
        {
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var pageData = posts.Single();

            string pageTemplate = "-----{Content}-----";
            string itemTemplate = "*****{Title}*****";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);

            var target = (null as IHomePageGenerator).Create(container);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);
            Console.WriteLine(actual);

            Assert.Contains(pageData.Title, actual);
        }

        [Fact]
        public void ReplaceAllTitlePlaceholdersWithTheTitle()
        {
            var siteSettings = (null as SiteSettings).Create();
            var posts = (null as IEnumerable<ContentItem>).Create(siteSettings.PostsPerPage);

            string pageTemplate = "-----{Content}-----";
            string itemTemplate = "*{Title}*";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<SiteSettings>(siteSettings);

            var target = (null as IHomePageGenerator).Create(container);
            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);

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

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);

            var target = (null as IHomePageGenerator).Create(container);
            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);

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
            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);
            Console.WriteLine(actual);

            foreach (string tag in tagList)
                Assert.Contains(tag, actual);
        }

        [Fact]
        public void ReplaceTheTagPlaceholderWithTheOutputOfTheLinkProvider()
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

            var container = (null as IServiceCollection).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");
            var linkProvider = new Mock<ILinkProvider>();

            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            foreach (string tag in tagList)
                linkProvider.Setup(l => l.GetUrl(".", "search", tag))
                    .Returns($"href_{tag}");

            var target = (null as IHomePageGenerator).Create(container);
            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);

            foreach (string tag in tagList)
                Assert.Contains($"href_{tag}", actual);
        }

        [Fact]
        public void CallTheLinkProviderOnceForEachTag()
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

            var container = (null as IServiceCollection).Create();
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");
            var linkProvider = new Mock<ILinkProvider>();

            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            foreach (string tag in tagList)
                linkProvider.Setup(l => l.GetUrl(".", "search", tag))
                    .Verifiable();

            var target = (null as IHomePageGenerator).Create(container);

            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);

            linkProvider.VerifyAll();
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
            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);
            Console.WriteLine(actual);

            var selectedCategories = categoryList.Where(c => categoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
                Assert.Contains(category.Name, actual);
        }

        [Fact]
        public void CallTheLinkProviderOnceForEachCategory()
        {
            const string placeholderText = "{Categories}";

            var categoryList = (null as IEnumerable<Category>).Create();
            var categoryIds = categoryList.GetRandomCategoryIds();
            var pageData = (null as ContentItem).Create(categoryIds);
            var posts = new List<ContentItem>() { pageData };

            string itemTemplate = $"*****{placeholderText}*****";
            string pageTemplate = "-----{Content}-----";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as ISettings).CreateDefault();
            var linkProvider = new Mock<ILinkProvider>();

            var selectedCategories = categoryList.Where(c => categoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
                linkProvider.Setup(l => l.GetUrl(".", "search", category.Name)).Verifiable();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);

            var target = (null as IHomePageGenerator).Create(container);

            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);

            linkProvider.VerifyAll();
        }

        [Fact]
        public void ReplaceTheCategoriesPlaceholderWithTheOutputOfTheLinkProvider()
        {
            const string placeholderText = "{Categories}";

            var categoryList = (null as IEnumerable<Category>).Create();
            var categoryIds = categoryList.GetRandomCategoryIds();
            var pageData = (null as ContentItem).Create(categoryIds);
            var posts = new List<ContentItem>() { pageData };

            string itemTemplate = $"*****{placeholderText}*****";
            string pageTemplate = "-----{Content}-----";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates("<html/>", pageTemplate, itemTemplate);
            var settings = (null as ISettings).CreateDefault();
            var linkProvider = new Mock<ILinkProvider>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as IHomePageGenerator).Create(container);

            var selectedCategories = categoryList.Where(c => categoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
                linkProvider.Setup(l => l.GetUrl(".", "search", category.Name)).Returns(category.Id.ToString());

            var actual = target.GenerateHomepage(string.Empty, string.Empty, posts);

            foreach (var category in selectedCategories)
                Assert.Contains(category.Id.ToString().ToLower(), actual.ToLower());
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheHomePageTemplateIsNotProvided()
        {
            var target = (null as IHomePageGenerator).Create(Enumerations.TemplateType.HomePage);
            Assert.Throws<TemplateNotFoundException>(() => target.GenerateHomepage(string.Empty, string.Empty, new List<ContentItem>()));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheItemPageTemplateIsNotProvided()
        {
            var target = (null as IHomePageGenerator).Create(Enumerations.TemplateType.Item);
            Assert.Throws<TemplateNotFoundException>(() => target.GenerateHomepage(string.Empty, string.Empty, new List<ContentItem>()));
        }

    }
}
