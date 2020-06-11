using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Generator.Links.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            // Contains mocks of all required dependencies
            var settings = (null as ISettings).Create();

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);

            return container;
        }

        public static ILinkProvider Create(this ILinkProvider ignore)
        {
            var container = (null as IServiceCollection).Create();
            return (null as ILinkProvider).Create(container);
        }

        public static ILinkProvider Create(this ILinkProvider ignore, IServiceCollection container)
        {
            return new PPTail.Generator.Links.LinkProvider(container.BuildServiceProvider());
        }

        public static ISettings Create(this ISettings ignore)
        {
            return new Settings()
            {
                DateFormatSpecifier = "MM/dd/yyyy",
                DateTimeFormatSpecifier = "MM/dd/yyyy hh:mm",
                ItemSeparator = string.Empty.GetRandom(),
                OutputFileExtension = string.Empty.GetRandom()
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
