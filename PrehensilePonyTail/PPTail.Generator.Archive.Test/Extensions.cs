using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Generator.Archive.Test
{
    public static class Extensions
    {
        public static IArchiveProvider Create(this IArchiveProvider ignore)
        {
            var container = new ServiceCollection();

            var template = new Template()
            {
                Content = "{Content}",
                Name = string.Empty.GetRandom(),
                TemplateType = Enumerations.TemplateType.HomePage
            };

            var templates = new List<Template>();
            templates.Add(template);
            container.AddSingleton<IEnumerable<Template>>(templates);

            // Add dependencies here as needed
            return ignore.Create(container.BuildServiceProvider());
        }

        public static IArchiveProvider Create(this IArchiveProvider ignore, IServiceProvider serviceProvider)
        {
            return new BasicProvider(serviceProvider);
        }

        public static Settings CreateDefault(this Settings ignore, string dateTimeFormatSpecifier)
        {
            return ignore.CreateDefault(dateTimeFormatSpecifier, "html");
        }

        public static Settings CreateDefault(this Settings ignore, string dateTimeFormatSpecifier, string outputFileExtension)
        {
            var settings = new Settings();
            settings.DateTimeFormatSpecifier = dateTimeFormatSpecifier;
            settings.outputFileExtension = outputFileExtension;
            return settings;
        }

        public static SiteSettings Create(this SiteSettings ignore)
        {
            var result = new SiteSettings();
            result.Description = string.Empty.GetRandom();
            result.PostsPerPage = 10.GetRandom(2);
            result.Title = string.Empty.GetRandom();
            return result;
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

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, int count)
        {
            var result = new List<ContentItem>();
            for (int i = 0; i < count; i++)
                result.Add((null as ContentItem).Create());
            return result;
        }

    }
}
