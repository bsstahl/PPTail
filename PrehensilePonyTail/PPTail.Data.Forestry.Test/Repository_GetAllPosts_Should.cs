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
using Xunit.Sdk;

namespace PPTail.Data.Forestry.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_GetAllPosts_Should
    {
        const String _postsFolder = "Posts";
        const String _connectionStringFilepathKey = "FilePath";

        [Fact]
        public void ReturnAllPostsIfAllAreValid()
        {
            int expected = 20.GetRandom(5);

            var fileSystemBuilder = new FileSystemBuilder()
                .AddRandomContentItemFiles(expected)
                .AddRandomCategories();

            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(fileSystemBuilder.ContentItemFileNames);

            var target = (null as IContentRepository).Create(fileSystemBuilder.Build(), directoryProvider.Object, "c:\\");
            var posts = target.GetAllPosts();

            Assert.Equal(expected, posts.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutMdExtension()
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
            var posts = target.GetAllPosts();

            Assert.Equal(8, posts.Count());
        }

        [Fact]
        public void RequestFilesFromThePostsFolder()
        {
            var fileSystemBuilder = new FileSystemBuilder()
                .AddRandomContentItemFiles()
                .AddRandomCategories();

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            String expectedPath = System.IO.Path.Combine(rootPath, _postsFolder);

            var sourceConnection = $"Provider=this;{_connectionStringFilepathKey}={rootPath}";

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(f => f.EnumerateFiles(expectedPath))
                .Returns(fileSystemBuilder.ContentItemFileNames)
                .Verifiable();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystemBuilder.Build());
            container.AddSingleton<IDirectory>(directoryProvider.Object);

            var target = new Repository(container.BuildServiceProvider(), sourceConnection);
            var posts = target.GetAllPosts();

            directoryProvider.VerifyAll();
        }

        [Fact]
        public void ThrowAnInvalidOperationExceptionIfAnyPostHasAnInvalidFormat()
        {
            var fileSystemBuilder = new FileSystemBuilder()
                .AddContentItemFile($"{string.Empty.GetRandom()}.md", "Not a valid post")
                .AddRandomCategories();

            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(fileSystemBuilder.ContentItemFileNames);

            var target = (null as IContentRepository).Create(fileSystemBuilder.Build(), directoryProvider.Object, "c:\\");
            Assert.Throws<InvalidOperationException>(() => _ = target.GetAllPosts());
        }

        [Fact]
        public void ReturnTheProperValueInTheIdField()
        {
            Guid fieldValueDelegate(ContentItem c) => c.Id;
            var expected = Guid.NewGuid();
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Id(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheIdFieldIfDelimitedBySingleQuotes()
        {
            Guid fieldValueDelegate(ContentItem c) => c.Id;
            var expected = Guid.NewGuid();
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Id(expected)
                .IdDelimiter('\'')
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheAuthorField()
        {
            String fieldValueDelegate(ContentItem c) => c.Author;
            string expected = string.Empty.GetRandom();
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Author(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            String fieldValueDelegate(ContentItem c) => c.Title;
            string expected = string.Empty.GetRandom();
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Title(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheDescriptionField()
        {
            String fieldValueDelegate(ContentItem c) => c.Description;
            string expected = string.Empty.GetRandom();
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Description(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
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
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }



        [Fact]
        public void ReturnTheHtmlFormattedValueInTheContentField()
        {
            String fieldValueDelegate(ContentItem c) => c.Content.Trim();
            string content = string.Empty.GetRandom();
            String expected = $"<p>{content}</p>";
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Content(content)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheCorrectValueInTheContentFieldEvenIfItContainsAHorizontalRule()
        {
            String fieldValueDelegate(ContentItem c) => c.Content.Trim();
            string field1 = string.Empty.GetRandom();
            string field2 = string.Empty.GetRandom();
            string content = $"<h1>{field1}</h1>\n---\n<h2>{field2}</h2>";
            String expected = $"<h1>{field1}</h1>\n<hr />\n<h2>{field2}</h2>\n".Trim();

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Content(content)
                .Build();

            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTrueIfThePostIsPublished()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.IsPublished;
            Boolean expected = true;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .IsPublished(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnFalseIfThePostIsNotPublished()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.IsPublished;
            Boolean expected = false;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .IsPublished(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnFalseIfTheIsPublishedFieldIsMissing()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.IsPublished;
            Boolean expected = false;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .RemoveIsPublished()
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTrueIfThePostIsMarkedBuildIfNotPublished()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.BuildIfNotPublished;
            Boolean expected = true;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .BuildIfNotPublished(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnFalseIfThePostIsNotMarkedBuildIfNotPublished()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.BuildIfNotPublished;
            Boolean expected = false;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .BuildIfNotPublished(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnFalseIfTheBuildIfNotPublishedFieldIsMissing()
        {
            Boolean fieldValueDelegate(ContentItem c) => c.BuildIfNotPublished;
            Boolean expected = false;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .RemoveBuildIfNotPublished()
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInThePublicationDateField()
        {
            DateTime fieldValueDelegate(ContentItem c) => c.PublicationDate.ToSecondPrecision();
            DateTime expected = DateTime.Parse("1/1/1900").AddSeconds(Int32.MaxValue.GetRandom());
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .PublicationDate(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInThePublicationDateFieldIfFullIso8601Format()
        {
            DateTime fieldValueDelegate(ContentItem c) => c.PublicationDate.ToSecondPrecision();
            DateTime expected = DateTime.Parse("1/1/1900").AddSeconds(Int32.MaxValue.GetRandom());
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .PublicationDate(expected)
                .PublicationDateSerializationFormat("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz")
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModifiedDateField()
        {
            DateTime fieldValueDelegate(ContentItem c) => c.LastModificationDate.ToSecondPrecision();
            DateTime expected = DateTime.Parse("1/1/1900").AddSeconds(Int32.MaxValue.GetRandom());
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .LastModificationDate(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModifiedDateFieldIfFullIso8601Format()
        {
            DateTime fieldValueDelegate(ContentItem c) => c.LastModificationDate.ToSecondPrecision();
            DateTime expected = DateTime.Parse("1/1/1900").AddSeconds(Int32.MaxValue.GetRandom());
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .LastModificationDate(expected)
                .LastModificationDateSerializationFormat("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz")
                .Build();
            fileContent.ExecutePostPropertyTest(expected.ToSecondPrecision(), fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheSlugField()
        {
            String fieldValueDelegate(ContentItem c) => c.Slug;
            string expected = string.Empty.GetRandom();
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Slug(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheByLineField()
        {
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            String author = string.Empty.GetRandom();
            String expected = $"by {author}";
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Author(author)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnAnEmptyStringInTheByLineFieldIfAuthorFieldIsEmpty()
        {
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            String author = string.Empty;
            String expected = string.Empty;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Author(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnAnEmptyStringInTheByLineFieldIfAuthorFieldIsMissing()
        {
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            String expected = string.Empty;
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .RemoveAuthor()
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheTagFromASingleTagPost()
        {
            String fieldValueDelegate(ContentItem c) => c.Tags.AsHash();
            String[] expected = new[] { string.Empty.GetRandom() };
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Tags(expected)
                .Build();
            fileContent.ExecutePostPropertyTest(expected.AsHash(), fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheTagFromAMultipleTagPost()
        {
            String fieldValueDelegate(ContentItem c) => c.Tags.AsHash();
            Int32 expectedCount = 10.GetRandom(3);
            List<String> tags = new List<string>();
            for (Int32 i = 0; i < expectedCount; i++)
                tags.Add(string.Empty.GetRandom());

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Tags(tags)
                .Build();
            fileContent.ExecutePostPropertyTest(tags.AsHash(), fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfTags()
        {
            Int32 fieldValueDelegate(ContentItem c) => c.Tags?.Count() ?? 0;

            Int32 expected = 10.GetRandom(3);
            List<String> tags = new List<string>();
            for (Int32 i = 0; i < expected; i++)
                tags.Add(string.Empty.GetRandom());

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Tags(tags)
                .Build();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheCategoryFromASingleCategoryPost()
        {
            String fieldValueDelegate(ContentItem c) => c.CategoryIds.AsHash();
            List<String> categories = new List<String>() { String.Empty.GetRandom() };

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Categories(categories)
                .Build();

            var sourceCategories = categories.AsCategoryEntities().ToArray();
            string expected = sourceCategories.Select(c => c.Id).AsHash();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate, sourceCategories);
        }

        [Fact]
        public void ReturnTheCategoriesFromAMultipleCategoryPost()
        {
            String fieldValueDelegate(ContentItem c) => c.CategoryIds.AsHash();
            Int32 expectedCount = 10.GetRandom(3);
            var categories = new List<String>();
            for (Int32 i = 0; i < expectedCount; i++)
                categories.Add(String.Empty.GetRandom());

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Categories(categories)
                .Build();

            var sourceCategories = categories.AsCategoryEntities().ToArray();
            string expected = sourceCategories.Select(c => c.Id).AsHash();
            fileContent.ExecutePostPropertyTest(expected, fieldValueDelegate, sourceCategories);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfCategories()
        {
            Int32 fieldValueDelegate(ContentItem c) => c.CategoryIds?.Count() ?? 0;
            Int32 expectedCount = 10.GetRandom(3);
            var categories = new List<String>();
            for (Int32 i = 0; i < expectedCount; i++)
                categories.Add(String.Empty.GetRandom());

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Categories(categories)
                .Build();

            var sourceCategories = categories.AsCategoryEntities();
            fileContent.ExecutePostPropertyTest(expectedCount, fieldValueDelegate, sourceCategories);
        }
    }
}
