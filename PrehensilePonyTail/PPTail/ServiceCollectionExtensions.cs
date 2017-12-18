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
            // Configure Dependencies
            IServiceCollection container = new ServiceCollection();


            // Data source 
            // container.AddSingleton<IContentRepository>(c => new PPTail.Data.FileSystem.Repository(c));
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.FileSystem.Wordpress.Repository(c));

            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IEnumerable<Template>>(templates);

            container.AddSingleton<IFile>(c => new PPTail.Io.File());
            container.AddSingleton<IDirectory>(c => new PPTail.Io.Directory());
            container.AddSingleton<ITagCloudStyler>(c => new PPTail.Generator.TagCloudStyler.DeviationStyler(c));
            container.AddSingleton<INavigationProvider>(c => new Generator.Navigation.BasicProvider(c));
            container.AddSingleton<IArchiveProvider>(c => new PPTail.Generator.Archive.BasicProvider(c));
            container.AddSingleton<IContactProvider>(c => new PPTail.Generator.Contact.TemplateProvider(c));
            container.AddSingleton<IPageGenerator>(c => new PPTail.Generator.T4Html.PageGenerator(c));
            container.AddSingleton<IContentItemPageGenerator>(c => new PPTail.Generator.ContentPage.PageGenerator(c));
            container.AddSingleton<IOutputRepository>(c => new PPTail.Output.FileSystem.Repository(c));
            container.AddSingleton<ISearchProvider>(c => new PPTail.Generator.Search.PageGenerator(c));
            container.AddSingleton<IRedirectProvider>(c => new PPTail.Generator.Redirect.RedirectProvider(c));
            container.AddSingleton<ISyndicationProvider>(c => new PPTail.Generator.Syndication.SyndicationProvider(c));
            container.AddSingleton<IHomePageGenerator>(c => new PPTail.Generator.HomePage.HomePageGenerator(c));
            container.AddSingleton<ILinkProvider>(c => new PPTail.Generator.Links.LinkProvider(c));
            container.AddSingleton<ITemplateProcessor>(c => new PPTail.Generator.Template.TemplateProcessor(c));
            container.AddSingleton<IContentEncoder>(c => new PPTail.Generator.Encoder.ContentEncoder(c));
            container.AddSingleton<ISiteBuilder>(c => new PPTail.SiteGenerator.Builder(c));

            var contentRepo = container.BuildServiceProvider().GetService<Interfaces.IContentRepository>();

            var siteSettings = contentRepo.GetSiteSettings();
            container.AddSingleton<SiteSettings>(siteSettings);

            var categories = contentRepo.GetCategories();
            container.AddSingleton<IEnumerable<Category>>(categories);

            return container;
        }
    }
}
