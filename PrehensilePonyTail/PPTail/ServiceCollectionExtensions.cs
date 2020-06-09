using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace PPTail
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddTemplateRepository(this IServiceCollection container, ISettings settings)
        {
            var templateProvider = settings.TemplateConnection.GetConnectionStringValue("Provider");
            var templatePath = settings.TemplateConnection.GetConnectionStringValue("FilePath");

            if (templateProvider.ToUpperInvariant().Equals("PPTAIL.TEMPLATES.FILESYSTEM.READREPOSITORY"))
                container.AddSingleton<ITemplateRepository>(c => new Templates.FileSystem.ReadRepository(c, templatePath));
            else if (templateProvider.ToUpperInvariant().Equals("PPTAIL.TEMPLATES.YAML.READREPOSITORY"))
                container.AddSingleton<ITemplateRepository>(c => new Templates.Yaml.ReadRepository(c, templatePath));
            else
                throw new ArgumentException($"Invalid Template provider type '{templateProvider}'.", nameof(settings));

            return container;
        }

        internal static IServiceCollection AddSourceRepository(this IServiceCollection container, ISettings settings)
        {
            var provider = settings.SourceConnection.GetConnectionStringValue("Provider");
            var path = settings.SourceConnection.GetConnectionStringValue("FilePath");

            if (provider.ToUpperInvariant().Equals("PPTAIL.DATA.FILESYSTEM.REPOSITORY"))
                container.AddSingleton<IContentRepository, PPTail.Data.FileSystem.Repository>();
            else if (provider.ToUpperInvariant().Equals("PPTAIL.DATA.EF.REPOSITORY"))
                container.AddSingleton<IContentRepository, PPTail.Data.Ef.Repository>();
            else if (provider.ToUpperInvariant().Equals("PPTAIL.DATA.NATIVEJSON.REPOSITORY"))
                container.AddSingleton<IContentRepository, PPTail.Data.NativeJson.Repository>();
            else if (provider.ToUpperInvariant().Equals("PPTAIL.DATA.WORDPRESSFILES.REPOSITORY"))
                container.AddSingleton<IContentRepository, PPTail.Data.WordpressFiles.Repository>();
            else if (provider.ToUpperInvariant().Equals("PPTAIL.DATA.PHOTOBLOG.REPOSITORY"))
                container.AddSingleton<IContentRepository, PPTail.Data.PhotoBlog.Repository>();
            else if (provider.ToUpperInvariant().Equals("PPTAIL.DATA.MEDIABLOG.REPOSITORY"))
                container.AddSingleton<IContentRepository, PPTail.Data.MediaBlog.Repository>();
            else if (provider.ToUpperInvariant().Equals("PPTAIL.DATA.FORESTRY.REPOSITORY"))
                container.AddSingleton<IContentRepository, PPTail.Data.Forestry.Repository>();
            else
                throw new ArgumentException($"Unknown source provider '{provider}'", nameof(settings));

            return container;
        }

        internal static IServiceCollection AddServices(this IServiceCollection container)
        {
            return container
                .AddSingleton<IFile>(c => new Io.File())
                .AddSingleton<IDirectory>(c => new Io.Directory())
                .AddSingleton<ITagCloudStyler>(c => new Generator.TagCloudStyler.DeviationStyler(c))
                .AddSingleton<INavigationProvider>(c => new Generator.Navigation.BootstrapProvider(c))
                .AddSingleton<IArchiveProvider>(c => new Generator.Archive.BasicProvider(c))
                .AddSingleton<IContactProvider>(c => new Generator.Contact.TemplateProvider(c))
                .AddSingleton<IPageGenerator>(c => new Generator.T4Html.PageGenerator(c))
                .AddSingleton<IContentItemPageGenerator>(c => new Generator.ContentPage.PageGenerator(c))
                .AddSingleton<IOutputRepository>(c => new Output.FileSystem.Repository(c))
                .AddSingleton<ISearchProvider>(c => new Generator.Search.PageGenerator(c))
                .AddSingleton<IRedirectProvider>(c => new Generator.Redirect.RedirectProvider(c))
                .AddSingleton<ISyndicationProvider>(c => new Generator.Syndication.SyndicationProvider(c))
                .AddSingleton<IHomePageGenerator>(c => new Generator.HomePage.HomePageGenerator(c))
                .AddSingleton<ILinkProvider>(c => new Generator.Links.LinkProvider(c))
                .AddSingleton<ITemplateProcessor>(c => new Generator.Template.TemplateProcessor(c))
                .AddSingleton<IContentEncoder>(c => new Generator.Encoder.ContentEncoder(c))
                .AddSingleton<ISiteBuilder>(c => new SiteGenerator.Builder(c));
        }
    }
}
