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

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_GetFolderContents_Should
    {

        [Fact]
        public void RequestTheContentsOfTheCorrectFolderPath()
        {
            int count = 25.GetRandom(10);
            string relativePath = string.Empty.GetRandom();
            string rootPath = "c:\\";
            string folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();

            var fileProvider = Mock.Of<IFile>();
            var target = (null as IContentRepository).Create(fileProvider, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            directoryProvider.Verify(fs => fs.EnumerateFiles(folderPath), Times.Once);
        }

        [Fact]
        public void ReturnOneEntityForEachItemInTheFolder()
        {
            int expected = 25.GetRandom(10);
            string relativePath = string.Empty.GetRandom();
            string rootPath = "c:\\";
            string folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, expected);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(files.Select(f => f.FileName));

            var fileProvider = new Mock<IFile>();
            foreach (var file in files)
            {
                string fullPath = System.IO.Path.Combine(folderPath, file.FileName);
                fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Returns(file.Contents);
            }

            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void RequestTheContentsOfTheCorrectFiles()
        {
            int count = 25.GetRandom(10);
            string relativePath = string.Empty.GetRandom();
            string rootPath = "c:\\";
            string folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(dp => dp.EnumerateFiles(It.IsAny<string>()))
                .Returns(files.Select(f => f.FileName));

            var fileProvider = new Mock<IFile>();
            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            foreach (var file in files)
            {
                string filePath = System.IO.Path.Combine(rootPath, relativePath, file.FileName);
                fileProvider.Verify(fs => fs.ReadAllBytes(filePath), Times.Once);
            }
        }

        [Fact]
        public void ReturnTheCorrectRelativePathForEachItem()
        {
            int count = 25.GetRandom(10);
            string relativePath = string.Empty.GetRandom();
            string rootPath = "c:\\";
            string folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(files.Select(f => f.FileName));

            var fileProvider = new Mock<IFile>();
            foreach (var file in files)
            {
                string fullPath = System.IO.Path.Combine(folderPath, file.FileName);
                fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Returns(file.Contents);
            }

            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            Assert.Equal(count, actual.Count(f => f.RelativePath == relativePath));
        }

        [Fact]
        public void ReturnTheCorrectFileNameForEachItem()
        {
            int count = 25.GetRandom(10);
            string relativePath = string.Empty.GetRandom();
            string rootPath = "c:\\";
            string folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(files.Select(f => f.FileName));

            var fileProvider = new Mock<IFile>();
            foreach (var file in files)
            {
                string fullPath = System.IO.Path.Combine(folderPath, file.FileName);
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
            int count = 25.GetRandom(10);
            string relativePath = string.Empty.GetRandom();
            string rootPath = "c:\\";
            string folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, count);

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(fs => fs.EnumerateFiles(folderPath)).Returns(files.Select(f => f.FileName));

            var fileProvider = new Mock<IFile>();
            foreach (var file in files)
            {
                string fullPath = System.IO.Path.Combine(folderPath, file.FileName);
                fileProvider.Setup(fp => fp.ReadAllBytes(fullPath)).Returns(file.Contents);
            }

            var target = (null as IContentRepository).Create(fileProvider.Object, directoryProvider.Object, rootPath);
            var actual = target.GetFolderContents(relativePath);

            foreach (var file in files)
                Assert.Equal(file.Contents, actual.Single(a => a.FileName == file.FileName).Contents);
        }


    }
}
