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
                Mock.Of<Settings>(), Mock.Of<SiteSettings>());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IEnumerable<Template> templates)
        {
            return ignore.Create(templates, Mock.Of<Settings>(), Mock.Of<SiteSettings>());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IEnumerable<Template> templates, Settings settings, SiteSettings siteSettings)
        {
            var serviceCollection = new ServiceCollection();

            if (templates != null)
                serviceCollection.AddSingleton<IEnumerable<Template>>(templates);

            if (settings != null)
                serviceCollection.AddSingleton<Settings>(settings);

            if (siteSettings != null)
                serviceCollection.AddSingleton<SiteSettings>(siteSettings);

            return new PageGenerator(serviceCollection.BuildServiceProvider());
        }

        public static ISearchProvider Create(this ISearchProvider ignore, IServiceProvider serviceProvider)
        {
            return new PageGenerator(serviceProvider);
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
                Name = name,
                TemplateType = templateType
            };
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, ContentItem item)
        {
            return new List<ContentItem>() { item };
        }

        public static ContentItem Create(this ContentItem ignore, string tag)
        {
            return ignore.Create(new List<string>() { tag });
        }

        public static ContentItem Create(this ContentItem ignore, IEnumerable<string> tags)
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


    }
}
