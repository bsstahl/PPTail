using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;
using System.IO;

namespace PPTail.Output.FileSystem
{
    public class Repository : IOutputRepository
    {
        const String _connectionStringFilepathKey = "FilePath";

        readonly IServiceProvider _serviceProvider;
        readonly IFile _file;
        readonly IDirectory _directory;

        readonly String _outputPath;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be fixed in Globalization effort")]
        public Repository(IServiceProvider serviceProvider, String targetConnection)
        {
            _serviceProvider = serviceProvider;
            if (_serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider.ValidateService<IFile>();
            _serviceProvider.ValidateService<IDirectory>();

            _file = serviceProvider.GetService<IFile>();
            _directory = serviceProvider.GetService<IDirectory>();

            if (string.IsNullOrWhiteSpace(targetConnection))
                throw new ArgumentNullException(targetConnection);

            _outputPath = targetConnection.GetConnectionStringValue(_connectionStringFilepathKey);
            if (String.IsNullOrWhiteSpace(_outputPath))
                throw new ArgumentException($"No FilePath supplied in Target Connection", nameof(targetConnection));
        }

        public void Save(IEnumerable<SiteFile> files)
        {
            var filesToSave = files ?? [];
            foreach (var sitePage in filesToSave)
            {
                String fullPath = Path.GetFullPath(Path.Combine(_outputPath, sitePage.RelativeFilePath));
                String folderPath = Path.GetDirectoryName(fullPath);

                if (!_directory.Exists(folderPath))
                    _directory.CreateDirectory(folderPath);

                if (sitePage.IsBase64Encoded)
                {
                    try
                    {
                        _file.WriteAllBytes(fullPath, Convert.FromBase64String(sitePage.Content));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //TODO: Log the fact that this file was skipped
                    }
                }
                else
                    _file.WriteAllText(fullPath, sitePage.Content);
            }

            Console.WriteLine($"{filesToSave.Count()} files written to {System.IO.Path.GetFullPath(_outputPath)}");
        }
    }
}
