using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;
using System;

namespace PPTail.Console.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseFileSystem(this IServiceCollection container)
    {
        return container
            .AddSingleton<IFile>(c => new PPTail.Io.File())
            .AddSingleton<IDirectory>(c => new PPTail.Io.Directory());
    }

    public static IServiceCollection AddTemplateRepository(this IServiceCollection container, string? templateConnection)
    {
        const String _yamlReadRepoName = "PPTAIL.TEMPLATES.YAML.READREPOSITORY";
        const String _fileReadRepoName = "PPTAIL.TEMPLATES.FILESYSTEM.READREPOSITORY";
        const String _connectionStringProviderKey = "Provider";
        
        String templateProvider = templateConnection.GetConnectionStringValue(_connectionStringProviderKey);

        if (templateProvider.Equals(_fileReadRepoName, StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<ITemplateRepository>(c => new Templates.FileSystem.ReadRepository(c, templateConnection!));
        else if (templateProvider.Equals(_yamlReadRepoName, StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<ITemplateRepository>(c => new Templates.Yaml.ReadRepository(c, templateConnection!));
        else
            throw new ArgumentException($"Invalid Template provider type '{templateProvider} in Template Connection string'.", nameof(templateConnection));

        return container;
    }

    public static IServiceCollection AddTargetRepository(this IServiceCollection container, string? targetConnection)
    {
        const String _fileRepoName = "PPTAIL.OUTPUT.FILESYSTEM.REPOSITORY";
        const String _connectionStringProviderKey = "Provider";

        String targetProvider = targetConnection.GetConnectionStringValue(_connectionStringProviderKey);

        if (targetProvider.Equals(_fileRepoName, StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IOutputRepository>(c => new PPTail.Output.FileSystem.Repository(c, targetConnection));
        else
            throw new ArgumentException($"Invalid Target provider type '{targetProvider} in Template Connection string'.", nameof(targetConnection));

        return container;
    }

    public static IServiceCollection AddSourceRepository(this IServiceCollection container, string? sourceConnection)
    {
        var provider = sourceConnection.GetConnectionStringValue("Provider");
        var path = sourceConnection.GetConnectionStringValue("FilePath");

        if (provider.Equals("PPTAIL.DATA.FILESYSTEM.REPOSITORY", StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.FileSystem.Repository(c, sourceConnection));
        else if (provider.Equals("PPTAIL.DATA.EF.REPOSITORY", StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.Ef.Repository(c));
        else if (provider.Equals("PPTAIL.DATA.NATIVEJSON.REPOSITORY", StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.NativeJson.Repository(c, sourceConnection));
        else if (provider.Equals("PPTAIL.DATA.WORDPRESSFILES.REPOSITORY", StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.WordpressFiles.Repository(c, sourceConnection));
        else if (provider.Equals("PPTAIL.DATA.PHOTOBLOG.REPOSITORY", StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.PhotoBlog.Repository(c, sourceConnection));
        else if (provider.Equals("PPTAIL.DATA.MEDIABLOG.REPOSITORY", StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.MediaBlog.Repository(c, sourceConnection!));
        else if (provider.Equals("PPTAIL.DATA.MEDIABLOG.YAMLREPOSITORY", StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.MediaBlog.YamlRepository(c, sourceConnection!));
        else if (provider.Equals("PPTAIL.DATA.FORESTRY.REPOSITORY", StringComparison.OrdinalIgnoreCase))
            container.AddSingleton<IContentRepository>(c => new PPTail.Data.Forestry.Repository(c, sourceConnection));
        else
            throw new ArgumentException($"Unknown source provider '{provider}'", nameof(sourceConnection));

        return container;
    }

    public static IServiceCollection AddServices(this IServiceCollection container)
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
