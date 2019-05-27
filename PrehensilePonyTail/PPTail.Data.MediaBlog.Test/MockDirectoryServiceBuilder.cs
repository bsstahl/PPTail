using Moq;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    public class MockDirectoryServiceBuilder
    {
        readonly List<string> _postFiles = new List<string>();
        readonly List<SourceFile> _sourceFiles = new List<SourceFile>();

        public Mock<IDirectory> Build(string rootPath)
        {
            var directoryService = new Mock<IDirectory>();

            // The path to the post files should show
            // as Exists in the Directory service
            string postPath = Path.Combine(rootPath, "posts");
            directoryService
                .Setup(f => f.Exists(postPath))
                .Returns(true)
                .Verifiable();

            // The list of post files should be returned
            // when that path is enumerated
            directoryService
                .Setup(f => f.EnumerateFiles(postPath))
                .Returns(_postFiles)
                .Verifiable();

            // Get the list of all paths of source files
            var sourceFilesPaths = _sourceFiles
                .Select(f => Path.Combine(rootPath, f.RelativePath))
                .Distinct();

            foreach (var sourceFilesPath in sourceFilesPaths)
            {
                // The path to each of the Source Files
                // should show as Exists in the Directory service
                directoryService
                    .Setup(f => f.Exists(sourceFilesPath))
                    .Returns(true)
                    .Verifiable();

                // The list of source files should be returned
                // when that path is enumerated
                var files = _sourceFiles.Where(f => Path.Combine(rootPath, f.RelativePath) == sourceFilesPath);
                directoryService
                    .Setup(f => f.EnumerateFiles(sourceFilesPath))
                    .Returns(files.Select(f => Path.Combine(f.RelativePath, f.FileName)))
                    .Verifiable();
            }


            return directoryService;
        }

        public MockDirectoryServiceBuilder AddPostFile(string fileName)
        {
            _postFiles.Add(fileName);
            return this;
        }

        public MockDirectoryServiceBuilder AddPostFiles(IEnumerable<string> filenames)
        {
            foreach (var filename in filenames)
            {
                this.AddPostFile(filename);
            }
            return this;
        }

        public MockDirectoryServiceBuilder AddSourceFile(SourceFile sourceFiles)
        {
            _sourceFiles.Add(sourceFiles);
            return this;
        }

        public MockDirectoryServiceBuilder AddSourceFiles(IEnumerable<SourceFile> sourceFiles)
        {
            foreach (var sourceFile in sourceFiles)
            {
                this.AddSourceFile(sourceFile);
            }

            return this;
        }
    }
}
