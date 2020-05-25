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

namespace PPTail.Data.Forestry.Test
{
    public class Repository_GetAllPages_Should
    {
        const String _connectionStringFilepathKey = "FilePath";
        const String _dataFolder = "Pages";

        [Fact]
        public void ReturnAllPagesIfAllAreValid()
        {
            var fileNames = new List<string>
            {
                $"{String.Empty.GetRandom()}.md",
                $"{String.Empty.GetRandom()}.md",
                $"{String.Empty.GetRandom()}.md"
            };

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(fileNames);

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<String>()))
                .Returns(new ContentItemFileBuilder().UseRandomValues().Build());

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var pages = target.GetAllPages();

            Assert.Equal(fileNames.Count(), pages.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutMDExtension()
        {
            var files = new List<string>
            {
                "82B52DBC-9D33-4C9E-A933-AF515E4FF140",
                "28C65CCD-D504-44D3-A54B-9E3DBB163D43.md",
                "0F716B73-9A2F-46D9-A576-3CA03EB10327.ppt",
                "8EE89C80-760E-4980-B980-5A4B70A563E2.md",
                "39836B5E-C330-4670-9897-1CBF0851AB5B.txt",
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.md",
                "86F29FA4-29CD-4292-8000-CEAFEA7A2315.com"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(new ContentItemFileBuilder().UseRandomValues().Build());

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var pages = target.GetAllPages();

            Assert.Equal(3, pages.Count());
        }

        [Fact]
        public void RequestFilesFromThePagesFolder()
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.md"
            };

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            String expectedPath = System.IO.Path.Combine(rootPath, _dataFolder);

            var settings = new Settings() { SourceConnection = $"Provider=this;{_connectionStringFilepathKey}={rootPath}" };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(expectedPath))
                .Returns(files)
                .Verifiable();

            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(new ContentItemFileBuilder().UseRandomValues().Build());

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem.Object);
            container.AddSingleton<IDirectory>(directoryProvider.Object);
            container.AddSingleton<ISettings>(settings);

            var target = (null as IContentRepository).Create(container.BuildServiceProvider());
            var pages = target.GetAllPages();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void ReturnTheProperValueInTheIdField()
        {
            Guid fieldValueDelegate(ContentItem c) => c.Id;
            var expected = Guid.NewGuid();
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .Id(expected)
                .Build();
            fileContents.ExecutePagePropertyTest<Guid>(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheAuthorField()
        {
            String fieldValueDelegate(ContentItem c) => c.Author;
            String expected = string.Empty.GetRandom();
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .Author(expected)
                .Build();
            fileContents.ExecutePagePropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            String fieldValueDelegate(ContentItem c) => c.Title;
            String expected = string.Empty.GetRandom();
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .Title(expected)
                .Build();
            fileContents.ExecutePagePropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheDescriptionField()
        {
            String fieldValueDelegate(ContentItem c) => c.Description;
            String expected = string.Empty.GetRandom();
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .Description(expected)
                .Build();
            fileContents.ExecutePagePropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentField()
        {
            String fieldValueDelegate(ContentItem c) => c.Content;
            String expected = string.Empty.GetRandom(500);
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .Content(expected)
                .Build();
            fileContents.ExecutePagePropertyTest<String>(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTrueIfThePageIsPublished()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.IsPublished;
            bool expected = true;
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .IsPublished(expected)
                .Build();
            fileContents.ExecutePagePropertyTest<Boolean>(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnFalseIfThePageIsNotPublished()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.IsPublished;
            bool expected = false;
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .IsPublished(expected)
                .Build();
            fileContents.ExecutePagePropertyTest<Boolean>(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnFalseIfTheIsPublishedFieldIsMissing()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.IsPublished;
            bool expected = false;
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .RemoveIsPublished()
                .Build();
            fileContents.ExecutePagePropertyTest<Boolean>(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTrueIfThePageIsMarkedShowInList()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.ShowInList;
            bool expected = true;
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .ShowInList(expected)
                .Build();
            fileContents.ExecutePagePropertyTest<Boolean>(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnFalseIfThePageIsNotMarkedShowInList()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.ShowInList;
            bool expected = false;
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .ShowInList(expected)
                .Build();
            fileContents.ExecutePagePropertyTest<Boolean>(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInThePublicationDateField()
        {
            DateTime fieldValueDelegate(ContentItem c) => c.PublicationDate.ToSecondPrecision();
            DateTime expected = DateTime.Parse("1/1/2000").AddSeconds(Int32.MaxValue);
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .PublicationDate(expected)
                .Build();
            fileContents.ExecutePagePropertyTest<DateTime>(expected.ToSecondPrecision(), fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModificationDateField()
        {
            DateTime fieldValueDelegate(ContentItem c) => c.LastModificationDate.ToSecondPrecision();
            DateTime expected = DateTime.Parse("1/1/2000").AddSeconds(Int32.MaxValue);
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .LastModificationDate(expected)
                .Build();
            fileContents.ExecutePagePropertyTest<DateTime>(expected.ToSecondPrecision(), fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheSlugField()
        {
            String fieldValueDelegate(ContentItem c) => c.Slug;
            String expected = string.Empty.GetRandom();
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .Slug(expected)
                .Build();
            fileContents.ExecutePagePropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheByLineField()
        {
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            String expectedAuthor = string.Empty.GetRandom();
            String expected = $"by {expectedAuthor}";
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .Author(expectedAuthor)
                .Build();
            fileContents.ExecutePagePropertyTest<String>(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnAnEmptyStringInTheByLineFieldIfAuthorFieldIsMissing()
        {
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .RemoveAuthor()
                .Build();
            fileContents.ExecutePagePropertyTest<String>(string.Empty, fieldValueDelegate);
        }

    }
}
