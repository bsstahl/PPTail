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
    public class Repository_GetAllPosts_Should
    {
        const String _postsFolder = "Posts";
        const String _connectionStringFilepathKey = "FilePath";

        [Fact]
        public void ReturnAllPostsIfAllAreValid()
        {
            var files = new List<string>
            {
                "28C65CCD-D504-44D3-A54B-9E3DBB163D43.md",
                "8EE89C80-760E-4980-B980-5A4B70A563E2.md",
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.md"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(new ContentItemFileBuilder().UseRandomValues().Build());

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var posts = target.GetAllPosts();

            Assert.Equal(files.Count(), posts.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutMdExtension()
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

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(new ContentItemFileBuilder().UseRandomValues().Build());

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var posts = target.GetAllPosts();

            Assert.Equal(3, posts.Count());
        }

        [Fact]
        public void RequestFilesFromThePostsFolder()
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.md"
            };

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            String expectedPath = System.IO.Path.Combine(rootPath, _postsFolder);

            var settings = new Settings() { SourceConnection = $"Provider=this;{_connectionStringFilepathKey}={rootPath}" };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(expectedPath))
                .Returns(files)
                .Verifiable();

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(new ContentItemFileBuilder().UseRandomValues().Build());

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem.Object);
            container.AddSingleton<IDirectory>(directoryProvider.Object);
            container.AddSingleton<ISettings>(settings);

            var target = (null as IContentRepository).Create(container.BuildServiceProvider());
            var posts = target.GetAllPosts();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void SkipPostsWithInvalidFormat()
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.md"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("Not a valid Post");

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var posts = target.GetAllPosts();

            Assert.Empty(posts);
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
        public void ReturnTheProperValueInTheContentField()
        {
            String fieldValueDelegate(ContentItem c) => c.Content;
            string expected = string.Empty.GetRandom();
            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .Content(expected)
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
            List<Guid> categories = new List<Guid>() { Guid.NewGuid() };

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .CategoryIds(categories)
                .Build();
            fileContent.ExecutePostPropertyTest(categories.AsHash(), fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheCategoriesFromAMultipleCategoryPost()
        {
            String fieldValueDelegate(ContentItem c) => c.CategoryIds.AsHash();
            Int32 expectedCount = 10.GetRandom(3);
            List<Guid> categories = new List<Guid>();
            for (Int32 i = 0; i < expectedCount; i++)
                categories.Add(Guid.NewGuid());

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .CategoryIds(categories)
                .Build();
            fileContent.ExecutePostPropertyTest(categories.AsHash(), fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfCategoryIds()
        {
            Int32 fieldValueDelegate(ContentItem c) => c.CategoryIds?.Count() ?? 0;
            Int32 expectedCount = 10.GetRandom(3);
            List<Guid> categories = new List<Guid>();
            for (Int32 i = 0; i < expectedCount; i++)
                categories.Add(Guid.NewGuid());

            var fileContent = new ContentItemFileBuilder()
                .UseRandomValues()
                .CategoryIds(categories)
                .Build();
            fileContent.ExecutePostPropertyTest(expectedCount, fieldValueDelegate);
        }
    }
}
