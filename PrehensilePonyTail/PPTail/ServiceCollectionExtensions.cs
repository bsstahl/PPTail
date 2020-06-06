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
            _ = container.AddSingleton<ISettings>(settings);
            _ = container.AddSingleton<IEnumerable<Template>>(templates);

            // Source Repositories
            _ = container.AddSingleton<IContentRepository, PPTail.Data.FileSystem.Repository>();
            _ = container.AddSingleton<IContentRepository, PPTail.Data.Ef.Repository>();
            _ = container.AddSingleton<IContentRepository, PPTail.Data.NativeJson.Repository>();
            _ = container.AddSingleton<IContentRepository, PPTail.Data.WordpressFiles.Repository>();
            _ = container.AddSingleton<IContentRepository, PPTail.Data.PhotoBlog.Repository>();
            _ = container.AddSingleton<IContentRepository, PPTail.Data.MediaBlog.Repository>();
            _ = container.AddSingleton<IContentRepository, PPTail.Data.Forestry.Repository>();

            // Additional Service Providers
            _ = container.AddSingleton<IFile>(c => new Io.File());
            _ = container.AddSingleton<IDirectory>(c => new Io.Directory());
            _ = container.AddSingleton<ITagCloudStyler>(c => new Generator.TagCloudStyler.DeviationStyler(c));
            _ = container.AddSingleton<INavigationProvider>(c => new Generator.Navigation.BootstrapProvider(c));
            _ = container.AddSingleton<IArchiveProvider>(c => new Generator.Archive.BasicProvider(c));
            _ = container.AddSingleton<IContactProvider>(c => new Generator.Contact.TemplateProvider(c));
            _ = container.AddSingleton<IPageGenerator>(c => new Generator.T4Html.PageGenerator(c));
            _ = container.AddSingleton<IContentItemPageGenerator>(c => new Generator.ContentPage.PageGenerator(c));
            _ = container.AddSingleton<IOutputRepository>(c => new Output.FileSystem.Repository(c));
            _ = container.AddSingleton<ISearchProvider>(c => new Generator.Search.PageGenerator(c));
            _ = container.AddSingleton<IRedirectProvider>(c => new Generator.Redirect.RedirectProvider(c));
            _ = container.AddSingleton<ISyndicationProvider>(c => new Generator.Syndication.SyndicationProvider(c));
            _ = container.AddSingleton<IHomePageGenerator>(c => new Generator.HomePage.HomePageGenerator(c));
            _ = container.AddSingleton<ILinkProvider>(c => new Generator.Links.LinkProvider(c));
            _ = container.AddSingleton<ITemplateProcessor>(c => new Generator.Template.TemplateProcessor(c));
            _ = container.AddSingleton<IContentEncoder>(c => new Generator.Encoder.ContentEncoder(c));
            _ = container.AddSingleton<ISiteBuilder>(c => new SiteGenerator.Builder(c));

            //var siteSettings = contentRepo.GetSiteSettings();
            //container.AddSingleton<SiteSettings>(siteSettings);

            //var categories = contentRepo.GetCategories();
            //container.AddSingleton<IEnumerable<Category>>(categories);

            return container;
        }
    }
}
