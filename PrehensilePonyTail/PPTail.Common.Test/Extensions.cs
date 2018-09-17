using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Common.Test
{
    public static class Extensions
    {
        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var container = new ServiceCollection();

            var siteSettings = (null as SiteSettings).Create();
            var contentRepo = (null as IContentRepository).Create(siteSettings);

            var settings = (null as ISettings).Create(contentRepo);
            var categories = (null as IEnumerable<Category>).Create();
            var linkProvider = Mock.Of<ILinkProvider>();

            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IEnumerable<Category>>(categories);
            container.AddSingleton<ILinkProvider>(linkProvider);

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
                SourceConnection = contentRepo.GetSourceConnection()
            };
        }

        public static IContentRepository Create(this IContentRepository ignore)
        {
            var siteSettings = (null as SiteSettings).Create();
            return ignore.Create(siteSettings);
        }

        public static IContentRepository Create(this IContentRepository ignore, SiteSettings siteSettings)
        {
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);
            return contentRepo.Object;
        }

        public static SiteSettings Create(this SiteSettings ignore)
        {
            return new SiteSettings()
            {
                Title = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                PostsPerPage = 25.GetRandom(5)
            };
        }

        public static ContentItem Create(this ContentItem ignore, DateTime pubDate)
        {
            return new ContentItem()
            {
                Author = string.Empty.GetRandom(),
                CategoryIds = new List<Guid>() { Guid.NewGuid() },
                Content = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                IsPublished = true,
                LastModificationDate = pubDate.AddMinutes(10.GetRandom()),
                PublicationDate = pubDate,
                Slug = string.Empty.GetRandom(),
                Tags = (null as IEnumerable<string>).CreateTags(),
                Title = string.Empty.GetRandom()
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
            var result = new List<Category>();

            int count = 10.GetRandom(5);
            for (int i = 0; i < count; i++)
                result.Add((null as Category).Create());

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

        public static string GetSourceConnection(this IContentRepository contentRepo)
        {
            string filePath = $"c:\\{string.Empty.GetRandom()}\\{string.Empty.GetRandom()}";
            return contentRepo.GetSourceConnection(filePath);
        }

        public static string GetSourceConnection(this IContentRepository contentRepo, string filePath)
        {
            return $"Provider={contentRepo.GetType().Name};FilePath={filePath}";
        }

    }
}
