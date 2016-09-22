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
            return ignore.Create(cpt, ppt);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, string contentPageTemplate, string postPageTemplate)
        {
            return ignore.Create(contentPageTemplate, postPageTemplate, _defaultDateTimeFormatSpecifier);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, string contentPageTemplate, string postPageTemplate, string dateTimeFormatSpecifier)
        {
            var contentTemplate = new Template() { Content = contentPageTemplate, Name = "Main", TemplateType = Enumerations.TemplateType.ContentPage };
            var postTemplate = new Template() { Content = postPageTemplate, Name = "Main", TemplateType = Enumerations.TemplateType.PostPage };
            var templates = new List<Template>() { contentTemplate, postTemplate };

            var settings = (null as Settings).CreateDefault(dateTimeFormatSpecifier);

            return ignore.Create(templates, settings);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, IEnumerable<Template> templates, Settings settings)
        {
            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Template>>(templates);
            container.AddSingleton<Settings>(settings);

            return new PPTail.Generator.T4Html.PageGenerator(container);
        }

        public static Settings CreateDefault(this Settings ignore, string dateTimeFormatSpecifier)
        {
            var settings = new Settings();
            settings.DateTimeFormatSpecifier = dateTimeFormatSpecifier;
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
            var contentTemplate = new Template() { Content = "<html/>", Name = "Main", TemplateType = Enumerations.TemplateType.ContentPage };
            var postTemplate = new Template() { Content = "<html/>", Name = "Main", TemplateType = Enumerations.TemplateType.PostPage };
            var homePageTemplate = new Template() { Content = "<html/>", Name = "main", TemplateType = Enumerations.TemplateType.HomePage };
            var styleTemplate = new Template() { Content = "body { }", Name = "main", TemplateType = Enumerations.TemplateType.Style };
            var bootstrapTemplate = new Template() { Content = "/*! * Bootstrap v0.0.0 */", Name = "Main", TemplateType = Enumerations.TemplateType.Bootstrap };
            var itemTemplate = new Template() { Content = "", Name = "main", TemplateType = Enumerations.TemplateType.Item };
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
    }
}
