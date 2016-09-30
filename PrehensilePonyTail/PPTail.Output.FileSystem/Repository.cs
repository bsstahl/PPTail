using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Output.FileSystem
{
    public class Repository: IOutputRepository
    {
        IServiceProvider _serviceProvider;
        IFile _file;
        IDirectory _directory;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _file = serviceProvider.GetService<IFile>();
            _directory = serviceProvider.GetService<IDirectory>();

            //TODO: Verify that serviceProvider is provided
            //TODO: Verify that the IFile provider is provided
            //TODO: Verify that the IDirectory provider is provided
            //TODO: Verify that the outputpath extended setting is provided
        }

        public void Save(IEnumerable<SiteFile> files)
        {
            var settings = _serviceProvider.GetService<Settings>();
            string outputPath = settings.ExtendedSettings.Single(t => t.Item1 == "outputPath").Item2;

            foreach (var sitePage in files)
            {
                string fullPath = System.IO.Path.Combine(outputPath, sitePage.RelativeFilePath);
                string folderPath = System.IO.Path.GetDirectoryName(fullPath);

                if (!_directory.Exists(folderPath))
                    _directory.CreateDirectory(folderPath);

                _file.WriteAllText(fullPath, sitePage.Content);
            }
        }
    }
}
