using System;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using PPTail.Extensions;

namespace PPTConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            const string _connectionStringProviderKey = "Provider";
            const string _connectionStringFilepathKey = "Filepath";


            (var argsAreValid, var argumentErrors) = args.ValidateArguments();

            if (argsAreValid)
            {
                var (sourceConnection, targetConnection) = args.ParseArguments();

                var container = new ServiceCollection();

                // Add all possible source repositories to the container
                string inputFilePath = sourceConnection.GetConnectionStringValue(_connectionStringFilepathKey);

                // TODO: Make FileSystem Repository work
                // container.AddSingleton<IContentRepository, PPTail.Data.FileSystem.Repository>();
                container.AddSingleton<IContentRepository, PPTail.Data.Ef.Repository>();
                container.AddSingleton<IContentRepository>(c => new PPTail.Data.WordpressFiles.Repository(inputFilePath));

                // Add all possible target repositories to the container
                string outputFilePath = targetConnection.GetConnectionStringValue(_connectionStringFilepathKey);
                container.AddSingleton<IContentRepositoryWriter>(c => new PPTail.Data.NativeJson.RepositoryWriter(outputFilePath));

                var serviceProvider = container.BuildServiceProvider();

                string readRepoName = sourceConnection.GetConnectionStringValue(_connectionStringProviderKey);
                var readRepo = serviceProvider.GetNamedService<IContentRepository>(readRepoName);

                string writeRepoName = targetConnection.GetConnectionStringValue(_connectionStringProviderKey);
                var writeRepo = serviceProvider.GetNamedService<IContentRepositoryWriter>(writeRepoName);

                writeRepo.SaveAllPages(readRepo.GetAllPages());
                writeRepo.SaveAllPosts(readRepo.GetAllPosts());
                writeRepo.SaveAllWidgets(readRepo.GetAllWidgets());
                writeRepo.SaveCategories(readRepo.GetCategories());
                writeRepo.SaveSiteSettings(readRepo.GetSiteSettings());
                
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
