using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using Moq;
using System.Linq.Expressions;
using PPTail.Entities;
using PPTail.Builders;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_GetAllPages_Should
    {
        const String _defaultConnection = "Provider=this;FilePath=c:\\";
        const String _connectionStringFilepathKey = "FilePath";

        [Fact]
        public void ReturnAllPagesIfAllAreValid()
        {
            var files = new List<string>
            {
                "28C65CCD-D504-44D3-A54B-9E3DBB163D43.json",
                "8EE89C80-760E-4980-B980-5A4B70A563E2.json",
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.json"
            };

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            var fileSystem = new Mock<IFile>();
            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("{}");

            var target = new ContentRepositoryBuilder()
                .AddDirectoryService(directoryProvider.Object)
                .AddFileService(fileSystem.Object)
                .Build(_defaultConnection);

            var pages = target.GetAllPages();

            Assert.Equal(files.Count(), pages.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutJsonExtension()
        {
            var files = new List<string>
            {
                "82B52DBC-9D33-4C9E-A933-AF515E4FF140",
                "28C65CCD-D504-44D3-A54B-9E3DBB163D43.json",
                "0F716B73-9A2F-46D9-A576-3CA03EB10327.ppt",
                "8EE89C80-760E-4980-B980-5A4B70A563E2.json",
                "39836B5E-C330-4670-9897-1CBF0851AB5B.txt",
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.json",
                "86F29FA4-29CD-4292-8000-CEAFEA7A2315.com"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("{}");

            var target = new ContentRepositoryBuilder()
                .AddDirectoryService(directoryProvider.Object)
                .AddFileService(fileSystem.Object)
                .Build(_defaultConnection);

            var pages = target.GetAllPages();

            Assert.Equal(3, pages.Count());
        }

        [Fact]
        public void RequestFilesFromThePagesFolder()
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.json"
            };

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            String expectedPath = System.IO.Path.Combine(rootPath, "pages");

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(expectedPath))
                .Returns(files).Verifiable();

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("{}");

            var target = new ContentRepositoryBuilder()
                .AddDirectoryService(directoryProvider.Object)
                .AddFileService(fileSystem.Object)
                .Build($"Provider=this;{_connectionStringFilepathKey}={rootPath}");

            var pages = target.GetAllPages();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void ReturnTheProperValueInTheAuthorField()
        {
            String fieldName = "author";
            String fieldValueDelegate(ContentItem c) => c.Author;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            String fieldName = "title";
            String fieldValueDelegate(ContentItem c) => c.Title;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheDescriptionField()
        {
            String fieldName = "description";
            String fieldValueDelegate(ContentItem c) => c.Description;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentField()
        {
            String fieldName = "content";
            String fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTrueIfThePageIsPublished()
        {
            String fieldName = "IsPublished";
            String fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();

            bool expectedValue = true;
            String expected = expectedValue.ToString();
            String json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnFalseIfThePageIsNotPublished()
        {
            String fieldName = "IsPublished";
            String fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();

            bool expectedValue = false;
            String expected = expectedValue.ToString();
            String json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTrueIfThePageIsMarkedShowInList()
        {
            String fieldName = "ShowInList";
            String fieldValueDelegate(ContentItem c) => c.ShowInList.ToString();

            bool expectedValue = true;
            String expected = expectedValue.ToString();
            String json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnFalseIfThePageIsNotMarkedShowInList()
        {
            String fieldName = "showinlist";
            String fieldValueDelegate(ContentItem c) => c.ShowInList.ToString();

            bool expectedValue = false;
            String expected = expectedValue.ToString();
            String json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInThePubDateField()
        {
            String fieldName = "PublicationDate";
            String fieldValueDelegate(ContentItem c) => c.PublicationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            String expected = expectedValue.ToString();
            String json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModifiedDateField()
        {
            String fieldName = "LastModificationDate";
            String fieldValueDelegate(ContentItem c) => c.LastModificationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            String expected = expectedValue.ToString();
            String json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheSlugField()
        {
            String fieldName = "slug";
            String fieldValueDelegate(ContentItem c) => c.Slug;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheByLineField()
        {
            String author = string.Empty.GetRandom();
            String expected = $"by {author}";
            String json = $"{{\"Author\" : \"{author}\"}}";
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        private static void ExecutePropertyTest(String fieldName, Func<ContentItem, string> fieldValueDelegate)
        {
            String expected = string.Empty.GetRandom();
            String json = $"{{\"{fieldName}\" : \"{expected}\"}}";
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        private static void ExecutePropertyTest(String expected, Func<ContentItem, string> fieldValueDelegate, String json)
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.json"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(json);

            var target = new ContentRepositoryBuilder()
                .AddDirectoryService(directoryProvider.Object)
                .AddFileService(fileSystem.Object)
                .Build(_defaultConnection);

            var pages = target.GetAllPages();
            var actual = pages.ToArray()[0];

            Assert.Equal(expected, fieldValueDelegate(actual));
        }
    }
}
