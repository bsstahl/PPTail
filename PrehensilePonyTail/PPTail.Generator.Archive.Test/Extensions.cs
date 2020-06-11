using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Generator.Archive.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IArchiveProvider Create(this IArchiveProvider ignore)
        {
            IServiceCollection container = (null as IServiceCollection).Create();
            return ignore.Create(container.BuildServiceProvider());
        }

        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var container = new ServiceCollection();

            var settings = (null as ISettings).CreateDefault();
            container.AddSingleton<ISettings>(settings);

            var templates = (null as IEnumerable<Template>).Create();
            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo
                .Setup(r => r.GetAllTemplates())
                .Returns(templates);
            container.AddSingleton<ITemplateRepository>(templateRepo.Object);

            var templateProcessor = (null as ITemplateProcessor).Create();
            container.AddSingleton<ITemplateProcessor>(templateProcessor);


            return container;
        }

        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore)
        {
            var templates = new List<Template>();
            templates.Add((null as Template).Create(Enumerations.TemplateType.Archive));
            templates.Add((null as Template).Create(Enumerations.TemplateType.ArchiveItem));
            return templates;
        }

        public static Template Create(this Template ignore)
        {
            return ignore.Create(Enumerations.TemplateType.Archive);
        }

        public static Template Create(this Template ignore, Enumerations.TemplateType templateType)
        {
            return new Template()
            {
                Content = "{Content}",
                TemplateType = templateType
            };
        }

        public static IArchiveProvider Create(this IArchiveProvider ignore, IServiceProvider serviceProvider)
        {
            return new BasicProvider(serviceProvider);
        }

        public static ISettings CreateDefault(this ISettings ignore)
        {
            return ignore.CreateDefault("MM/dd/yyyy hh:mm");
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
            String author = string.Empty.GetRandom();
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

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, Int32 count)
        {
            var result = new List<ContentItem>();
            for (Int32 i = 0; i < count; i++)
                result.Add((null as ContentItem).Create());
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

    }
}
