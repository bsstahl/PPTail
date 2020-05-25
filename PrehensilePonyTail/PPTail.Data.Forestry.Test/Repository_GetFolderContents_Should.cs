using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TestHelperExtensions;
using Xunit;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;

namespace PPTail.Data.Forestry.Test
{
    public class Repository_GetFolderContents_Should
    {

        [Fact]
        public void RequestTheContentsOfTheCorrectFolderPath()
        {
            Int32 count = 25.GetRandom(10);
            String relativePath = string.Empty.GetRandom();
            String rootPath = "c:\\";
            String folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.Exists(folderPath)).Returns(true);

            var fileProvider = Mock.Of<IFile>();
            var target = (null as IContentRepository).Create(fileProvider, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            directoryProvider.Verify(fs => fs.EnumerateFiles(folderPath), Times.Once);
        }

        [Fact]
        public void ReturnOneEntityForEachItemInTheFolder()
        {
            Int32 expected = 25.GetRandom(10);
            String relativePath = string.Empty.GetRandom();
            String rootPath = "c:\\";
            String folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, expected);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(files.Select(f => f.FileName));
            directoryProvider.Setup(fs => fs.Exists(folderPath)).Returns(true);

            var fileProvider = new Mock<IFile>();
            foreach (var file in files)
            {
                String fullPath = System.IO.Path.Combine(folderPath, file.FileName);
                fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Returns(file.Contents);
            }

            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void RequestTheContentsOfTheCorrectFiles()
        {
            Int32 count = 25.GetRandom(10);
            String relativePath = string.Empty.GetRandom();
            String rootPath = "c:\\";
            String folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.Exists(folderPath)).Returns(true);
            directoryProvider.Setup(dp => dp.EnumerateFiles(It.IsAny<string>()))
                .Returns(files.Select(f => System.IO.Path.Combine(rootPath, relativePath, f.FileName)));

            var fileProvider = new Mock<IFile>();
            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            foreach (var file in files)
            {
                String filePath = System.IO.Path.Combine(rootPath, relativePath, file.FileName);
                fileProvider.Verify(fs => fs.ReadAllBytes(filePath), Times.Once);
            }
        }

        [Fact]
        public void ReturnTheCorrectRelativePathForEachItem()
        {
            Int32 count = 25.GetRandom(10);
            String relativePath = string.Empty.GetRandom();
            String rootPath = "c:\\";
            String folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.Exists(folderPath)).Returns(true);
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(files.Select(f => f.FileName));

            var fileProvider = new Mock<IFile>();
            foreach (var file in files)
            {
                String fullPath = System.IO.Path.Combine(folderPath, file.FileName);
                fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Returns(file.Contents);
            }

            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            Assert.Equal(count, actual.Count(f => f.RelativePath == relativePath));
        }

        [Fact]
        public void ReturnTheCorrectFileNameForEachItem()
        {
            Int32 count = 25.GetRandom(10);
            String relativePath = string.Empty.GetRandom();
            String rootPath = "c:\\";
            String folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.Exists(folderPath)).Returns(true);
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(files.Select(f => f.FileName));

            var fileProvider = new Mock<IFile>();
            foreach (var file in files)
            {
                String fullPath = System.IO.Path.Combine(folderPath, file.FileName);
                fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Returns(file.Contents);
            }

            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            foreach (var file in files)
                Assert.Equal(1, actual.Count(a => a.FileName == file.FileName));
        }

        [Fact]
        public void ReturnTheCorrectContentsForEachItem()
        {
            Int32 count = 25.GetRandom(10);
            String relativePath = string.Empty.GetRandom();
            String rootPath = "c:\\";
            String folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.Exists(folderPath)).Returns(true);
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(files.Select(f => System.IO.Path.Combine(folderPath, f.FileName)));

            var fileProvider = new Mock<IFile>();
            foreach (var file in files)
            {
                String fullPath = System.IO.Path.Combine(folderPath, file.FileName);
                fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Returns(file.Contents);
            }

            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            foreach (var file in files)
                Assert.Equal(file.Contents, actual.Single(a => a.FileName == file.FileName).Contents);
        }

        [Fact]
        public void ReturnAnEmptyCollectionIfTheFolderDoesNotExist()
        {
            String relativePath = string.Empty.GetRandom();
            String rootPath = "c:\\";
            String folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.Exists(folderPath)).Returns(false);
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath))
                .Throws(new System.IO.DirectoryNotFoundException());

            var target = (null as IContentRepository).Create(Mock.Of<IFile>(), directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            Assert.Empty(actual);
        }

        [Fact]
        public void SkipItemsThatRequireAuthorizationToAccess()
        {
            Int32 count = 35.GetRandom(20);
            String relativePath = string.Empty.GetRandom();
            String rootPath = "c:\\";
            String folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);
            var fileNames = new List<string>();
            fileNames.AddRange(files.Select(f => System.IO.Path.Combine(folderPath, f.FileName)));

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(fileNames);
            directoryProvider.Setup(fs => fs.Exists(folderPath)).Returns(true);

            var fileProvider = new Mock<IFile>();
            Int32 expected = 0;
            foreach (var file in files)
            {
                String fullPath = System.IO.Path.Combine(folderPath, file.FileName);
                bool bad = true.GetRandom();
                if (bad)
                    fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Throws(new System.UnauthorizedAccessException());
                else
                {
                    expected++;
                    fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Returns(file.Contents);
                }
            }

            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            Assert.Equal(expected, actual.Count());
        }

    }
}
