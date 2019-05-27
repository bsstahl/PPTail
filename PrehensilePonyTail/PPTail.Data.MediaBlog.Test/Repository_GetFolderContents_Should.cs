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
using PPTail.Common.Builders;
using System.IO;

namespace PPTail.Data.MediaBlog.Test
{
    public class Repository_GetFolderContents_Should
    {

        [Fact]
        public void RequestTheContentsOfTheCorrectFolderPath()
        {
            int count = 25.GetRandom(10);
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string relativePath = string.Empty.GetRandom();
            string folderPath = Path.Combine(rootPath, relativePath);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                        .AddFilePath(rootPath)
                        .Build())
                .Build();

            var files = new SourceFileCollectionBuilder()
                .AddRandomFiles(count, relativePath)
                .Build();

            var fileService = new MockFileServiceBuilder()
                .AddSourceFiles(files)
                .Build();

            var directoryService = new MockDirectoryServiceBuilder()
                .AddSourceFiles(files)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileService.Object)
                .AddDirectoryService(directoryService.Object)
                .Build();

            var actual = target.GetFolderContents(relativePath);

            directoryService.Verify(fs => fs.EnumerateFiles(folderPath), Times.Once);
        }

        [Fact]
        public void ReturnOneEntityForEachItemInTheFolder()
        {
            int count = 25.GetRandom(10);
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string relativePath = string.Empty.GetRandom();
            string folderPath = Path.Combine(rootPath, relativePath);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                        .AddFilePath(rootPath)
                        .Build())
                .Build();

            var files = new SourceFileCollectionBuilder()
                .AddRandomFiles(count, relativePath)
                .Build();

            var fileService = new MockFileServiceBuilder()
                .AddSourceFiles(files)
                .Build();

            var directoryService = new MockDirectoryServiceBuilder()
                .AddSourceFiles(files)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileService.Object)
                .AddDirectoryService(directoryService.Object)
                .Build();

            var actual = target.GetFolderContents(relativePath);

            Assert.Equal(count, actual.Count());
        }

        [Fact]
        public void RequestTheContentsOfTheCorrectFiles()
        {
            int count = 25.GetRandom(10);
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string relativePath = string.Empty.GetRandom();
            string folderPath = Path.Combine(rootPath, relativePath);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                        .AddFilePath(rootPath)
                        .Build())
                .Build();

            var files = new SourceFileCollectionBuilder()
                .AddRandomFiles(count, relativePath)
                .Build();

            var fileService = new MockFileServiceBuilder()
                .AddSourceFiles(files)
                .Build();

            var directoryService = new MockDirectoryServiceBuilder()
                .AddSourceFiles(files)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileService.Object)
                .AddDirectoryService(directoryService.Object)
                .Build();

            var actual = target.GetFolderContents(relativePath);

            foreach (var file in files)
            {
                string filePath = System.IO.Path.Combine(relativePath, file.FileName);
                fileService.Verify(fs => fs.ReadAllBytes(filePath), Times.Once);
            }
        }

        [Fact]
        public void ReturnTheCorrectRelativePathForEachItem()
        {
            int count = 25.GetRandom(10);
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string relativePath = string.Empty.GetRandom();
            string folderPath = Path.Combine(rootPath, relativePath);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                        .AddFilePath(rootPath)
                        .Build())
                .Build();

            var files = new SourceFileCollectionBuilder()
                .AddRandomFiles(count, relativePath)
                .Build();

            var fileService = new MockFileServiceBuilder()
                .AddSourceFiles(files)
                .Build();

            var directoryService = new MockDirectoryServiceBuilder()
                .AddSourceFiles(files)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileService.Object)
                .AddDirectoryService(directoryService.Object)
                .Build();

            var actual = target.GetFolderContents(relativePath);

            Assert.Equal(count, actual.Count(f => f.RelativePath == relativePath));
        }

        [Fact]
        public void ReturnTheCorrectFileNameForEachItem()
        {
            int count = 25.GetRandom(10);
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string relativePath = string.Empty.GetRandom();
            string folderPath = Path.Combine(rootPath, relativePath);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                        .AddFilePath(rootPath)
                        .Build())
                .Build();

            var files = new SourceFileCollectionBuilder()
                .AddRandomFiles(count, relativePath)
                .Build();

            var fileService = new MockFileServiceBuilder()
                .AddSourceFiles(files)
                .Build();

            var directoryService = new MockDirectoryServiceBuilder()
                .AddSourceFiles(files)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileService.Object)
                .AddDirectoryService(directoryService.Object)
                .Build();

            var actual = target.GetFolderContents(relativePath);

            foreach (var file in files)
                Assert.Equal(1, actual.Count(a => a.FileName == file.FileName));
        }

        [Fact]
        public void ReturnTheCorrectContentsForEachItem()
        {
            int count = 25.GetRandom(10);
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string relativePath = string.Empty.GetRandom();
            string folderPath = Path.Combine(rootPath, relativePath);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                        .AddFilePath(rootPath)
                        .Build())
                .Build();

            var files = new SourceFileCollectionBuilder()
                .AddRandomFiles(count, relativePath)
                .Build();

            var fileService = new MockFileServiceBuilder()
                .AddSourceFiles(files)
                .Build();

            var directoryService = new MockDirectoryServiceBuilder()
                .AddSourceFiles(files)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileService.Object)
                .AddDirectoryService(directoryService.Object)
                .Build();

            var actual = target.GetFolderContents(relativePath);

            foreach (var file in files)
                Assert.Equal(file.Contents, actual.Single(a => a.FileName == file.FileName).Contents);
        }

        [Fact]
        public void ReturnAnEmptyCollectionIfTheFolderDoesNotExist()
        {
            int count = 25.GetRandom(10);
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string relativePath = string.Empty.GetRandom();
            string folderPath = Path.Combine(rootPath, relativePath);

            string fakeFolder = String.Empty.GetRandom();
            string fakePath = Path.Combine(rootPath, fakeFolder);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                        .AddFilePath(rootPath)
                        .Build())
                .Build();

            var files = new SourceFileCollectionBuilder()
                .AddRandomFiles(count, relativePath)
                .Build();

            var fileService = new MockFileServiceBuilder()
                .AddSourceFiles(files)
                .Build();

            var directoryService = new MockDirectoryServiceBuilder()
                .AddSourceFiles(files)
                .Build(rootPath);

            directoryService.Setup(fs => fs.Exists(fakePath)).Returns(false);
            directoryService.Setup(fs => fs.EnumerateFiles(fakeFolder))
                .Throws(new System.IO.DirectoryNotFoundException());

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileService.Object)
                .AddDirectoryService(directoryService.Object)
                .Build();

            var actual = target.GetFolderContents(fakePath);

            Assert.Empty(actual);
        }

        [Fact]
        public void SkipItemsThatRequireAuthorizationToAccess()
        {
            int count = 35.GetRandom(20);
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string relativePath = string.Empty.GetRandom();
            string folderPath = Path.Combine(rootPath, relativePath);

            string fakeFolder = String.Empty.GetRandom();
            string fakePath = Path.Combine(rootPath, fakeFolder);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                        .AddFilePath(rootPath)
                        .Build())
                .Build();

            var files = new SourceFileCollectionBuilder()
                .AddRandomFiles(count, relativePath)
                .Build();

            var directoryService = new MockDirectoryServiceBuilder()
                .AddSourceFiles(files)
                .Build(rootPath);

            var fileServiceBuilder = new MockFileServiceBuilder();

            int expected = 0;
            foreach (var file in files)
            {
                bool secured = true.GetRandom();
                fileServiceBuilder.AddSecuredSourceFile(file, secured);
                if (!secured) expected++;
            }

            var fileService = fileServiceBuilder.Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileService.Object)
                .AddDirectoryService(directoryService.Object)
                .Build();

            var actual = target.GetFolderContents(relativePath);

            Assert.Equal(expected, actual.Count());
        }

    }
}
