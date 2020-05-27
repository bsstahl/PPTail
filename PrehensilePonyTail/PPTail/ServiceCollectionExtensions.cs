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
            _ = container.AddSingleton<IFile>(c => new PPTail.Io.File());
            _ = container.AddSingleton<IDirectory>(c => new PPTail.Io.Directory());
            _ = container.AddSingleton<ITagCloudStyler>(c => new PPTail.Generator.TagCloudStyler.DeviationStyler(c));
            _ = container.AddSingleton<INavigationProvider>(c => new Generator.Navigation.BasicProvider(c));
            _ = container.AddSingleton<IArchiveProvider>(c => new PPTail.Generator.Archive.BasicProvider(c));
            _ = container.AddSingleton<IContactProvider>(c => new PPTail.Generator.Contact.TemplateProvider(c));
            _ = container.AddSingleton<IPageGenerator>(c => new PPTail.Generator.T4Html.PageGenerator(c));
            _ = container.AddSingleton<IContentItemPageGenerator>(c => new PPTail.Generator.ContentPage.PageGenerator(c));
            _ = container.AddSingleton<IOutputRepository>(c => new PPTail.Output.FileSystem.Repository(c));
            _ = container.AddSingleton<ISearchProvider>(c => new PPTail.Generator.Search.PageGenerator(c));
            _ = container.AddSingleton<IRedirectProvider>(c => new PPTail.Generator.Redirect.RedirectProvider(c));
            _ = container.AddSingleton<ISyndicationProvider>(c => new PPTail.Generator.Syndication.SyndicationProvider(c));
            _ = container.AddSingleton<IHomePageGenerator>(c => new PPTail.Generator.HomePage.HomePageGenerator(c));
            _ = container.AddSingleton<ILinkProvider>(c => new PPTail.Generator.Links.LinkProvider(c));
            _ = container.AddSingleton<ITemplateProcessor>(c => new PPTail.Generator.Template.TemplateProcessor(c));
            _ = container.AddSingleton<IContentEncoder>(c => new PPTail.Generator.Encoder.ContentEncoder(c));
            _ = container.AddSingleton<ISiteBuilder>(c => new PPTail.SiteGenerator.Builder(c));

            //var siteSettings = contentRepo.GetSiteSettings();
            //container.AddSingleton<SiteSettings>(siteSettings);

            //var categories = contentRepo.GetCategories();
            //container.AddSingleton<IEnumerable<Category>>(categories);

            return container;
        }
    }
}
