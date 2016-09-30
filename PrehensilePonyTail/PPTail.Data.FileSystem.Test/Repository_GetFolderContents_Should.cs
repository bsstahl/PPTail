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
            int expected = 25.GetRandom(10);
            string relativePath = string.Empty.GetRandom();
            string rootPath = "c:\\";
            string folderPath = System.IO.Path.Combine(rootPath, relativePath);

            var files = (null as IEnumerable<SourceFile>).Create(relativePath, expected);

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
    }
}
