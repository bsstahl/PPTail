using System;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using PPTail.Extensions;
using PPTail.Entities;

namespace PPTConvert
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

                // Add file system abstractions
                _ = container.AddSingleton<IFile>(c => new PPTail.Io.File());
                _ = container.AddSingleton<IDirectory>(c => new PPTail.Io.Directory());

                var readRepo = sourceConnection.GetSourceRepository(container.BuildServiceProvider());
                
                container.AddSingleton<IContentRepository>(readRepo);
                
                var writeRepo = targetConnection.GetTargetRepository(container.BuildServiceProvider());

                var pages = readRepo.GetAllPages();
                var posts = readRepo.GetAllPosts();
                var widgets = readRepo.GetAllWidgets();
                var categories = readRepo.GetCategories();
                var siteSettings = readRepo.GetSiteSettings();

                // writeRepo.SaveAllPages(pages);
                // writeRepo.SaveAllPosts(posts);
                writeRepo.SaveAllWidgets(widgets);
                writeRepo.SaveCategories(categories);
                writeRepo.SaveSiteSettings(siteSettings);

                // TODO: writeRepo.SaveFolderContents(folder, readRepo.GetFolderContents(folder))
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
