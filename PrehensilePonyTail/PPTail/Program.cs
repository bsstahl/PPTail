using System;
using System.Linq;
using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail
{
    public static class Program
    {
        private const String _connectionStringProviderKey = "Provider";
        private const String _connectionStringFilePathKey = "FilePath";

        private const String _invalidArgumentsText = "Invalid Arguments:";

        public static void Main(String[] args)
        {
            (var argsAreValid, var argumentErrors) = args.ValidateParameters();

            if (argsAreValid)
            {
                var (sourceConnection, targetConnection, templateConnection, switches) = args.ParseArguments();
                var settings = (null as ISettings).Create(sourceConnection, targetConnection);

                string templateProvider = templateConnection.GetConnectionStringValue(_connectionStringProviderKey);
                string templatePath = templateConnection.GetConnectionStringValue(_connectionStringFilePathKey);

                var serviceProvider = new ServiceCollection()
                    .AddSingleton<ISettings>(settings)  // TODO: Eliminate ISettings
                    .AddSourceRepository(settings) // TODO: Remove from container
                    .AddTemplateRepository(templateProvider, templatePath)
                    .AddServices()
                    .BuildServiceProvider();

                // Generate the website pages
                var siteBuilder = serviceProvider.GetService<ISiteBuilder>();
                var sitePages = siteBuilder.Build();

                if (switches.Contains(Constants.VALIDATEONLY_SWITCH))
                {
                    var contentRepo = serviceProvider.GetService<IContentRepository>();
                    var siteSettings = contentRepo.GetSiteSettings();
                    Console.WriteLine($"{siteSettings.Title} generated successfully.");
                    Console.WriteLine($"\tPost Pages: {sitePages.Count(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage)}");
                    Console.WriteLine($"\tContent Pages: {sitePages.Count(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage)}");
                }
                else
                {
                    // Store the resulting output
                    var outputRepoInstanceName = settings.TargetConnection.GetConnectionStringValue(_connectionStringProviderKey);
                    var outputRepo = serviceProvider.GetNamedService<IOutputRepository>(outputRepoInstanceName);
                    outputRepo.Save(sitePages);
                }
            }
            else
            {
                Console.WriteLine(_invalidArgumentsText);
                foreach (var argumentError in argumentErrors)
                {
                    Console.WriteLine($"\t{argumentError}");
                }

                Console.WriteLine();
            }
        }

    }
}
