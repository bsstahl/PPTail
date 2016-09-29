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
            var settings = (null as Settings).Create();

            var file = new Mock<IFile>();
            var target = (null as IOutputRepository).Create(file.Object, settings);
            int expected = 25.GetRandom(10);

            var files = (null as IEnumerable<SiteFile>).Create(expected);
            target.Save(files);

            file.Verify(f => f.WriteAllTest(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(files.Count()));
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
                file.Verify(f => f.WriteAllTest(expected, It.IsAny<string>()), Times.Once);
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
                file.Verify(f => f.WriteAllTest(It.IsAny<string>(), siteFile.Content), Times.Once);
        }

        [Fact]
        public void CreateTheDirectoryIfItDoesntExist()
        {
            var outputPath = $"\\{string.Empty.GetRandom()}\\{string.Empty.GetRandom()}";
            var settings = (null as Settings).Create(outputPath);

            var file = Mock.Of<IFile>();
            var target = (null as IOutputRepository).Create(file, settings);
            var files = (null as IEnumerable<SiteFile>).Create(1);

            var siteFile = files.Single();
            string fullPath = System.IO.Path.Combine(outputPath, siteFile.RelativeFilePath);

            var directory = new Mock<IDirectory>();
            directory.Setup(d => d.Exists(fullPath)).Returns(false);

            target.Save(files);

            directory.Verify(d => d.CreateDirectory(fullPath), Times.Once);
        }

        [Fact]
        public void NotCreateTheDirectoryIfItAlreadyExists()
        {
            var outputPath = $"\\{string.Empty.GetRandom()}\\{string.Empty.GetRandom()}";
            var settings = (null as Settings).Create(outputPath);

            var file = Mock.Of<IFile>();
            var target = (null as IOutputRepository).Create(file, settings);
            var files = (null as IEnumerable<SiteFile>).Create(1);

            var siteFile = files.Single();
            string fullPath = System.IO.Path.Combine(outputPath, siteFile.RelativeFilePath);

            var directory = new Mock<IDirectory>();
            directory.Setup(d => d.Exists(fullPath)).Returns(true);

            target.Save(files);

            directory.Verify(d => d.CreateDirectory(It.IsAny<string>()), Times.Never);
        }
    }
}
