using Microsoft.Extensions.DependencyInjection;
using System;
using PPTail.Extensions;
using PPTail.Entities;
using PPTail.Interfaces;
using System.Linq;
using System.Text;

namespace PPTReport
{
    class Program
    {
        static void Main(string[] args)
        {
            const String _connectionStringProviderKey = "Provider";
            const String _connectionStringFilepathKey = "Filepath";

            (var argsAreValid, var argumentErrors) = args.ValidateArguments();

            if (argsAreValid)
            {
                var (sourceConnection, targetConnection) = args.ParseArguments();

                var container = new ServiceCollection();

                String inputFilePath = sourceConnection.GetConnectionStringValue(_connectionStringFilepathKey);

                // Add settings needed by some input repositories
                var settings = new Settings()
                {
                    SourceConnection = sourceConnection
                };
                _ = container.AddSingleton<ISettings>(settings);

                // Add file system abstractions
                _ = container.AddSingleton<IFile>(c => new PPTail.Io.File());
                _ = container.AddSingleton<IDirectory>(c => new PPTail.Io.Directory());

                // Add all possible source repositories to the container
                container.AddSingleton<IContentRepository, PPTail.Data.FileSystem.Repository>();
                container.AddSingleton<IContentRepository, PPTail.Data.Ef.Repository>();
                container.AddSingleton<IContentRepository, PPTail.Data.Forestry.Repository>();
                container.AddSingleton<IContentRepository>(c => new PPTail.Data.WordpressFiles.Repository(inputFilePath));

                var serviceProvider = container.BuildServiceProvider();

                String readRepoName = sourceConnection.GetConnectionStringValue(_connectionStringProviderKey);
                var readRepo = serviceProvider.GetNamedService<IContentRepository>(readRepoName);

                var posts = readRepo.GetAllPosts();
                var pages = readRepo.GetAllPages();
                var widgets = readRepo.GetAllWidgets();
                var categories = readRepo.GetCategories();
                var siteSettings = readRepo.GetSiteSettings();

                // TODO: Run the site generator and add stats of those results to the report

                var report = new StringBuilder();

                var allTags = posts.GetAllTags();
                var tagCounts = allTags.GetTagCounts();
                var singleUseTags = tagCounts
                    .Where(tc => tc.Item2 == 1)
                    .OrderBy(tc => tc.Item1)
                    .Select(t => t.Item1);

                report.AddHeader("Single Use Tags");
                report.AppendLine(String.Join("\r\n", singleUseTags));

                string outputFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PPTReport.txt");
                System.IO.File.WriteAllText(outputFilePath, report.ToString());
                Console.WriteLine($"Report written to {outputFilePath}");
            }
            else
            {
                Console.WriteLine("Invalid Arguments:");
                foreach (var argumentError in argumentErrors)
                    Console.WriteLine($"\t{argumentError}");
                Console.WriteLine();
            }
        }
    }
}
