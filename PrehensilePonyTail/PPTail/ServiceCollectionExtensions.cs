﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;

namespace PPTail
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Create(this IServiceCollection ignore, Settings settings, IEnumerable<Template> templates)
        {
            IServiceCollection container = new ServiceCollection();

            // Configure Dependencies
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<IEnumerable<Template>>(templates);

            container.AddSingleton<Data.FileSystem.IFileSystem>(c => new PPTail.Data.FileSystem.FileSystemAbstraction.Provider());
            container.AddSingleton<Interfaces.IContentRepository>(c => new PPTail.Data.FileSystem.Repository(c));
            container.AddSingleton<Interfaces.ITagCloudStyler>(c => new PPTail.Generator.TagCloudStyler.DeviationStyler(c));
            container.AddSingleton<Interfaces.INavigationProvider>(c => new Generator.Navigation.BasicProvider(c));
            container.AddSingleton<Interfaces.IArchiveProvider>(c => new PPTail.Generator.Archive.BasicProvider(c));
            container.AddSingleton<Interfaces.IPageGenerator>(c => new PPTail.Generator.T4Html.PageGenerator(c));
            container.AddSingleton<SiteGenerator.Builder>(c => new PPTail.SiteGenerator.Builder(c));

            return container;
        }
    }
}
