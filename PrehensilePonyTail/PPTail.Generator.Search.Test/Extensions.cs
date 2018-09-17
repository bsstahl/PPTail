using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Generator.Search.Test
{
    public static class Extensions
    {
        public static ISearchProvider Create(this ISearchProvider ignore)
        {
            return ignore.Create(Mock.Of<IEnumerable<Template>>(),
                Mock.Of<Settings>(), null, Mock.Of<ILinkProvider>());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IEnumerable<Template> templates)
        {
            return ignore.Create(templates, Mock.Of<Settings>(), null, Mock.Of<ILinkProvider>());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IEnumerable<Template> templates, IEnumerable<Category> categories)
        {
            return ignore.Create(templates, Mock.Of<Settings>(), categories, Mock.Of<ILinkProvider>());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IEnumerable<Template> templates, ISettings settings, SiteSettings siteSettings)
        {
            return ignore.Create(templates, settings, null, Mock.Of<ILinkProvider>());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IEnumerable<Template> templates, ISettings settings, IEnumerable<Category> categories, ILinkProvider linkProvider)
        {
            var container = (null as IServiceCollection).Create(templates, settings, categories, linkProvider, Mock.Of<ITemplateProcessor>());
            return new PageGenerator(container.BuildServiceProvider());
        }

        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            return ignore.Create((null as IEnumerable<Template>).Create(),
                null, (null as IEnumerable<Category>).Create(),
                Mock.Of<ILinkProvider>(), Mock.Of<ITemplateProcessor>());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IServiceCollection container)
        {
            return ignore.Create(container.BuildServiceProvider());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IServiceProvider serviceProvider)
        {
            return new PageGenerator(serviceProvider);
        }

        public static IServiceCollection Create(this IServiceCollection ignore, IEnumerable<Template> templates, ISettings settings, IEnumerable<Category> categories, ILinkProvider linkProvider, ITemplateProcessor templateProcessor)
        {
            var container = new ServiceCollection();

            var siteSettings = new SiteSettings() { PostsPerPage = 10.GetRandom(3), PostsPerFeed = 10.GetRandom(3), Title = string.Empty.GetRandom(), Description = string.Empty.GetRandom() };
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);
            container.AddSingleton<IContentRepository>(contentRepo.Object);

            if (settings is null)
                container.AddSingleton<ISettings>((null as ISettings).Create(contentRepo.Object));
            else
                container.AddSingleton<ISettings>(settings);

            if (templates != null)
                container.AddSingleton<IEnumerable<Template>>(templates);

            if (categories != null)
                container.AddSingleton<IEnumerable<Category>>(categories);

            if (linkProvider != null)
                container.AddSingleton<ILinkProvider>(linkProvider);

            if (templateProcessor != null)
                container.AddSingleton<ITemplateProcessor>(templateProcessor);

            return container;
        }

        public static ISettings Create(this ISettings ignore, IContentRepository contentRepo)
        {
            return new Settings()
            {
                DateFormatSpecifier = "MM/dd/yyyy",
                DateTimeFormatSpecifier = "MM/dd/yyyy hh:mm",
                ItemSeparator = string.Empty.GetRandom(),
                OutputFileExtension = string.Empty.GetRandom(),
                SourceConnection = $"Provider={contentRepo.GetType().FullName};FilePath=c:\\"
            };
        }

        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore)
        {
            var itemTemplate = (null as Template).Create("{Title}-{ByLine}-{PublicationDate}-{Content}", "Item", TemplateType.Item);
            var searchTemplate = (null as Template).Create("***{Title}***{Content}***{Sidebar}***{NavigationMenu}***", "Search", TemplateType.SearchPage);
            return ignore.Create(itemTemplate, searchTemplate);
        }

        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore, Template itemTemplate, Template searchTemplate)
        {
            var result = new List<Template>();

            if (itemTemplate != null)
                result.Add(itemTemplate);

            if (searchTemplate != null)
                result.Add(searchTemplate);

            return result;
        }

        public static Template Create(this Template ignore, TemplateType templateType)
        {
            return ignore.Create(string.Empty.GetRandom(), string.Empty.GetRandom(), templateType);
        }

        public static Template Create(this Template ignore, string content, string name, TemplateType templateType)
        {
            return new Template()
            {
                Content = content,
                TemplateType = templateType
            };
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, ContentItem item)
        {
            return new List<ContentItem>() { item };
        }

        public static ContentItem Create(this ContentItem ignore, string tag)
        {
            var tags = new List<string>() { tag };
            return ignore.Create(Guid.NewGuid(), tags);
        }

        public static ContentItem Create(this ContentItem ignore, Guid categoryId)
        {
            var tags = new List<string>() { string.Empty.GetRandom() };
            return ignore.Create(categoryId, tags);
        }

        public static ContentItem Create(this ContentItem ignore, Guid categoryId, IEnumerable<string> tags)
        {
            string author = string.Empty.GetRandom();
            return new ContentItem()
            {
                Author = author,
                CategoryIds = new List<Guid>() { categoryId },
                Content = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                IsPublished = true,
                LastModificationDate = DateTime.UtcNow.AddDays(-10.GetRandom()),
                PublicationDate = DateTime.UtcNow.AddDays(-20.GetRandom(10)),
                Slug = string.Empty.GetRandom(),
                Tags = tags,
                Title = string.Empty.GetRandom(),
                ByLine = $"by {author}"
            };
        }

        public static Settings Create(this Settings ignore)
        {
            return ignore.Create(string.Empty.GetRandom(3));
        }

        public static Settings Create(this Settings ignore, string outputFileExtension)
        {
            return ignore.Create("yyyyMMdd", "yyyyMMdd hhmm", outputFileExtension, $"*********{string.Empty.GetRandom()}*********", null);
        }

        public static Settings Create(this Settings ignore, string dateFormatSpecifier, string dateTimeFormatSpecifier, string outputFileExtension, string itemSeparator, IEnumerable<Tuple<string, string>> extendedSettings)
        {
            var result = new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                OutputFileExtension = outputFileExtension,
                ItemSeparator = itemSeparator
            };

            if (extendedSettings != null && extendedSettings.Any())
                result.ExtendedSettings.AddRange(extendedSettings);

            return result;
        }

        public static Category Create(this Category ignore)
        {
            Guid id = Guid.NewGuid();
            string name = $"nameof_{id.ToString()}";
            string description = $"descriptionof_{id.ToString()}";
            return ignore.Create(id, name, description);
        }

        public static Category Create(this Category ignore, Guid id, string name, string description)
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
            return ignore.Create(6.GetRandom(3));
        }

        public static IEnumerable<Category> Create(this IEnumerable<Category> ignore, int count)
        {
            var result = new List<Category>();
            for (int i = 0; i < count; i++)
                result.Add((null as Category).Create());
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
