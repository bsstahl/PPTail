using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestHelperExtensions;
using PPTail.Interfaces;
using PPTail.Entities;

namespace PPTail.Generator.Template.Test
{
    public static class Extensions
    {
        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var container = new ServiceCollection();
            container.AddSingleton<ISettings>((null as ISettings).Create());
            return container;
        }

        public static ITemplateProcessor Create(this ITemplateProcessor ignore, IServiceCollection container)
        {
            return ignore.Create(container.BuildServiceProvider());
        }

        public static ITemplateProcessor Create(this ITemplateProcessor ignore, IServiceProvider serviceProvider)
        {
            return new TemplateProcessor(serviceProvider);
        }

        public static ISettings Create(this ISettings ignore)
        {
            return ignore.Create("MM/dd/yyyy", "MM/dd/yyyy hh:mm", string.Empty.GetRandom(), string.Empty.GetRandom(3));
        }

        public static ISettings Create(this ISettings ignore, string dateFormatSpecifier,
            string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension)
        {
            return new Entities.Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                OutputFileExtension = outputFileExtension
            };
        }

        public static IEnumerable<Entities.Template> Create(this IEnumerable<Entities.Template> ignore)
        {
            var templates = new List<Entities.Template>();
            foreach (Enumerations.TemplateType templateType in Enum.GetValues(typeof(Enumerations.TemplateType)))
                templates.Add((null as Entities.Template).Create(templateType));
            return templates;
        }

        public static Entities.Template Create(this Entities.Template ignore)
        {
            throw new NotImplementedException();
        }

        public static Entities.Template Create(this Entities.Template ignore, Enumerations.TemplateType templateType)
        {
            return new Entities.Template()
            {
                Content = string.Empty.GetRandom(),
                TemplateType = templateType
            };
        }

        public static ContentItem Create(this ContentItem ignore)
        {
            return ignore.Create(string.Empty.GetRandom(), new List<Guid>() { Guid.NewGuid() }, string.Empty.GetRandom(), string.Empty.GetRandom(), true.GetRandom(), DateTime.UtcNow.AddMinutes(10.GetRandom()), DateTime.UtcNow.AddHours(10.GetRandom(1)), string.Empty.GetRandom(), new List<string>() { string.Empty.GetRandom() }, string.Empty.GetRandom());
        }

        public static ContentItem Create(this ContentItem ignore, string author, IEnumerable<Guid> categoryIds, string content, string description, bool isPublished, DateTime lastModDate, DateTime pubDate, string slug, IEnumerable<string> tags, string title)
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
            return ignore.Create(25.GetRandom(3));
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, int count)
        {
            var result = new List<ContentItem>();
            for (int i = 0; i < count; i++)
                result.Add((null as ContentItem).Create());
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
