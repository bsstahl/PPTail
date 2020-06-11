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
            var settings = new SettingsBuilder()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build())
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
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build();

            var posts = target.GetAllPosts();

            Assert.Equal(postCount, posts.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutJsonExtension()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var settings = new SettingsBuilder()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build())
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
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build();

            var posts = target.GetAllPosts();

            Assert.Equal(3, posts.Count());
        }

        [Fact]
        public void RequestFilesFromThePostsFolder()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var settings = new SettingsBuilder()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build())
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
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build();

            var posts = target.GetAllPosts();

            directoryProvider.VerifyAll();
        }


        [Fact]
        public void ReturnTheTagFromASingleTagPost()
        {
            String expected = string.Empty.GetRandom();

            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var settings = new SettingsBuilder()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build())
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
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build();

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
            var settings = new SettingsBuilder()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build())
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
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build();

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
                .AddFlickrImage(post)
                .Build();

            String expected = $"<a data-flickr-embed=\"true\" href=\"{post.FlickrListUrl}\" title=\"{post.Title}\"><img class=\"img-responsive\" src=\"{post.ImageUrl}\" alt=\"{post.Title}\"></a>";

            String fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentFieldForAYouTubePost()
        {
            var video = new YouTubeMediaItemBuilder()
                .UseRandom()
                .Build();

            String json = new MediaPostBuilder()
                .AddYouTubeVideo(video)
                .Build();

            String expected = $"<img class=\"img-responsive\"  title=\"{video.Title}\" src=\"{video.VideoUrl}\" alt=\"{video.Title}\" />";
            String fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTrueThatThePostIsPublished()
        {
            // Currently, all posts in the repo are considered 
            // published since they can just be removed from 
            // the repo if they are not. Thus, all must return
            // a ContentItem that has IsPublished = True

            String fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();
            bool expectedValue = true;
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
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
        public void ReturnTheProperValueInTheByLineField()
        {
            String author = string.Empty.GetRandom();
            String expected = $"by {author}";
            String json = new MediaPostBuilder()
                .UseRandomFlickrPost()
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

        private static void ExecutePropertyTest(String expected, Func<ContentItem, string> fieldValueDelegate, String json)
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}";
            var settings = new SettingsBuilder()
                .SourceConnection(
                    new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build())
                .Build();

            var postFiles = new MockMediaFileCollectionBuilder()
                .AddPost(json)
                .Build(rootPath);

            var directoryProvider = new MockDirectoryServiceBuilder()
                .AddPostFiles(postFiles.Select(f => f.GetFilename()))
                .Build(rootPath);

            var fileSystem = new MockFileServiceBuilder()
                .AddPosts(postFiles)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .AddDirectoryService(directoryProvider.Object)
                .Build();

            var pages = target.GetAllPosts();
            var actual = pages.ToArray()[0];

            Assert.Equal(expected, fieldValueDelegate(actual));
        }
    }
}
