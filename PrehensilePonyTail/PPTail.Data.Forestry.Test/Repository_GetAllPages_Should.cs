using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using System.Linq;
using Xunit;
using TestHelperExtensions;
using Moq;
using PPTail.Entities;

namespace PPTail.Data.Forestry.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_GetAllPages_Should
    {
        const String _connectionStringFilepathKey = "FilePath";
        const String _dataFolder = "Pages";

        [Fact]
        public void ReturnAllPagesIfAllAreValid()
        {
            int expected = 10.GetRandom(3);

            var fileSystemBuilder = new FileSystemBuilder()
                .AddRandomContentItemFiles(expected)
                .AddRandomCategories();
                
            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(fileSystemBuilder.ContentItemFileNames);

            var fileSystem = fileSystemBuilder.Build();

            var target = (null as IContentRepository).Create(fileSystem, directoryProvider.Object, "c:\\");
            var pages = target.GetAllPages();

            Assert.Equal(expected, pages.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutMDExtension()
        {
            var fileSystemBuilder = new FileSystemBuilder()
                .AddRandomContentItemFiles(2)
                .AddContentItemFileWithRandomContent(string.Empty.GetRandom())
                .AddRandomContentItemFiles(1)
                .AddContentItemFileWithRandomContent($"{string.Empty.GetRandom()}.ppt")
                .AddRandomContentItemFiles(1)
                .AddContentItemFileWithRandomContent($"{string.Empty.GetRandom()}.txt")
                .AddRandomContentItemFiles(3)
                .AddContentItemFileWithRandomContent($"{string.Empty.GetRandom()}.com")
                .AddRandomContentItemFiles(1)
                .AddRandomCategories();

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(fileSystemBuilder.ContentItemFileNames);

            var target = (null as IContentRepository).Create(fileSystemBuilder.Build(), directoryProvider.Object, "c:\\");
            var pages = target.GetAllPages();

            Assert.Equal(8, pages.Count());
        }

        [Fact]
        public void RequestFilesFromThePagesFolder()
        {
            var fileSystemBuilder = new FileSystemBuilder()
                .AddRandomContentItemFiles()
                .AddRandomCategories();

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            String expectedPath = System.IO.Path.Combine(rootPath, _dataFolder);

            var sourceConnection = $"Provider=this;{_connectionStringFilepathKey}={rootPath}";

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(f => f.EnumerateFiles(expectedPath))
                .Returns(fileSystemBuilder.ContentItemFileNames)
                .Verifiable();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystemBuilder.Build());
            container.AddSingleton<IDirectory>(directoryProvider.Object);

            var target = new Repository(container.BuildServiceProvider(), sourceConnection);
            var pages = target.GetAllPages();

            directoryProvider.VerifyAll();
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
        public void ReturnAnEmptyStringInTheByDescriptionFieldIfDescriptionIsMissing()
        {
            String fieldValueDelegate(ContentItem c) => c.Description;
            String expected = string.Empty;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .RemoveDescription()
                .Build();
            fileContent.ExecutePagePropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheHtmlFormattedValueInTheContentField()
        {
            String fieldValueDelegate(ContentItem c) => c.Content.Trim();
            String content = string.Empty.GetRandom(500);
            String expected = $"<p>{content}</p>";
            var fileContents = new ContentItemFileBuilder()
                .UseRandomValues()
                .Content(content)
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
