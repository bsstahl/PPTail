﻿using System;
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
        const String _connectionStringProviderKey = "Provider";
        private const String _invalidArgumentsText = "Invalid Arguments:";

        public static void Main(String[] args)
        {
            (var argsAreValid, var argumentErrors) = args.ValidateArguments();

            if (argsAreValid)
            {
                var (sourceConnection, targetConnection, templateConnection) = args.ParseArguments();

                var settings = (null as ISettings).Create(sourceConnection, targetConnection, templateConnection);
                var templates = (null as IEnumerable<Template>).Create(templateConnection);

                var container = (null as IServiceCollection).Create(settings, templates);
                var serviceProvider = container.BuildServiceProvider();

                // Generate the website pages
                var siteBuilder = serviceProvider.GetService<ISiteBuilder>();
                var sitePages = siteBuilder.Build();

                // Store the resulting output
                var outputRepoInstanceName = settings.TargetConnection.GetConnectionStringValue(_connectionStringProviderKey);
                var outputRepo = serviceProvider.GetNamedService<IOutputRepository>(outputRepoInstanceName);
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
