using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;
using Moq;
using PPTail.Entities;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Enumerations;

namespace PPTail.SiteGenerator.Test
{
    public static class Extensions
    {

        public static string ToBase64(this string data)
        {
            byte[] byteContent = data.Select(d => Convert.ToByte(d)).ToArray();
            return Convert.ToBase64String(byteContent);
        }

        public static string FromBase64(this string data)
        {
            byte[] byteContent = Convert.FromBase64String(data);
            return System.Text.Encoding.UTF8.GetString(byteContent);
        }

        public static Builder Create(this Builder ignore)
        {
            return ignore.Create((null as IServiceCollection).Create());
        }

        public static Builder Create(this Builder ignore, IServiceCollection container)
        {
            return new Builder(container.BuildServiceProvider());
        }

        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            return ignore.Create(Mock.Of<IContentRepository>(), Mock.Of<IArchiveProvider>(), Mock.Of<IContactProvider>(),
                Mock.Of<ISearchProvider>(), Mock.Of<IPageGenerator>(), Mock.Of<IHomePageGenerator>(), Mock.Of<INavigationProvider>(), Mock.Of<IRedirectProvider>(), Mock.Of<ISyndicationProvider>(),
                Mock.Of<ISettings>(), Mock.Of<SiteSettings>(), new List<Category>());
        }

        public static IServiceCollection Create(this IServiceCollection ignore, IContentRepository contentRepo, IArchiveProvider archiveProvider, IContactProvider contactProvider, ISearchProvider searchProvider, IPageGenerator pageGen, IHomePageGenerator homePageGen, INavigationProvider navProvider, IRedirectProvider redirectProvider, ISyndicationProvider syndicationProvider, ISettings settings, SiteSettings siteSettings, IEnumerable<Category> categories)
        {
            IServiceCollection container = new ServiceCollection();
            container.AddSingleton<IContentRepository>(contentRepo);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<IHomePageGenerator>(homePageGen);
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<SiteSettings>(siteSettings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);
            container.AddSingleton<ISearchProvider>(searchProvider);
            container.AddSingleton<ISyndicationProvider>(syndicationProvider);
            container.AddSingleton<IEnumerable<Category>>(categories);
            container.AddSingleton<IRedirectProvider>(redirectProvider);
            return container;
        }

        public static IServiceCollection RemoveDependency<T>(this IServiceCollection container) where T : class
        {
            var item = container.Where(sd => sd.ServiceType == typeof(T)).Single();
            container.Remove(item);
            return container;
        }

        public static IServiceCollection ReplaceDependency<T>(this IServiceCollection container, T serviceInstance) where T : class
        {
            var item = container.Where(sd => sd.ServiceType == typeof(T)).Single();
            container.Remove(item);
            container.AddSingleton<T>(serviceInstance);
            return container;
        }

        public static ContentItem Create(this ContentItem ignore)
        {
            var categoryId = Guid.NewGuid();
            return ignore.Create(categoryId);
        }

        public static ContentItem Create(this ContentItem ignore, Guid categoryId)
        {
            string tag = string.Empty.GetRandom();
            return ignore.Create(categoryId, new List<string>() { tag });
        }

        public static ContentItem Create(this ContentItem ignore, string tag)
        {
            var categoryId = Guid.NewGuid();
            return ignore.Create(categoryId, new List<string>() { tag });
        }

        public static ContentItem Create(this ContentItem ignore, Guid categoryId, IEnumerable<string> tags)
        {
            return new ContentItem()
            {
                Id = Guid.NewGuid(),
                Author = string.Empty.GetRandom(),
                CategoryIds = new List<Guid>() { categoryId },
                Content = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                IsPublished = true,
                LastModificationDate = DateTime.UtcNow.AddDays(-10.GetRandom()),
                PublicationDate = DateTime.UtcNow.AddDays(-20.GetRandom(10)),
                Slug = string.Empty.GetRandom(),
                Tags = tags,
                Title = string.Empty.GetRandom()
            };
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore)
        {
            return ignore.Create(25.GetRandom(5));
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, int count)
        {
            var allCategories = (null as IEnumerable<Category>).Create();
            return ignore.Create(allCategories, count);
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, IEnumerable<Category> allCategories, int count)
        {
            var contentItems = new List<ContentItem>();
            for (int i = 0; i < count; i++)
                contentItems.Add((null as ContentItem).Create(allCategories.GetRandom().Id));
            return contentItems;
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

        public static ISettings Create(this ISettings ignore)
        {
            return ignore.Create(string.Empty.GetRandom(3));
        }

        public static ISettings Create(this ISettings ignore, string outputFileExtension)
        {
            return ignore.Create("yyyyMMdd", "yyyyMMdd hhmm", outputFileExtension, $"*********{string.Empty.GetRandom()}*********", null);
        }

        public static ISettings Create(this ISettings ignore, string dateFormatSpecifier, string dateTimeFormatSpecifier, string outputFileExtension, string itemSeparator, IEnumerable<Tuple<string, string>> extendedSettings)
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

        public static IEnumerable<SourceFile> Create(this IEnumerable<SourceFile> ignore)
        {
            return ignore.Create(10.GetRandom(3));
        }

        public static IEnumerable<SourceFile> Create(this IEnumerable<SourceFile> ignore, int count)
        {
            var result = new List<SourceFile>();

            for (int i = 0; i < count; i++)
                result.Add((null as SourceFile).Create());

            return result;
        }

        public static SourceFile Create(this SourceFile ignore)
        {
            return ignore.Create(
                string.Empty.GetRandom().Select(s => Convert.ToByte(s)).ToArray(),
                string.Empty.GetRandom(),
                string.Empty.GetRandom());
        }

        public static SourceFile Create(this SourceFile ignore, byte[] content, string relativePath, string fileName)
        {
            return new SourceFile()
            {
                Contents = content,
                RelativePath = relativePath,
                FileName = fileName
            };
        }

        public static IEnumerable<string> CreateTags(this IEnumerable<string> ignore)
        {
            var result = new List<string>();
            int count = 10.GetRandom(1);
            for (int i = 0; i < count; i++)
                result.Add(string.Empty.GetRandom());
            return result;
        }

        public static IEnumerable<Category> Create(this IEnumerable<Category> ignore)
        {
            return ignore.Create(8.GetRandom(3));
        }

        public static IEnumerable<Category> Create(this IEnumerable<Category> ignore, int count)
        {
            var result = new List<Category>();
            for (int i = 0; i < count; i++)
                result.Add((null as Category).Create());
            return result;
        }

        public static Category Create(this Category ignore)
        {
            var id = Guid.NewGuid();
            var name = $"nameof_{id.ToString()}";
            var description = $"descriptionof_{id.ToString()}";
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


    }
}
