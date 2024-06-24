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
                .AddSourceRepository(sourceConnection)
                .AddTargetRepository(targetConnection)
                .AddTemplateRepository(templateConnection)
                .AddServices()
                .AddLogging(l => l.AddSerilog(Log.Logger))
                .BuildServiceProvider();

            // Run the pre-generation tasks
            // TOOD: Add tool to generate an Unpublished Posts list page

            // Generate the website pages
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
                    var postPages = sitePages.Where(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage);
                    var contentPages = sitePages.Where(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage);
                    var otherPages = sitePages.Where(p => p.SourceTemplateType != Enumerations.TemplateType.PostPage && p.SourceTemplateType != Enumerations.TemplateType.ContentPage);

                    System.Console.WriteLine($"{siteSettings.Title} generated successfully.");
                    System.Console.WriteLine($"\tPost Pages: {postPages.Count()}");
                    System.Console.WriteLine($"\tContent Pages: {contentPages.Count()}");
                    System.Console.WriteLine($"\tOther Pages: {otherPages.Count()}");

                    if (switches.Contains(Constants.VERBOSE_SWITCH))
                    {
                        System.Console.WriteLine("Post Pages:");
                        foreach (var page in postPages)
                            System.Console.WriteLine($"\t{page.RelativeFilePath}");

                        System.Console.WriteLine("Content Pages:");
                        foreach (var page in contentPages)
                            System.Console.WriteLine($"\t{page.RelativeFilePath}");

                        System.Console.WriteLine("Other Pages:");
                        foreach (var page in otherPages)
                            System.Console.WriteLine($"\t{page.RelativeFilePath} ({page.SourceTemplateType})");
                    }
                }
            }
            else
            {
                // Store the resulting output
                var outputRepo = serviceProvider.GetRequiredService<IOutputRepository>();
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
