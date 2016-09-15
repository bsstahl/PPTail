using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;

namespace PPTail
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Create(this IServiceCollection ignore, Settings settings)
        {
            IServiceCollection container = new ServiceCollection();

            // Configure Dependencies
            container.AddSingleton<Settings>(settings);

            // var fileSystem = new PPTail.Data.FileSystem.

            var repo = new PPTail.Data.FileSystem.Repository(container);
            container.AddSingleton<Interfaces.IContentRepository>(repo);

            var pageGenerator = new PPTail.Generator.T4Html.PageGenerator(container);
            container.AddSingleton<Interfaces.IPageGenerator>(pageGenerator);

            var builder = new PPTail.SiteGenerator.Builder(container);
            container.AddSingleton<SiteGenerator.Builder>(builder);

            return container;
        }
    }
}
