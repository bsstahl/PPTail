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
    public class Repository_Save_Should
    {
        [Fact]
        public void CallWriteAllTextOnTheFileSystemOnceForEachFile()
        {
            int expected = 25.GetRandom(10);

            var settings = (null as Settings).Create();
            var file = new Mock<IFile>();
            var files = (null as IEnumerable<SiteFile>).Create(expected);

            var target = (null as IOutputRepository).Create(file.Object, settings);
            target.Save(files);

            file.Verify(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(files.Count()));
        }

        [Fact]
        public void SkipAFileThatRequiresAuthorizationToWrite()
        {
            var settings = (null as Settings).Create();
            var fileSystem = new Mock<IFile>();
            var file = (null as SiteFile).Create(true);
            var files = new List<SiteFile>() { file };

            fileSystem.Setup(f => f.WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Throws(new System.UnauthorizedAccessException());

            var target = (null as IOutputRepository).Create(fileSystem.Object, settings);
            target.Save(files);
        }

        [Fact]
        public void PassTheCorrectPathsToTheWriteMethod()
        {
            var outputPath = $"\\{string.Empty.GetRandom()}\\{string.Empty.GetRandom()}";
            var settings = (null as Settings).Create(outputPath);

            var file = new Mock<IFile>();
            var target = (null as IOutputRepository).Create(file.Object, settings);

            var files = (null as IEnumerable<SiteFile>).Create();
            target.Save(files);

            foreach (var siteFile in files)
            {
                string expected = System.IO.Path.Combine(outputPath, siteFile.RelativeFilePath);
                file.Verify(f => f.WriteAllText(expected, It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void PassTheCorrectDataToTheFileSystem()
        {
            var settings = (null as Settings).Create();

            var file = new Mock<IFile>();
            var target = (null as IOutputRepository).Create(file.Object, settings);

            var files = (null as IEnumerable<SiteFile>).Create();
            target.Save(files);

            foreach (var siteFile in files)
                file.Verify(f => f.WriteAllText(It.IsAny<string>(), siteFile.Content), Times.Once);
        }

        [Fact]
        public void PassTheDecodedDataToTheFileSystemIfTheFileIsEncoded()
        {
            var settings = (null as Settings).Create();

            var file = new Mock<IFile>();
            var target = (null as IOutputRepository).Create(file.Object, settings);

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
            var settings = (null as Settings).Create(outputPath);

            var file = Mock.Of<IFile>();
            var directory = new Mock<IDirectory>();

            var target = (null as IOutputRepository).Create(file, directory.Object, settings);
            var files = (null as IEnumerable<SiteFile>).Create(1);

            var siteFile = files.Single();
            string folderPath = System.IO.Path.GetDirectoryName(System.IO.Path.Combine(outputPath, siteFile.RelativeFilePath));

            directory.Setup(d => d.Exists(folderPath)).Returns(false);

            target.Save(files);

            directory.Verify(d => d.CreateDirectory(folderPath), Times.Once);
        }

        [Fact]
        public void NotCreateTheDirectoryIfItAlreadyExists()
        {
            var outputPath = $"\\{string.Empty.GetRandom()}\\{string.Empty.GetRandom()}";
            var settings = (null as Settings).Create(outputPath);

            var file = Mock.Of<IFile>();
            var directory = new Mock<IDirectory>();

            var target = (null as IOutputRepository).Create(file, directory.Object, settings);
            var files = (null as IEnumerable<SiteFile>).Create(1);

            var siteFile = files.Single();
            string folderPath = System.IO.Path.GetDirectoryName(System.IO.Path.Combine(outputPath, siteFile.RelativeFilePath));

            directory.Setup(d => d.Exists(folderPath)).Returns(true);

            target.Save(files);

            directory.Verify(d => d.CreateDirectory(folderPath), Times.Never);
        }
    }
}
