using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;
using Moq;
using PPTail.Entities;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.SiteGenerator.Test
{
    public static class Extensions
    {
        public static Builder Create(this Builder ignore, IContentRepository contentRepo)
        {
            return ignore.Create(contentRepo, string.Empty.GetRandom());
        }

        public static Builder Create(this Builder ignore, IContentRepository contentRepo, string pageFilenameExtension)
        {
            IServiceCollection container = (null as IServiceCollection).Create();
            var pageGen = Mock.Of<IPageGenerator>();

            var settings = new Settings();
            settings.outputFileExtension = pageFilenameExtension;

            container.AddSingleton<IContentRepository>(contentRepo);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<Settings>(settings);

            return ignore.Create(container);
        }

        public static Builder Create(this Builder ignore, IServiceCollection container)
        {
            return new Builder(container.BuildServiceProvider());
        }

        public static ContentItem Create(this ContentItem ignore)
        {
            return new ContentItem()
            {
                Author = string.Empty.GetRandom(),
                CategoryIds = new List<Guid>() { Guid.NewGuid() },
                Content = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                IsPublished = true,
                LastModificationDate = DateTime.UtcNow.AddDays(-10.GetRandom()),
                PublicationDate = DateTime.UtcNow.AddDays(-20.GetRandom(10)),
                Slug = string.Empty.GetRandom(),
                Tags = new List<string>() { string.Empty.GetRandom() },
                Title = string.Empty.GetRandom()
            };
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, int count)
        {
            var contentItems = new List<ContentItem>();
            for (int i = 0; i < count; i++)
                contentItems.Add((null as ContentItem).Create());
            return contentItems;
        }

        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var serviceCollection = new ServiceCollection();
            
            // TODO: create contentRepo, pageGen, pageFilenameExtension

            return serviceCollection;
        }
    }
}
