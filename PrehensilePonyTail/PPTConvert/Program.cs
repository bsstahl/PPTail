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

                // Add all possible source repositories to the container
                String inputFilePath = sourceConnection.GetConnectionStringValue(_connectionStringFilepathKey);

                // Add file system abstractions
                _ = container.AddSingleton<IFile>(c => new PPTail.Io.File());
                _ = container.AddSingleton<IDirectory>(c => new PPTail.Io.Directory());


                // Provide additional dependencies for the FileSystem Repository to work
                container.AddSingleton<IContentRepository, PPTail.Data.FileSystem.Repository>();
                container.AddSingleton<IContentRepository, PPTail.Data.Ef.Repository>();
                container.AddSingleton<IContentRepository>(c => new PPTail.Data.WordpressFiles.Repository(inputFilePath));

                String readRepoName = sourceConnection.GetConnectionStringValue(_connectionStringProviderKey);
                String writeRepoName = targetConnection.GetConnectionStringValue(_connectionStringProviderKey);

                // Add all possible target repositories to the container
                String outputFilePath = targetConnection.GetConnectionStringValue(_connectionStringFilepathKey);
                container.AddSingleton<IContentRepositoryWriter>(c => new PPTail.Data.NativeJson.RepositoryWriter(outputFilePath));
                container.AddSingleton<IContentRepositoryWriter>(c => new PPTail.Data.Forestry.RepositoryWriter(c, outputFilePath, readRepoName));

                var serviceProvider = container.BuildServiceProvider();

                var readRepo = serviceProvider.GetService<IContentRepository>();
                var writeRepo = serviceProvider.GetService<IContentRepositoryWriter>();

                writeRepo.SaveAllPages(readRepo.GetAllPages());
                writeRepo.SaveAllPosts(readRepo.GetAllPosts());
                //writeRepo.SaveAllWidgets(readRepo.GetAllWidgets());
                //writeRepo.SaveCategories(readRepo.GetCategories());
                //writeRepo.SaveSiteSettings(readRepo.GetSiteSettings());
                
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
