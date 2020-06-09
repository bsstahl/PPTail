using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Enumerations;
using Xunit;
using PPTail.Exceptions;
using Moq;

namespace PPTail.Generator.T4Html.Test
{
    public static class Extensions
    {
        const String _contentPageTemplatePath = @"..\..\..\..\ContentPage.template.html";
        const String _postPageTemplatePath = @"..\..\..\..\PostPage.template.html";
        const String _styleTemplatePath = @"..\..\..\..\Style.template.css";
        const String _defaultDateTimeFormatSpecifier = "MM/dd/yy H:mm:ss zzz";

        public static IPageGenerator Create(this IPageGenerator ignore)
        {
            var cpt = System.IO.File.ReadAllText(_contentPageTemplatePath);
            var ppt = System.IO.File.ReadAllText(_postPageTemplatePath);
            var styleTemplate = System.IO.File.ReadAllText(_styleTemplatePath);
            var nav = new FakeNavProvider();
            return ignore.Create(cpt, ppt, styleTemplate, nav);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, String contentPageTemplate, String postPageTemplate, String styleTemplate)
        {
            return ignore.Create(contentPageTemplate, postPageTemplate, styleTemplate, new FakeNavProvider());
        }

        public static IPageGenerator Create(this IPageGenerator ignore, String contentPageTemplate, String postPageTemplate, String stylePageTemplate, INavigationProvider navProvider)
        {
            return ignore.Create(contentPageTemplate, postPageTemplate, stylePageTemplate, _defaultDateTimeFormatSpecifier, navProvider);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, String contentPageTemplate, String postPageTemplate, String stylePageTemplate, String dateTimeFormatSpecifier)
        {
            return ignore.Create(contentPageTemplate, postPageTemplate, stylePageTemplate, dateTimeFormatSpecifier, new FakeNavProvider());
        }

        public static IPageGenerator Create(this IPageGenerator ignore, String contentPageTemplate, String postPageTemplate, String stylePageTemplate, String dateTimeFormatSpecifier, INavigationProvider navProvider)
        {
            var contentTemplate = new Template() { Content = contentPageTemplate, TemplateType = Enumerations.TemplateType.ContentPage };
            var postTemplate = new Template() { Content = postPageTemplate, TemplateType = Enumerations.TemplateType.PostPage };
            var styleTemplate = new Template() { Content = stylePageTemplate, TemplateType = Enumerations.TemplateType.Style };

            var templates = new List<Template>() { contentTemplate, postTemplate, styleTemplate };

            var settings = (null as Settings).CreateDefault(dateTimeFormatSpecifier);

            return ignore.Create(templates, settings, navProvider);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, IEnumerable<Template> templates, ISettings settings)
        {
            return ignore.Create(templates, settings, new FakeNavProvider());
        }

