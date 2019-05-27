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
using PPTail.Common.Builders;

namespace PPTail.Data.MediaBlog.Test
{
    public class Repository_GetAllPages_Should
    {
        const string _connectionStringFilepathKey = "FilePath";

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
                .UseGenericSettings()
                .AddDirectoryService(directoryProvider.Object)
                .AddFileService(fileSystem.Object)
                .Build();
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
                .UseGenericSettings()
                .AddDirectoryService(directoryProvider.Object)
                .AddFileService(fileSystem.Object)
                .Build();
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

            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string expectedPath = System.IO.Path.Combine(rootPath, "pages");

            var settings = new SettingsBuilder()
                .SourceConnection($"Provider=this;{_connectionStringFilepathKey}={rootPath}")
                .Build();

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(expectedPath))
                .Returns(files).Verifiable();

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("{}");

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddDirectoryService(directoryProvider.Object)
                .AddFileService(fileSystem.Object)
                .Build();
            var pages = target.GetAllPages();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void ReturnTheProperValueInTheAuthorField()
        {
            string fieldName = "author";
            string fieldValueDelegate(ContentItem c) => c.Author;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            string fieldName = "title";
            string fieldValueDelegate(ContentItem c) => c.Title;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheDescriptionField()
        {
            string fieldName = "description";
            string fieldValueDelegate(ContentItem c) => c.Description;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentField()
        {
            string fieldName = "content";
            string fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTrueIfThePageIsPublished()
        {
            string fieldName = "IsPublished";
            string fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();

            bool expectedValue = true;
            string expected = expectedValue.ToString();
            string json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnFalseIfThePageIsNotPublished()
        {
            string fieldName = "IsPublished";
            string fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();

            bool expectedValue = false;
            string expected = expectedValue.ToString();
            string json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTrueIfThePageIsMarkedShowInList()
        {
            string fieldName = "ShowInList";
            string fieldValueDelegate(ContentItem c) => c.ShowInList.ToString();

            bool expectedValue = true;
            string expected = expectedValue.ToString();
            string json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnFalseIfThePageIsNotMarkedShowInList()
        {
            string fieldName = "showinlist";
            string fieldValueDelegate(ContentItem c) => c.ShowInList.ToString();

            bool expectedValue = false;
            string expected = expectedValue.ToString();
            string json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInThePubDateField()
        {
            string fieldName = "PublicationDate";
            string fieldValueDelegate(ContentItem c) => c.PublicationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string expected = expectedValue.ToString();
            string json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModifiedDateField()
        {
            string fieldName = "LastModificationDate";
            string fieldValueDelegate(ContentItem c) => c.LastModificationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string expected = expectedValue.ToString();
            string json = $"{{\"{fieldName}\" : \"{expected}\"}}";

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheSlugField()
        {
            string fieldName = "slug";
            string fieldValueDelegate(ContentItem c) => c.Slug;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheByLineField()
        {
            string author = string.Empty.GetRandom();
            string expected = $"by {author}";
            string json = $"{{\"Author\" : \"{author}\"}}";
            string fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        // TODO: Determine if this actually requires an empty string or if a null string is ok
        //[Fact]
        //public void ReturnAnEmptyStringInTheByLineFieldIfAuthorFieldIsEmpty()
        //{
        //    string expected = string.Empty;
        //    string json = "{}";
        //    string fieldValueDelegate(ContentItem c) => c.ByLine;
        //    ExecutePropertyTest(expected, fieldValueDelegate, json);
        //}

        private static void ExecutePropertyTest(string fieldName, Func<ContentItem, string> fieldValueDelegate)
        {
            string expected = string.Empty.GetRandom();
            string json = $"{{\"{fieldName}\" : \"{expected}\"}}";
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        private static void ExecutePropertyTest(string expected, Func<ContentItem, string> fieldValueDelegate, string json)
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
                .UseGenericSettings()
                .AddDirectoryService(directoryProvider.Object)
                .AddFileService(fileSystem.Object)
                .Build();

            var pages = target.GetAllPages();
            var actual = pages.ToArray()[0];

            Assert.Equal(expected, fieldValueDelegate(actual));
        }
    }
}
