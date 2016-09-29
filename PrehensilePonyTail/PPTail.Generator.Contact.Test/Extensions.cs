using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TestHelperExtensions;

namespace PPTail.Generator.Contact.Test
{
    public static class Extensions
    {
        public static IContactProvider Create(this IContactProvider ignore)
        {
            var container = new ServiceCollection();

            var siteSettings = (null as SiteSettings).Create();
            container.AddSingleton<SiteSettings>(siteSettings);

            var settings = (null as Settings).Create();
            container.AddSingleton<Settings>(settings);

            var template = (null as Template).Create();
            var templates = new List<Template>() { template };
            container.AddSingleton<IEnumerable<Template>>(templates);

            return ignore.Create(container.BuildServiceProvider());
        }

        public static IContactProvider Create(this IContactProvider ignore, IServiceProvider serviceProvider)
        {
            return new TemplateProvider(serviceProvider);
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
                Name = name,
                TemplateType = templateType
            };
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

        public static Settings Create(this Settings ignore)
        {
            return ignore.Create("yyyyMMdd", "yyyyMMdd hh:mm", "<hr/>", "html");
        }

        public static Settings Create(this Settings ignore, string dateFormatSpecifier, string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension)
        {
            return new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                outputFileExtension = outputFileExtension
            };
        }
    }
}
