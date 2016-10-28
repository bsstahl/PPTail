using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TestHelperExtensions;
using Moq;

namespace PPTail.Generator.Contact.Test
{
    public static class Extensions
    {
        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var container = new ServiceCollection();

            var siteSettings = (null as SiteSettings).Create();
            container.AddSingleton<SiteSettings>(siteSettings);

            var settings = (null as Settings).Create();
            container.AddSingleton<ISettings>(settings);

            var templateProcessor = (null as ITemplateProcessor).Create();
            container.AddSingleton<ITemplateProcessor>(templateProcessor);

            var templates = (null as IEnumerable<Template>).Create();
            container.AddSingleton<IEnumerable<Template>>(templates);

            return container;
        }

        public static IContactProvider Create(this IContactProvider ignore)
        {
            return ignore.Create((null as IServiceCollection).Create());
        }

        public static IContactProvider Create(this IContactProvider ignore, IServiceCollection container)
        {
            return ignore.Create(container.BuildServiceProvider());
        }

        public static IContactProvider Create(this IContactProvider ignore, IServiceProvider serviceProvider)
        {
            return new TemplateProvider(serviceProvider);
        }

        public static ITemplateProcessor Create(this ITemplateProcessor ignore)
        {
            return Mock.Of<ITemplateProcessor>();
        }

        public static Template Create(this Template ignore)
        {
            return ignore.Create("{NavigationMenu} {Sidebar} {Content}", string.Empty.GetRandom(), Enumerations.TemplateType.ContactPage);
        }

        public static Template Create(this Template ignore, string content, string name, Enumerations.TemplateType templateType)
        {
            return new Template()
            {
                Content = content,
                TemplateType = templateType
            };
        }

        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore)
        {
            var templates = new List<Template>();
            templates.Add((null as Template).Create());
            return templates;
        }

        public static SiteSettings Create(this SiteSettings ignore)
        {
            return ignore.Create(string.Empty.GetRandom(), 10.GetRandom(3), string.Empty.GetRandom());
        }

        public static SiteSettings Create(this SiteSettings ignore, string description, int postsPerPage, string title)
        {
            return new SiteSettings()
            {
                Description = description,
                PostsPerPage = postsPerPage,
                Title = title
            };
        }

        public static ISettings Create(this ISettings ignore)
        {
            return ignore.Create("yyyyMMdd", "yyyyMMdd hh:mm", "<hr/>", "html");
        }

        public static ISettings Create(this ISettings ignore, string dateFormatSpecifier, string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension)
        {
            return new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                OutputFileExtension = outputFileExtension
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

    }
}
