using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_GenerateContentPage_Should
    {
        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheContentPageTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.ContentPage);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var pageData = (null as ContentItem).Create();
            var target = (null as IPageGenerator).Create(templates, settings);
            Assert.Throws<TemplateNotFoundException>(() => target.GenerateContentPage(string.Empty, string.Empty, pageData));
        }

        [Fact]
        public void ThrowWithTheProperTemplateTypeIfTheContentPageTemplateIsNotSupplied()
        {
            var allTemplates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templates = allTemplates.Where(t => t.TemplateType != Enumerations.TemplateType.ContentPage);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<ISettings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var pageData = (null as ContentItem).Create();

            TemplateType expected = TemplateType.ContentPage;
            var target = (null as IPageGenerator).Create(templates, settings);

            try
            {
                target.GenerateContentPage(string.Empty, string.Empty, pageData);
            }
            catch (TemplateNotFoundException ex)
            {
                Assert.Equal(expected, ex.TemplateType);
            }
        }

        [Fact]
        public void ReplaceATitlePlaceholderWithTheTitle()
        {
            var pageData = (null as ContentItem).Create();

            string template = "*******************************{Title}*******************************";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);
            Assert.Contains(pageData.Title, actual);
        }

        [Fact]
        public void ReplaceAllTitlePlaceholdersWithTheTitle()
        {
            var pageData = (null as ContentItem).Create();

            string template = "{Title}***\r\n************{Title}*********************\t\t****{Title}*****{Title}************{Title}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(pageData.Title));
            Assert.Equal(5, actualCount);
        }

        [Fact]
        public void RemoveThePlaceholderTextIfTheTitleDataValueIsNull()
        {
            const string placeholderText = "{Title}";

            var pageData = (null as ContentItem).Create();
            pageData.Title = null;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(placeholderText));
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void ReplaceAContentPlaceholderWithTheContent()
        {
            const string placeholderText = "{Content}";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.Content;

            string template = $"*******************************{placeholderText}*******************************";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);
            Assert.Contains(expectedData, actual);
        }

        [Fact]
        public void ReplaceAllContentPlaceholdersWithTheContent()
        {
            const string placeholderText = "{Content}";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.Content;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(expectedData));
            Assert.Equal(6, actualCount);
        }

        [Fact]
        public void RemoveThePlaceholderTextIfTheContentDataValueIsNull()
        {
            const string placeholderText = "{Content}";

            var pageData = (null as ContentItem).Create();
            pageData.Content = null;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(placeholderText));
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void ReplaceAllAuthorPlaceholdersWithTheAuthor()
        {
            const string placeholderText = "{Author}";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.Author;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(expectedData));
            Assert.Equal(6, actualCount);
        }

        [Fact]
        public void RemoveThePlaceholderTextIfTheAuthorDataValueIsNull()
        {
            const string placeholderText = "{Author}";

            var pageData = (null as ContentItem).Create();
            pageData.Author = null;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(placeholderText));
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void ReplaceAllDescriptionPlaceholdersWithTheDescription()
        {
            const string placeholderText = "{Description}";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.Description;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(expectedData));
            Assert.Equal(6, actualCount);
        }

        [Fact]
        public void RemoveThePlaceholderTextIfTheDescriptionDataValueIsNull()
        {
            const string placeholderText = "{Description}";

            var pageData = (null as ContentItem).Create();
            pageData.Description = null;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(placeholderText));
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void ReplaceAllPubDatePlaceholdersWithThePubDate()
        {
            const string placeholderText = "{PublicationDate}";
            const string dateTimeFormatSpecifier = "MM/dd/yy H:mm:ss zzz";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.PublicationDate.ToString(dateTimeFormatSpecifier);

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, dateTimeFormatSpecifier);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(expectedData));
            Assert.Equal(6, actualCount);
        }

        [Fact]
        public void ReplaceAllLastModDatePlaceholdersWithTheLastModDate()
        {
            const string placeholderText = "{LastModificationDate}";
            const string dateTimeFormatSpecifier = "MM/dd/yy H:mm:ss zzz";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.LastModificationDate.ToString(dateTimeFormatSpecifier);

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, dateTimeFormatSpecifier);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(expectedData));
            Assert.Equal(6, actualCount);
        }

        [Fact]
        public void ReplaceAllByLinePlaceholdersWithTheByLine()
        {
            const string placeholderText = "{ByLine}";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.ByLine;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(expectedData));
            Assert.Equal(6, actualCount);
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
            
            string template = $"*****{placeholderText}*****";
            var target = (null as IPageGenerator).Create(template, string.Empty, string.Empty);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
            Console.WriteLine(actual);

            foreach (string tag in tagList)
                Assert.Contains(tag, actual);
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

            string template = $"*****{placeholderText}*****";
            var templates = new List<Template>();
            templates.Add(new Template() {
                Content = template,
                TemplateType = TemplateType.ContentPage
            });

            var linkProvider = new Mock<ILinkProvider>();
            foreach (var tag in tagList)
                linkProvider.Setup(l => l.GetUrl("..", "search", tag)).Verifiable();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);

            linkProvider.VerifyAll();
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

            string template = $"*****{placeholderText}*****";
            var templates = new List<Template>();
            templates.Add(new Template()
            {
                Content = template,
                TemplateType = TemplateType.ContentPage
            });

            var linkProvider = new Mock<ILinkProvider>();
            foreach (var tag in tagList)
                linkProvider.Setup(l => l.GetUrl("..", "search", tag)).Returns($"href_{tag}");

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);

            foreach (string tag in tagList)
                Assert.Contains($"href_{tag}".ToLower(), actual.ToLower());
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

            string template = $"*****{placeholderText}*****";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates(template, "<html/>", template);
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");

            var target = (null as IPageGenerator).Create(templates, settings, categoryList);

            var siteSettings = (null as SiteSettings).Create();
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);
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

            string template = $"*****{placeholderText}*****";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates(template, "<html/>", "<div/>");
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");
            var linkProvider = new Mock<ILinkProvider>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var selectedCategories = categoryList.Where(c => categoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
                linkProvider.Setup(l => l.GetUrl("..", "search", category.Name)).Verifiable();

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);

            linkProvider.VerifyAll();
        }

        [Fact]
        public void ReplaceTheCategoriesPlaceholderWithALinkToEachSearchPage()
        {
            const string placeholderText = "{Categories}";

            var categoryList = (null as IEnumerable<Category>).Create();
            var categoryIds = categoryList.GetRandomCategoryIds();
            var pageData = (null as ContentItem).Create(categoryIds);

            string template = $"*****{placeholderText}*****";

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates(template, "<html/>", "<div/>");
            var settings = (null as Settings).CreateDefault("MM/dd/yyyy");
            var linkProvider = new Mock<ILinkProvider>();

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<IEnumerable<Category>>(categoryList);
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);

            var selectedCategories = categoryList.Where(c => categoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
                linkProvider.Setup(l => l.GetUrl("..", "search", category.Name)).Returns(category.Id.ToString());

            var target = (null as IPageGenerator).Create(container);
            var actual = target.GenerateContentPage(string.Empty, string.Empty, pageData);

            foreach (var category in selectedCategories)
                Assert.Contains(category.Id.ToString().ToLower(), actual.ToLower());
        }

    }
}
