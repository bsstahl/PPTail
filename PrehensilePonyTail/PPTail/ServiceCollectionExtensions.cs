using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace PPTail
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddTemplateRepository(this IServiceCollection container, string templateConnection)
        {
            const String _yamlReadRepoName = "PPTAIL.TEMPLATES.YAML.READREPOSITORY";
            const String _fileReadRepoName = "PPTAIL.TEMPLATES.FILESYSTEM.READREPOSITORY";

            const String _connectionStringProviderKey = "Provider";

            String templateProvider = templateConnection.GetConnectionStringValue(_connectionStringProviderKey);

            if (templateProvider.ToUpperInvariant() == _fileReadRepoName)
                container.AddSingleton<ITemplateRepository>(c => new Templates.FileSystem.ReadRepository(c, templateConnection));
            else if (templateProvider.ToUpperInvariant() == _yamlReadRepoName)
                container.AddSingleton<ITemplateRepository>(c => new Templates.Yaml.ReadRepository(c, templateConnection));
            else
                throw new ArgumentException($"Invalid Template provider type '{templateProvider} in Template Connection string'.", nameof(templateConnection));

            return container;
        }

        internal static IServiceCollection AddTargetRepository(this IServiceCollection container, string targetConnection)
        {
            const String _fileRepoName = "PPTAIL.OUTPUT.FILESYSTEM.REPOSITORY";

            const String _connectionStringProviderKey = "Provider";

            String targetProvider = targetConnection.GetConnectionStringValue(_connectionStringProviderKey);

            if (targetProvider.ToUpperInvariant() == _fileRepoName)
                container.AddSingleton<IOutputRepository>(c => new PPTail.Output.FileSystem.Repository(c, targetConnection));
            else
                throw new ArgumentException($"Invalid Target provider type '{targetProvider} in Template Connection string'.", nameof(targetConnection));

            return container;
        }

        internal static IServiceCollection AddSourceRepository(this IServiceCollection container, ISettings settings)
        {
            var provider = settings.SourceConnection.GetConnectionStringValue("Provider");
            var path = settings.SourceConnection.GetConnectionStringValue("FilePath");

            if (provider.ToUpperInvariant() == "PPTAIL.DATA.FILESYSTEM.REPOSITORY")
                container.AddSingleton<IContentRepository, PPTail.Data.FileSystem.Repository>();
            else if (provider.ToUpperInvariant() == "PPTAIL.DATA.EF.REPOSITORY")
                container.AddSingleton<IContentRepository, PPTail.Data.Ef.Repository>();
            else if (provider.ToUpperInvariant() == "PPTAIL.DATA.NATIVEJSON.REPOSITORY")
                container.AddSingleton<IContentRepository, PPTail.Data.NativeJson.Repository>();
            else if (provider.ToUpperInvariant() == "PPTAIL.DATA.WORDPRESSFILES.REPOSITORY")
                container.AddSingleton<IContentRepository, PPTail.Data.WordpressFiles.Repository>();
            else if (provider.ToUpperInvariant() == "PPTAIL.DATA.PHOTOBLOG.REPOSITORY")
                container.AddSingleton<IContentRepository, PPTail.Data.PhotoBlog.Repository>();
            else if (provider.ToUpperInvariant() == "PPTAIL.DATA.MEDIABLOG.REPOSITORY")
                container.AddSingleton<IContentRepository, PPTail.Data.MediaBlog.Repository>();
            else if (provider.ToUpperInvariant() == "PPTAIL.DATA.FORESTRY.REPOSITORY")
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
