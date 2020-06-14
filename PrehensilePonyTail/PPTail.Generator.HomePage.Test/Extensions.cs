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
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PPTail.Generator.HomePage.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        const String _defaultDateTimeFormatSpecifier = "MM/dd/yy H:mm:ss zzz";

        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var siteSettings = (null as SiteSettings).Create();
            return ignore.Create(siteSettings);
        }

        public static IServiceCollection Create(this IServiceCollection ignore, SiteSettings siteSettings)
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);
            container.AddSingleton<IContentRepository>(contentRepo.Object);

            var templateProcessor = (null as ITemplateProcessor).Create();
            container.AddSingleton<ITemplateProcessor>(templateProcessor);

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(templates);
            container.AddSingleton<ITemplateRepository>(templateRepo.Object);

            return container;
        }

        public static IHomePageGenerator Create(this IHomePageGenerator ignore)
        {
            var homepageTemplateText = "<html/>";
            var itemTemplateText = "<div/>";

            var homepageTemplate = new Template() { Content = homepageTemplateText, TemplateType = TemplateType.HomePage };
            var itemTemplate = new Template() { Content = itemTemplateText, TemplateType = TemplateType.Item };
            var templates = new List<Template>() { homepageTemplate, itemTemplate };

            var contentRepo = Mock.Of<IContentRepository>();
            return ignore.Create(templates);
        }

        public static IHomePageGenerator Create(this IHomePageGenerator ignore, IEnumerable<Template> templates)
        {
            return ignore.Create(templates, new FakeNavProvider(), new List<Category>());
        }

        public static IHomePageGenerator Create(this IHomePageGenerator ignore, IEnumerable<Template> templates, IEnumerable<Category> categories)
        {
            return ignore.Create(templates, new FakeNavProvider(), categories);
        }

        public static IHomePageGenerator Create(this IHomePageGenerator ignore, TemplateType templateTypeToBeMissing)
        {
            var container = (null as IServiceCollection).Create();

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var testTemplates = templates.Where(t => t.TemplateType != templateTypeToBeMissing);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(testTemplates);
            container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);

            return ignore.Create(container);
        }

        public static IHomePageGenerator Create(this IHomePageGenerator ignore, IEnumerable<Template> templates, INavigationProvider navProvider, IEnumerable<Category> categories)
        {
            return ignore.Create(templates, navProvider, categories, Mock.Of<ILinkProvider>());
        }

        public static IHomePageGenerator Create(this IHomePageGenerator ignore, IEnumerable<Template> templates, INavigationProvider navProvider, IEnumerable<Category> categories, ILinkProvider linkProvider)
        {
            var container = new ServiceCollection();

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(templates);
            container.AddSingleton<ITemplateRepository>(templateRepo.Object);

            container.AddSingleton<ITagCloudStyler>(c => new Generator.TagCloudStyler.DeviationStyler(c));
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IEnumerable<Category>>(categories);
            container.AddSingleton<IContentRepository>(Mock.Of<IContentRepository>());
            container.AddSingleton<ILinkProvider>(linkProvider);
            container.AddSingleton<ITemplateProcessor>(Mock.Of<ITemplateProcessor>());
            return ignore.Create(container);
        }

        public static IHomePageGenerator Create(this IHomePageGenerator ignore, IServiceCollection container)
        {
            return new PPTail.Generator.HomePage.HomePageGenerator(container.BuildServiceProvider());
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

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore)
        {
            return ignore.Create(25.GetRandom(2));
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, Int32 count)
        {
            var contentItems = new List<ContentItem>();
            for (Int32 i = 0; i < count; i++)
                contentItems.Add((null as ContentItem).Create());
            return contentItems;
        }

        public static IEnumerable<Template> CreateBlankTemplates(this IEnumerable<Template> ignore)
        {
            return ignore.CreateBlankTemplates("<html/>", "<html/>", "<html/>", "body { }", "/*! * Bootstrap v0.0.0 */", "");
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

        public static ITemplateProcessor Create(this ITemplateProcessor ignore)
        {
            return Mock.Of<ITemplateProcessor>();
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

        public static IServiceCollection ReplaceTemplateRepo(this IServiceCollection container, IEnumerable<Template> templates)
        {
            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates()).Returns(templates);
            return container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);
        }
    }
}