        public static IPageGenerator Create(this IPageGenerator ignore, IEnumerable<Template> templates, ISettings settings, ITemplateProcessor templateProcessor)
        {
            var serviceCollection = (null as IServiceCollection).Create(templates, settings, new FakeNavProvider(), templateProcessor);
            return ignore.Create(serviceCollection);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, TemplateType templateTypeToBeMissing)
        {
            var container = new ServiceCollection();

            var settings = (null as ISettings).CreateDefault();
            container.AddSingleton<ISettings>(settings);

            var nav = new FakeNavProvider();
            container.AddSingleton<INavigationProvider>(nav);

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var testTemplates = templates.Where(t => t.TemplateType != templateTypeToBeMissing);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(testTemplates);
            container.AddSingleton<ITemplateRepository>(templateRepo.Object);

            var linkProvider = Mock.Of<ILinkProvider>();
            container.AddSingleton<ILinkProvider>(linkProvider);

            return ignore.Create(container);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, IEnumerable<Template> templates, ISettings settings, INavigationProvider navProvider)
        {
            var container = (null as IServiceCollection).Create(templates, settings, navProvider);
            return ignore.Create(container);
        }

        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as ISettings).CreateDefault();
            var navProvider = Mock.Of<INavigationProvider>();
            return ignore.Create(templates, settings, navProvider);
        }

        public static IServiceCollection Create(this IServiceCollection ignore, IEnumerable<Template> templates, ISettings settings, INavigationProvider navProvider)
        {
            return ignore.Create(templates, settings, navProvider, Mock.Of<ITemplateProcessor>());
        }

        public static IServiceCollection Create(this IServiceCollection ignore, IEnumerable<Template> templates, ISettings settings, INavigationProvider navProvider, ITemplateProcessor templateProcessor)
        {
            var container = new ServiceCollection();

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(templates);

            container.AddSingleton<ITemplateRepository>(templateRepo.Object);
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<ITagCloudStyler>(c => new Generator.TagCloudStyler.DeviationStyler(c));
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<ILinkProvider>(Mock.Of<ILinkProvider>());
            container.AddSingleton<ITemplateProcessor>(templateProcessor);
            container.AddSingleton<IContentEncoder>(Mock.Of<IContentEncoder>());

            return container;
        }

        public static IPageGenerator Create(this IPageGenerator ignore, IServiceCollection container)
        {
            return new PPTail.Generator.T4Html.PageGenerator(container.BuildServiceProvider());
        }

        public static ISettings CreateDefault(this ISettings ignore)
        {
            return ignore.CreateDefault("yyyy-MM-dd hh:mm", "html");
        }

        public static ISettings CreateDefault(this ISettings ignore, String dateTimeFormatSpecifier)
        {
            return ignore.CreateDefault(dateTimeFormatSpecifier, "html");
        }

        public static ISettings CreateDefault(this ISettings ignore, String dateTimeFormatSpecifier, String outputFileExtension)
        {
            var settings = new Settings();
            settings.DateTimeFormatSpecifier = dateTimeFormatSpecifier;
            settings.OutputFileExtension = outputFileExtension;
            return settings;
        }

        public static ContentItem Create(this ContentItem ignore)
        {
            var tags = new List<string>() { string.Empty.GetRandom() };
            return ignore.Create(tags);
        }

        public static ContentItem Create(this ContentItem ignore, IEnumerable<string> tags)
        {
            var categoryIds = new List<Guid>() { Guid.NewGuid() };
            return ignore.Create(tags, categoryIds);
        }

        public static ContentItem Create(this ContentItem ignore, IEnumerable<Guid> categoryIds)
        {
            var tags = new List<string>() { string.Empty.GetRandom() };
            return ignore.Create(tags, categoryIds);
        }

        public static ContentItem Create(this ContentItem ignore, IEnumerable<string> tags, IEnumerable<Guid> categoryIds)
        {
            String author = string.Empty.GetRandom();
            var lastModDate = DateTime.UtcNow.AddDays(-10.GetRandom(1));
            var pubDate = DateTime.UtcNow.AddDays(-20.GetRandom(10));
            var slug = string.Empty.GetRandom();
            var title = string.Empty.GetRandom();
            return ignore.Create(author, categoryIds, string.Empty.GetRandom(), string.Empty.GetRandom(), true, lastModDate, pubDate, slug, tags, title);
        }

        private static ContentItem Create(this ContentItem ignore,
            String author, IEnumerable<Guid> categoryIds, String content,
            String description, bool isPublished, DateTime lastModDate, DateTime pubDate,
            String slug, IEnumerable<string> tags, String title)
        {
            return new ContentItem()
            {
                Author = author,
                CategoryIds = categoryIds,
                Content = content,
                Description = description,
                IsPublished = isPublished,
                LastModificationDate = lastModDate,
                PublicationDate = pubDate,
                Slug = slug,
                Tags = tags,
                Title = title,
                ByLine = $"by {author}"
            };
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore)
        {
            return ignore.CreateBlankTemplates("<html/>", "<html/>", "<html/>", "body { }", "/*! * Bootstrap v0.0.0 */", "");
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore, String contentpageTemplate, String itemTemplate)
        {
            return ignore.CreateBlankTemplates(contentpageTemplate, "<html/>", "<html/>", "body { }", "/*! * Bootstrap v0.0.0 */", itemTemplate);
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore, String contentpageTemplate, String homePageTemplate, String itemTemplate)
        {
            return ignore.CreateBlankTemplates(contentpageTemplate, "<html/>", homePageTemplate, "body { }", "/*! * Bootstrap v0.0.0 */", itemTemplate);
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore,
            String contentTemplateText, String postTemplateText,
            String homepageTemplateText, String styleTemplateText,
            String bootstrapTemplateText, String itemTemplateText)
        {
            var contentTemplate = new Template() { Content = contentTemplateText, TemplateType = Enumerations.TemplateType.ContentPage };
            var postTemplate = new Template() { Content = postTemplateText, TemplateType = Enumerations.TemplateType.PostPage };
            var homePageTemplate = new Template() { Content = homepageTemplateText, TemplateType = Enumerations.TemplateType.HomePage };
            var styleTemplate = new Template() { Content = styleTemplateText, TemplateType = Enumerations.TemplateType.Style };
            var bootstrapTemplate = new Template() { Content = bootstrapTemplateText, TemplateType = Enumerations.TemplateType.Bootstrap };
            var itemTemplate = new Template() { Content = itemTemplateText, TemplateType = Enumerations.TemplateType.Item };
            return new List<Template>() { contentTemplate, postTemplate, homePageTemplate, styleTemplate, bootstrapTemplate, itemTemplate };
        }

        public static SiteSettings Create(this SiteSettings ignore)
        {
            return ignore.Create("My Test Blog", "A blog of epic scalability", 10.GetRandom(2));
        }

        public static SiteSettings Create(this SiteSettings ignore, String title, String description, Int32 postsPerPage)
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

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, Int32 count)
        {
            var contentItems = new List<ContentItem>();
            for (Int32 i = 0; i < count; i++)
                contentItems.Add((null as ContentItem).Create());
            return contentItems;
        }

        public static Category Create(this Category ignore)
        {
            var id = Guid.NewGuid();
            String name = $"nameof_{id.ToString()}";
            return ignore.Create(id, name);
        }

        public static Category Create(this Category ignore, Guid id, String name)
        {
            String description = $"descriptionof_{id.ToString()}";
            return ignore.Create(id, name, description);
        }

        public static Category Create(this Category ignore, Guid id, String name, String description)
        {
            return new Category()
            {
                Id = id,
                Name = name,
                Description = description
            };
        }

        public static IEnumerable<Category> Create(this IEnumerable<Category> ignore)
        {
            var categoryList = new List<Category>();
            for (Int32 i = 0; i < 10; i++)
                categoryList.Add((null as Category).Create());
            return categoryList;
        }

        public static IEnumerable<Guid> GetRandomCategoryIds(this IEnumerable<Category> categories)
        {
            // Returns 1 or 2 category IDs from the collection of categories
            var result = new List<Guid>();
            Category cat1 = categories.GetRandom();
            Category cat2 = null;

            result.Add(cat1.Id);
            if (true.GetRandom())
            {
                do
                {
                    cat2 = categories.GetRandom();
                } while (cat1.Id == cat2.Id);

                result.Add(cat2.Id);
            }

            return result;
        }

        public static IServiceCollection RemoveDependency<T>(this IServiceCollection container) where T : class
        {
            var item = container.Where(sd => sd.ServiceType == typeof(T)).Single();
            container.Remove(item);
            return container;
        }

        public static IServiceCollection ReplaceDependency<T>(this IServiceCollection container, T serviceInstance) where T : class
        {
            container.RemoveDependency<T>();
            container.AddSingleton<T>(serviceInstance);
            return container;
        }
    }
}
