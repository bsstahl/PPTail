﻿using System;
using System.Linq;
using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace PPTail
{
    public static class Program
    {
        // private const String _connectionStringProviderKey = "Provider";
        private const String _invalidArgumentsText = "Invalid Arguments:";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be fixed when globalization is done")]
        public static void Main(String[] args)
        {
            (var argsAreValid, var argumentErrors) = args.ValidateParameters();

            if (argsAreValid)
            {
                var (sourceConnection, targetConnection, templateConnection, switches) = args.ParseArguments();

                var loggingLevelSwitch = switches.Contains(Constants.VERBOSE_SWITCH)
                    ? new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Verbose)
                    : new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Warning);

                var logFormatter = new Serilog.Formatting.Json.JsonFormatter();

                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console(logFormatter)
                    .MinimumLevel.ControlledBy(loggingLevelSwitch)
                    .CreateLogger();

                var serviceProvider = new ServiceCollection()
                    .AddSourceRepository(sourceConnection)
                    .AddTargetRepository(targetConnection)
                    .AddTemplateRepository(templateConnection)
                    .AddServices()
                    .AddLogging(l => l.AddSerilog(Log.Logger))
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
                    var outputRepo = serviceProvider.GetService<IOutputRepository>();
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

                throw new ArgumentException("One or more arguments are invalid");
            }
        }

    }
}
