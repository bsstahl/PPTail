using System;
using System.Linq;
using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using PPTail.Console.Common.Extensions;
using PPTail.Console.Common;
using PPTail.Output.FileSystem.Extensions;

namespace PPTail;

public static class Program
{
    private const String _invalidArgumentsText = "Invalid Arguments:";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be fixed when globalization is done")]
    public static void Main(String[] args)
    {
        (var argsAreValid, var argumentErrors) = args.ValidateParameters("PPtail.exe");

        if (argsAreValid)
        {
            var (sourceConnection, targetConnection, templateConnection, switches) = args.ParseArguments();

            var loggingLevelSwitch = switches.Contains(Constants.VERBOSE_SWITCH)
                ? new Serilog.Core.LoggingLevelSwitch(Serilog.Events.LogEventLevel.Verbose)
                : new Serilog.Core.LoggingLevelSwitch(Serilog.Events.LogEventLevel.Warning);

            var logFormatter = new Serilog.Formatting.Json.JsonFormatter();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(logFormatter)
                .MinimumLevel.ControlledBy(loggingLevelSwitch)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddUnpublishedPagesGenerator()
                .AddPresentationsLinksGenerator()
                .AddSourceRepository(sourceConnection)
                .AddTargetRepository(targetConnection)
                .AddTemplateRepository(templateConnection)
                .AddServices()
                .AddLogging(l => l.AddSerilog(Log.Logger))
                .BuildServiceProvider();

            // Generate the website pages
            var outputRepo = serviceProvider.GetRequiredService<IOutputRepository>();
            var siteBuilder = serviceProvider.GetRequiredService<ISiteBuilder>();
            var sitePages = siteBuilder.Build();

            if (!switches.Contains(Constants.WINDOWSLINEENDINGS_SWITCH))
                sitePages = sitePages.ConvertLineEndingsToUnix();

            if (switches.Contains(Constants.VALIDATEONLY_SWITCH))
            {
                var contentRepo = serviceProvider.GetRequiredService<IContentRepository>();
                var siteSettings = contentRepo.GetSiteSettings();

                if (sitePages is null || !sitePages.Any())
                    System.Console.WriteLine($"Unable to generate {siteSettings.Title}. No results returned.");
                else 
                {
                    System.Console.WriteLine($"{siteSettings.Title} generated successfully.");
                    System.Console.WriteLine($"Output WOULD HAVE gone to: {outputRepo.OutputLocation}");
                    var templateTypes = sitePages.Select(p => p.SourceTemplateType).Distinct();
                    foreach (var templateType in templateTypes)
                    {
                        var templatePages = sitePages.Where(p => p.SourceTemplateType == templateType);
                        System.Console.WriteLine($"\r\n{templateType} Pages: {templatePages.Count()}");

                        if (switches.Contains(Constants.VERBOSE_SWITCH))
                        {
                            foreach (var page in templatePages)
                                System.Console.WriteLine($"\t{page.RelativeFilePath}");
                        }
                    }
                }
            }
            else
            {
                // Store the resulting output
                outputRepo.Save(sitePages);
            }
        }
        else
        {
            System.Console.WriteLine(_invalidArgumentsText);
            foreach (var argumentError in argumentErrors)
                System.Console.WriteLine($"\t{argumentError}");
            System.Console.WriteLine();

            throw new ArgumentException("One or more arguments are invalid");
        }
    }

}
