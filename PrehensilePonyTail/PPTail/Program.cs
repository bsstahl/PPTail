using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;

namespace PPTail
{
    public static class Program
    {
        private const String _connectionStringProviderKey = "Provider";
        private const String _invalidArgumentsText = "Invalid Arguments:";

        public static void Main(String[] args)
        {
            (var argsAreValid, var argumentErrors) = args.ValidateParameters();

            if (argsAreValid)
            {
                var (sourceConnection, targetConnection, templateConnection, switches) = args.ParseArguments();

                var settings = (null as ISettings).Create(sourceConnection, targetConnection, templateConnection);
                var templateFullPath = System.IO.Path.GetFullPath(templateConnection);
                var templates = (null as IEnumerable<Template>).Create(templateFullPath);

                var container = (null as IServiceCollection).Create(settings, templates);
                var serviceProvider = container.BuildServiceProvider();

                // Generate the website pages
                var siteBuilder = serviceProvider.GetService<ISiteBuilder>();
                var sitePages = siteBuilder.Build();

                // Store the resulting output
                var outputRepoInstanceName = settings.TargetConnection.GetConnectionStringValue(_connectionStringProviderKey);
                var outputRepo = serviceProvider.GetNamedService<IOutputRepository>(outputRepoInstanceName);

                if (switches.Contains(Constants.VALIDATEONLY_SWITCH))
                    Console.WriteLine($"Site output skipped due to '{Constants.VALIDATEONLY_SWITCH}' switch setting.");
                else
                    outputRepo.Save(sitePages);
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
