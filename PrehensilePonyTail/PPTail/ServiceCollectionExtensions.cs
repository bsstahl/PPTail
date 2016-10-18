using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Create(this IServiceCollection ignore, ISettings settings, IEnumerable<Template> templates)
        {
            IServiceCollection container = new ServiceCollection();

            // Configure Dependencies
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IEnumerable<Template>>(templates);

            container.AddSingleton<Interfaces.IFile>(c => new PPTail.Io.File());
            container.AddSingleton<Interfaces.IDirectory>(c => new PPTail.Io.Directory());
            container.AddSingleton<Interfaces.IContentRepository>(c => new PPTail.Data.FileSystem.Repository(c));
            container.AddSingleton<Interfaces.ITagCloudStyler>(c => new PPTail.Generator.TagCloudStyler.DeviationStyler(c));
            container.AddSingleton<Interfaces.INavigationProvider>(c => new Generator.Navigation.BasicProvider(c));
            container.AddSingleton<Interfaces.IArchiveProvider>(c => new PPTail.Generator.Archive.BasicProvider(c));
            container.AddSingleton<Interfaces.IContactProvider>(c => new PPTail.Generator.Contact.TemplateProvider(c));
            container.AddSingleton<Interfaces.IPageGenerator>(c => new PPTail.Generator.T4Html.PageGenerator(c));
            container.AddSingleton<Interfaces.IOutputRepository>(c => new PPTail.Output.FileSystem.Repository(c));
            container.AddSingleton<Interfaces.ISearchProvider>(c => new PPTail.Generator.Search.PageGenerator(c));
            container.AddSingleton<SiteGenerator.Builder>(c => new PPTail.SiteGenerator.Builder(c));

            var contentRepo = container.BuildServiceProvider().GetService<Interfaces.IContentRepository>();

            var siteSettings = contentRepo.GetSiteSettings();
            container.AddSingleton<SiteSettings>(siteSettings);

            var categories = contentRepo.GetCategories();
            container.AddSingleton<IEnumerable<Category>>(categories);

            return container;
        }
    }
}
