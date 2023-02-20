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
                String inputRepoType = sourceConnection.GetConnectionStringValue(_connectionStringProviderKey);

                // Add file system abstractions
                _ = container.AddSingleton<IFile>(c => new PPTail.Io.File());
                _ = container.AddSingleton<IDirectory>(c => new PPTail.Io.Directory());

                // Add all possible source repositories to the container
                container.AddSingleton<IContentRepository>(c => new PPTail.Data.FileSystem.Repository(c, sourceConnection));
                container.AddSingleton<IContentRepository, PPTail.Data.Ef.Repository>();
                container.AddSingleton<IContentRepository>(c => new PPTail.Data.Forestry.Repository(c, sourceConnection));
                container.AddSingleton<IContentRepository>(c => new PPTail.Data.WordpressFiles.Repository(inputFilePath));
                container.AddSingleton<IContentRepository>(c => new PPTail.Data.MediaBlog.YamlRepository(c, sourceConnection));

                var serviceProvider = container.BuildServiceProvider();

                var readRepos = serviceProvider.GetServices<IContentRepository>();
                var readRepo = readRepos.Single(r => r.GetType().FullName == inputRepoType);

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
                report.AppendLine();

                var postTitles = posts
                    .OrderByDescending(p => p.PublicationDate)
                    .Select(p => p.Title);

                report.AddHeader("Posts");
                report.AppendLine(String.Join("\r\n", postTitles));
                report.AppendLine();

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
