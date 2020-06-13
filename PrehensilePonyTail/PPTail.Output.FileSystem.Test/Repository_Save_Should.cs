using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;

namespace PPTail.Output.FileSystem.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_Save_Should
    {
        const string _connectionStringFormat = "Provider={0};FilePath={1}";
        const string _defaultProviderName = "PPTail.Output.FileSystem.Repository";

        [Fact]
        public void CallWriteAllTextOnTheFileSystemOnceForEachFile()
        {
            Int32 expected = 25.GetRandom(10);

            var file = new Mock<IFile>();
            var files = (null as IEnumerable<SiteFile>).Create(expected);

            string outputPath = string.Empty.GetRandom();
            var connectionString = String.Format(_connectionStringFormat, _defaultProviderName, outputPath);

            var target = (null as IOutputRepository).Create(file.Object, connectionString);
            target.Save(files);

            file.Verify(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(files.Count()));
        }

        [Fact]
        public void SkipAFileThatRequiresAuthorizationToWrite()
        {
            var fileSystem = new Mock<IFile>();
            var file = (null as SiteFile).Create(true);
            var files = new List<SiteFile>() { file };

            fileSystem.Setup(f => f.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Throws(new System.UnauthorizedAccessException());

            string outputPath = string.Empty.GetRandom();
            var connectionString = String.Format(_connectionStringFormat, _defaultProviderName, outputPath);

            var target = (null as IOutputRepository).Create(fileSystem.Object, connectionString);
            target.Save(files);
        }

        [Fact]
        public void PassTheCorrectPathsToTheWriteMethod()
        {
            var outputPath = $"\\{string.Empty.GetRandom()}\\{string.Empty.GetRandom()}";
            var connectionString = String.Format(_connectionStringFormat, _defaultProviderName, outputPath);

            var file = new Mock<IFile>();
            var target = (null as IOutputRepository).Create(file.Object, connectionString);

            var files = (null as IEnumerable<SiteFile>).Create();
            target.Save(files);

            foreach (var siteFile in files)
            {
                String expected = System.IO.Path.Combine(outputPath, siteFile.RelativeFilePath);
                file.Verify(f => f.WriteAllText(expected, It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void PassTheCorrectDataToTheFileSystem()
        {
            var file = new Mock<IFile>();

            string outputPath = string.Empty.GetRandom();
            var connectionString = String.Format(_connectionStringFormat, _defaultProviderName, outputPath);

            var target = (null as IOutputRepository).Create(file.Object, connectionString);

            var files = (null as IEnumerable<SiteFile>).Create();
            target.Save(files);

            foreach (var siteFile in files)
                file.Verify(f => f.WriteAllText(It.IsAny<string>(), siteFile.Content), Times.Once);
        }

        [Fact]
        public void PassTheDecodedDataToTheFileSystemIfTheFileIsEncoded()
        {
            var file = new Mock<IFile>();

            string outputPath = string.Empty.GetRandom();
            var connectionString = String.Format(_connectionStringFormat, _defaultProviderName, outputPath);

            var target = (null as IOutputRepository).Create(file.Object, connectionString);

            var files = (null as IEnumerable<SiteFile>).Create(10.GetRandom(3), true);
            target.Save(files);

            foreach (var siteFile in files)
            {
                byte[] content = Convert.FromBase64String(siteFile.Content);
                file.Verify(f => f.WriteAllBytes(It.IsAny<string>(), content), Times.Once);
            }
        }

        [Fact]
        public void CreateTheDirectoryIfItDoesntExist()
        {
            var outputPath = $"\\{string.Empty.GetRandom()}\\{string.Empty.GetRandom()}";
            var connectionString = String.Format(_connectionStringFormat, _defaultProviderName, outputPath);

            var file = Mock.Of<IFile>();
            var directory = new Mock<IDirectory>();

            var target = (null as IOutputRepository).Create(file, directory.Object, connectionString);
            var files = (null as IEnumerable<SiteFile>).Create(1);

            var siteFile = files.Single();
            String folderPath = System.IO.Path.GetDirectoryName(System.IO.Path.Combine(outputPath, siteFile.RelativeFilePath));

            directory.Setup(d => d.Exists(folderPath)).Returns(false);

            target.Save(files);

            directory.Verify(d => d.CreateDirectory(folderPath), Times.Once);
        }

        [Fact]
        public void NotCreateTheDirectoryIfItAlreadyExists()
        {
            var outputPath = $"\\{string.Empty.GetRandom()}\\{string.Empty.GetRandom()}";
            var connectionString = String.Format(_connectionStringFormat, _defaultProviderName, outputPath);

            var file = Mock.Of<IFile>();
            var directory = new Mock<IDirectory>();

            var target = (null as IOutputRepository).Create(file, directory.Object, connectionString);
            var files = (null as IEnumerable<SiteFile>).Create(1);

            var siteFile = files.Single();
            String folderPath = System.IO.Path.GetDirectoryName(System.IO.Path.Combine(outputPath, siteFile.RelativeFilePath));

            directory.Setup(d => d.Exists(folderPath)).Returns(true);

            target.Save(files);

            directory.Verify(d => d.CreateDirectory(folderPath), Times.Never);
        }
    }
}
