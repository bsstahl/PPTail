using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using TestHelperExtensions;

namespace PPTail.Generator.Syndication.Test
{
    public static class Extensions
    {
        const string _defaultSyndicationTemplateContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rss><channel>{Content}</channel></rss>";
        const string _defaultSyndicationItemTemplateContent = "<item><title>{Title}</title><description>{Content}</description><link>{Link}</link><author>{Author}</author><guid>{Permalink}</guid><pubDate>{PublicationDate}</pubDate><category>{CategoryList}</category></item>";

        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var container = new ServiceCollection();

            var siteSettings = (null as SiteSettings).Create();
            container.AddSingleton<SiteSettings>(siteSettings);

            var settings = (null as ISettings).Create();
            container.AddSingleton<ISettings>(settings);

            var syndicationTemplate = new Template() { Content = _defaultSyndicationTemplateContent, TemplateType = Enumerations.TemplateType.Syndication };
            var syndicationItemTemplate = new Template() { Content = _defaultSyndicationItemTemplateContent, TemplateType = Enumerations.TemplateType.SyndicationItem };
            var templates = new List<Template>() { syndicationTemplate, syndicationItemTemplate };

            container.AddSingleton<IEnumerable<Template>>(templates);

            return container;
        }

        public static ISyndicationProvider Create(this ISyndicationProvider ignore, IServiceCollection container)
        {
            return new SyndicationProvider(container.BuildServiceProvider());
        }

        public static ISyndicationProvider Create(this ISyndicationProvider ignore, IServiceProvider serviceProvider)
        {
            return new SyndicationProvider(serviceProvider);
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
            var contentItems = new List<ContentItem>();
            for (int i = 0; i < count; i++)
                contentItems.Add((null as ContentItem).Create());
            return contentItems;
        }

        public static SiteSettings Create(this SiteSettings ignore)
        {
            return ignore.Create("My Test Blog", "A blog of epic scalability", 10.GetRandom(2), 10.GetRandom(5));
        }

        public static SiteSettings Create(this SiteSettings ignore, string title, string description, int postsPerPage, int postsPerFeed)
        {
            return new SiteSettings()
            {
                Title = title,
                Description = description,
                PostsPerPage = postsPerPage,
                PostsPerFeed = postsPerFeed
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

    }
}
