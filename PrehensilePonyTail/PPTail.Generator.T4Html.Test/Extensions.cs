using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.T4Html.Test
{
    public static class Extensions
    {
        const string _contentPageTemplatePath = @"..\ContentPage.template.html";
        const string _postPageTemplatePath = @"..\PostPage.template.html";
        const string _defaultDateTimeFormatSpecifier = "MM/dd/yy H:mm:ss zzz";

        public static IPageGenerator Create(this IPageGenerator ignore)
        {
            var cpt = System.IO.File.ReadAllText(_contentPageTemplatePath);
            var ppt = System.IO.File.ReadAllText(_postPageTemplatePath);
            var nav = new FakeNavProvider();
            return ignore.Create(cpt, ppt, nav);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, string contentPageTemplate, string postPageTemplate)
        {
            return ignore.Create(contentPageTemplate, postPageTemplate, new FakeNavProvider());
        }

        public static IPageGenerator Create(this IPageGenerator ignore, string contentPageTemplate, string postPageTemplate, INavigationProvider navProvider)
        {
            return ignore.Create(contentPageTemplate, postPageTemplate, _defaultDateTimeFormatSpecifier, navProvider);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, string contentPageTemplate, string postPageTemplate, string dateTimeFormatSpecifier)
        {
            return ignore.Create(contentPageTemplate, postPageTemplate, dateTimeFormatSpecifier, new FakeNavProvider());
        }

        public static IPageGenerator Create(this IPageGenerator ignore, string contentPageTemplate, string postPageTemplate, string dateTimeFormatSpecifier, INavigationProvider navProvider)
        {
            var contentTemplate = new Template() { Content = contentPageTemplate, Name = "Main", TemplateType = Enumerations.TemplateType.ContentPage };
            var postTemplate = new Template() { Content = postPageTemplate, Name = "Main", TemplateType = Enumerations.TemplateType.PostPage };
            var templates = new List<Template>() { contentTemplate, postTemplate };

            var settings = (null as Settings).CreateDefault(dateTimeFormatSpecifier);

            return ignore.Create(templates, settings, navProvider);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, IEnumerable<Template> templates, Settings settings)
        {
            return ignore.Create(templates, settings, new FakeNavProvider());
        }

        public static IPageGenerator Create(this IPageGenerator ignore, IEnumerable<Template> templates, Settings settings, INavigationProvider navProvider)
        {
            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<ITagCloudStyler>(c => new Generator.TagCloudStyler.DeviationStyler(c));
            container.AddSingleton<INavigationProvider>(navProvider);
            return new PPTail.Generator.T4Html.PageGenerator(container.BuildServiceProvider());
        }

        public static Settings CreateDefault(this Settings ignore, string dateTimeFormatSpecifier)
        {
            return ignore.CreateDefault(dateTimeFormatSpecifier, "html");
        }

        public static Settings CreateDefault(this Settings ignore, string dateTimeFormatSpecifier, string outputFileExtension)
        {
            var settings = new Settings();
            settings.DateTimeFormatSpecifier = dateTimeFormatSpecifier;
            settings.OutputFileExtension = outputFileExtension;
            return settings;
        }

        public static ContentItem Create(this ContentItem ignore)
        {
            string author = string.Empty.GetRandom();
            return new ContentItem()
            {
                Author = author,
                CategoryIds = new List<Guid>() { Guid.NewGuid() },
                Content = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                IsPublished = true,
                LastModificationDate = DateTime.UtcNow.AddDays(-10.GetRandom()),
                PublicationDate = DateTime.UtcNow.AddDays(-20.GetRandom(10)),
                Slug = string.Empty.GetRandom(),
                Tags = new List<string>() { string.Empty.GetRandom() },
                Title = string.Empty.GetRandom(),
                ByLine = $"by {author}"
            };
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore)
        {
            return ignore.CreateBlankTemplates("<html/>", "<html/>", "<html/>", "body { }", "/*! * Bootstrap v0.0.0 */", "");
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore, string contentpageTemplate, string itemTemplate)
        {
            return ignore.CreateBlankTemplates(contentpageTemplate, "<html/>", "<html/>", "body { }", "/*! * Bootstrap v0.0.0 */", itemTemplate);
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore, string contentpageTemplate, string homePageTemplate, string itemTemplate)
        {
            return ignore.CreateBlankTemplates(contentpageTemplate, "<html/>", homePageTemplate, "body { }", "/*! * Bootstrap v0.0.0 */", itemTemplate);
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore, 
            string contentTemplateText, string postTemplateText, 
            string homepageTemplateText, string styleTemplateText,
            string bootstrapTemplateText, string itemTemplateText)
        {
            var contentTemplate = new Template() { Content = contentTemplateText, Name = "Main", TemplateType = Enumerations.TemplateType.ContentPage };
            var postTemplate = new Template() { Content = postTemplateText, Name = "Main", TemplateType = Enumerations.TemplateType.PostPage };
            var homePageTemplate = new Template() { Content = homepageTemplateText, Name = "main", TemplateType = Enumerations.TemplateType.HomePage };
            var styleTemplate = new Template() { Content = styleTemplateText, Name = "main", TemplateType = Enumerations.TemplateType.Style };
            var bootstrapTemplate = new Template() { Content = bootstrapTemplateText, Name = "Main", TemplateType = Enumerations.TemplateType.Bootstrap };
            var itemTemplate = new Template() { Content = itemTemplateText, Name = "main", TemplateType = Enumerations.TemplateType.Item };
            return new List<Template>() { contentTemplate, postTemplate, homePageTemplate, styleTemplate, bootstrapTemplate, itemTemplate };
        }

        public static SiteSettings Create(this SiteSettings ignore)
        {
            return ignore.Create("My Test Blog", "A blog of epic scalability", 10.GetRandom(2));
        }

        public static SiteSettings Create(this SiteSettings ignore, string title, string description, int postsPerPage)
        {
            return new SiteSettings()
            {
                Title = title,
                Description = description,
                PostsPerPage = postsPerPage
            };
        }

        public static Widget CreateWidget(this Enumerations.WidgetType widgetType)
        {
            return new Widget()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty.GetRandom(),
                ShowTitle = true.GetRandom(),
                WidgetType = widgetType,
                Dictionary = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("content", string.Empty.GetRandom())
                    }
            };
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, int count)
        {
            var contentItems = new List<ContentItem>();
            for (int i = 0; i < count; i++)
                contentItems.Add((null as ContentItem).Create());
            return contentItems;
        }


    }
}
