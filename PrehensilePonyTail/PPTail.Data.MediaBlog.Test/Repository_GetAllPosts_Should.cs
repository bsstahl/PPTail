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
    public class Repository_GetAllPosts_Should
    {
        const String _connectionStringFilepathKey = "FilePath";

        [Fact]
        public void ReturnAllPostsIfAllAreValid()
        {
            Int32 postCount = 3;

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddRandomPosts(postCount)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var posts = target.GetAllPosts();

            Assert.Equal(postCount, posts.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutJsonExtension()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddRandomPosts(7)
                .Build(rootPath)
                .ToArray();

            // Setup some bad file extensions
            postFiles[0].Extension = string.Empty;
            postFiles[2].Extension = ".ppt";
            postFiles[4].Extension = ".txt";
            postFiles[5].Extension = ".com";

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var posts = target.GetAllPosts();

            Assert.Equal(3, posts.Count());
        }

        [Fact]
        public void RequestFilesFromThePostsFolder()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddRandomPosts(1)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var posts = target.GetAllPosts();

            directoryProvider.VerifyAll();
        }


        [Fact]
        public void ReturnTheTagFromASingleTagPost()
        {
            String expected = string.Empty.GetRandom();

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postJson = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .ClearTags()
                .AddTag(expected)
                .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddPost(postJson)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var pages = target.GetAllPosts();
            var actual = pages.Single().Tags.Single();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfTags()
        {
            var expected = 30.GetRandom(3);
            var tags = new List<string>();
            for (Int32 i = 0; i < expected; i++)
            {
                tags.Add(string.Empty.GetRandom());
            }

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postJson = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .ClearTags()
                .AddTags(tags)
                .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddPost(postJson)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var pages = target.GetAllPosts();
            var actual = pages.Single().Tags.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheCorrectTagsFromAMultiTagPost()
        {
            var expectedCount = 30.GetRandom(3);
            var tags = new List<string>();
            for (Int32 i = 0; i < expectedCount; i++)
            {
                tags.Add(string.Empty.GetRandom());
            }
            String expected = tags.AsHash();

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postJson = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .ClearTags()
                .AddTags(tags)
                .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddPost(postJson)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var pages = target.GetAllPosts();
            var actual = pages.Single().Tags.AsHash();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfTagsEvenIfAddedAtDifferentTimes()
        {
            var expected1 = 30.GetRandom(3);
            var expected2 = 30.GetRandom(3);
            var expected = expected1 + expected2;

            var tags1 = new List<string>();
            for (Int32 i = 0; i < expected1; i++)
            {
                tags1.Add(string.Empty.GetRandom());
            }

            var tags2 = new List<string>();
            for (Int32 i = 0; i < expected2; i++)
            {
                tags2.Add(string.Empty.GetRandom());
            }

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postJson = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .ClearTags()
                .AddTags(tags1)
                .AddTags(tags2)
                .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddPost(postJson)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var pages = target.GetAllPosts();
            var actual = pages.Single().Tags.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfTagsEvenIfANullTagCollectionAddedLater()
        {
            var expected = 30.GetRandom(3);
            var tags = new List<string>();
            for (Int32 i = 0; i < expected; i++)
            {
                tags.Add(string.Empty.GetRandom());
            }

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postJson = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .ClearTags()
                .AddTags(tags)
                .AddTags(null)
                .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddPost(postJson)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var pages = target.GetAllPosts();
            var actual = pages.Single().Tags.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheProperValueInTheAuthorField()
        {
            String expected = string.Empty.GetRandom();
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Author(expected)
                .Build();
            String fieldValueDelegate(ContentItem c) => c.Author;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            String expected = string.Empty.GetRandom();
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Title(expected)
                .Build();
            String fieldValueDelegate(ContentItem c) => c.Title;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheDescriptionField()
        {
            String expected = string.Empty.GetRandom();
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Description(expected)
                .Build();
            String fieldValueDelegate(ContentItem c) => c.Description;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentFieldForAnEmptyPost()
        {
            String expected = string.Empty;
            String json = new MediaPostBuilder()
                .UseRandomEmptyPost()
                .Build();
            String fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentFieldForAFlickrPost()
        {
            var post = new FlickrMediaItemBuilder()
                .UseRandom()
                .Build();

            String json = new MediaPostBuilder()
                .AddFlickrImage(post.Title, post.DisplayWidth, post.DisplayHeight, post.CreateDate, post.FlickrListUrl, post.ImageUrl)
                .Build();

            String expected = $"<a data-flickr-embed=\"true\" href=\"{post.FlickrListUrl}\" title=\"{post.Title}\"><img class=\"img-responsive\" src=\"{post.ImageUrl}\" alt=\"{post.Title}\"></a>";

            String fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperVideoUrlInTheContentFieldForAYouTubePost()
        {
            var video = new YouTubeMediaItemBuilder()
                .UseRandom()
                .Build();

            String json = new MediaPostBuilder()
                .AddYouTubeVideo(video.Title, video.DisplayWidth, video.DisplayHeight, video.CreateDate, video.VideoUrl)
                .Build();

            String expected = video.VideoUrl;

            var actual = ExecutePropertyTest(json);

            Assert.Contains(expected, actual.Content);
        }

        [Fact]
        public void ReturnTheProperWidthInTheContentFieldForAYouTubePostIfAValueIsSupplied()
        {
            var video = new YouTubeMediaItemBuilder()
                .UseRandom()
                .Build();

            String json = new MediaPostBuilder()
                .AddYouTubeVideo(video.Title, video.DisplayWidth, video.DisplayHeight, video.CreateDate, video.VideoUrl)
                .Build();

            String expected = $"width=\"{video.DisplayWidth}\"";

            var actual = ExecutePropertyTest(json);

            Assert.True(video.DisplayWidth > 0, "Test is not valid if DisplayWidth is Zero or less");
            Assert.Contains(expected, actual.Content);
        }

        [Fact]
        public void ReturnAnAutoWidthInTheContentFieldForAYouTubePostIfAValueIsZero()
        {
            var video = new YouTubeMediaItemBuilder()
                .UseRandom()
                .DisplayWidth(0)
                .Build();

            String json = new MediaPostBuilder()
                .AddYouTubeVideo(video.Title, video.DisplayWidth, video.DisplayHeight, video.CreateDate, video.VideoUrl)
                .Build();

            String expected = $"width=\"auto\"";

            var actual = ExecutePropertyTest(json);

            Assert.Contains(expected, actual.Content);
        }

        [Fact]
        public void ReturnTheProperHeightInTheContentFieldForAYouTubePostIfAValueIsSupplied()
        {
            var video = new YouTubeMediaItemBuilder()
                .UseRandom()
                .Build();

            String json = new MediaPostBuilder()
                .AddYouTubeVideo(video.Title, video.DisplayWidth, video.DisplayHeight, video.CreateDate, video.VideoUrl)
                .Build();

            String expected = $"height=\"{video.DisplayHeight}\"";

            var actual = ExecutePropertyTest(json);

            Assert.True(video.DisplayHeight > 0, "Test is not valid if DisplayHeight is Zero or less");
            Assert.Contains(expected, actual.Content);
        }

        [Fact]
        public void ReturnAnAutoHeightInTheContentFieldForAYouTubePostIfAValueIsZero()
        {
            var video = new YouTubeMediaItemBuilder()
                .UseRandom()
                .DisplayHeight(0)
                .Build();

            String json = new MediaPostBuilder()
                .AddYouTubeVideo(video.Title, video.DisplayWidth, video.DisplayHeight, video.CreateDate, video.VideoUrl)
                .Build();

            String expected = $"height=\"auto\"";

            var actual = ExecutePropertyTest(json);

            Assert.Contains(expected, actual.Content);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ReturnTheProperValueForIsPublishedForAFlickrPost(Boolean expectedValue)
        {
            String fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .IsPublished(expectedValue)
                .Build();
            String expected = expectedValue.ToString();
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ReturnTheProperValueForIsPublishedForAYouTubePost(Boolean expectedValue)
        {
            String fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();
            String json = new MediaPostBuilder()
                .UseRandomYouTubePost()
                .IsPublished(expectedValue)
                .Build();
            String expected = expectedValue.ToString();
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInThePubDateField()
        {
            String fieldValueDelegate(ContentItem c) => c.PublicationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            String expected = expectedValue.ToString();
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Posted(expectedValue)
                .Build();

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModifiedDateField()
        {
            // LastModifiedDate should be set to the
            // create date of the Media Item

            String fieldValueDelegate(ContentItem c) => c.LastModificationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            String expected = expectedValue.ToString();

            var flickrItem = new FlickrMediaItemBuilder()
                .UseRandom()
                .CreateDate(expectedValue)
                .Build();

            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .AddFlickrImage(flickrItem)
                .Build();

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Theory]
        [InlineData("Two Words", "Two-Words")]
        [InlineData("Three Word Title", "Three-Word-Title")]
        [InlineData("Comma,Separated,Title", "Comma-Separated-Title")]
        [InlineData("With Space,Comma'Apostrophe", "With-Space-CommaApostrophe")]
        [InlineData("With,,Multiple,,,Dashes", "With-Multiple-Dashes")]
        [InlineData("with <html>", "with-&lt;html&gt;")]
        public void ReturnTheProperValueInTheSlugField(String title, String expected)
        {
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Title(title)
                .Build();
            String fieldValueDelegate(ContentItem c) => c.Slug;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheByLineFieldForAPhotoPost()
        {
            String author = string.Empty.GetRandom();
            String expected = $"Photo by {author}";
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Author(author)
                .Build();
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheByLineFieldForAVideoPost()
        {
            String author = string.Empty.GetRandom();
            String expected = $"Video by {author}";
            String json = new MediaPostBuilder()
                .UseRandomYouTubePost()
                .Author(author)
                .Build();
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnAnEmptyStringInTheByLineFieldIfAuthorFieldIsEmpty()
        {
            String expected = string.Empty;
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Author(expected)
                .Build();
            String fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnAnEmptyContentPropertyIfTheMediaItemNodeIsNull()
        {
            String fieldValueDelegate(ContentItem c) => c.Content;

            String expectedValue = String.Empty;
            String expected = expectedValue.ToString();
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .MediaItem(null)
                .Build();

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnAnEmptyContentPropertyIfTheMediaTypeNodeIsNull()
        {
            String fieldValueDelegate(ContentItem c) => c.Content;

            String expectedValue = String.Empty;
            String expected = expectedValue.ToString();
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .MediaType(null)
                .Build();

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ThrowAnInvalidOperationExceptionIfTheMediaTypeIsInvalid()
        {
            String fieldValueDelegate(ContentItem c) => c.Content;

            String expectedValue = String.Empty;
            String expected = expectedValue.ToString();
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .MediaType(String.Empty.GetRandom())
                .Build();

            Assert.Throws<InvalidOperationException>(() => ExecutePropertyTest(expected, fieldValueDelegate, json));
        }

        private static void ExecutePropertyTest(String expected, Func<ContentItem, string> fieldValueDelegate, String postFileContent)
        {
            var actual = ExecutePropertyTest(postFileContent);
            Assert.Equal(expected, fieldValueDelegate(actual));
        }

        private static ContentItem ExecutePropertyTest(String postFileContent)
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddPost(postFileContent)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build(connectionString);

            var pages = target.GetAllPosts();
            return pages.ToArray()[0];
        }
    }
}
