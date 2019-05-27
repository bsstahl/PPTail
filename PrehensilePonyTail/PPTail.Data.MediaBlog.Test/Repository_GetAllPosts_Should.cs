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
    public class Repository_GetAllPosts_Should
    {
        const string _connectionStringFilepathKey = "FilePath";

        [Fact]
        public void ReturnAllPostsIfAllAreValid()
        {
            int postCount = 3;

            string rootPath = $"c:\\{string.Empty.GetRandom()}";
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
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
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
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
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
            string expected = string.Empty.GetRandom();

            string rootPath = $"c:\\{string.Empty.GetRandom()}";
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
            for (int i = 0; i < expected; i++)
            {
                tags.Add(string.Empty.GetRandom());
            }

            string rootPath = $"c:\\{string.Empty.GetRandom()}";
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
            string expected = string.Empty.GetRandom();
            string json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Author(expected)
                .Build();
            string fieldValueDelegate(ContentItem c) => c.Author;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            string expected = string.Empty.GetRandom();
            string json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Title(expected)
                .Build();
            string fieldValueDelegate(ContentItem c) => c.Title;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheDescriptionField()
        {
            string expected = string.Empty.GetRandom();
            string json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Description(expected)
                .Build();
            string fieldValueDelegate(ContentItem c) => c.Description;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentFieldForAnEmptyPost()
        {
            string expected = string.Empty;
            string json = new MediaPostBuilder()
                .UseRandomEmptyPost()
                .Build();
            string fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentFieldForAFlickrPost()
        {
            var post = new FlickrMediaItemBuilder()
                .UseRandom()
                .Build();

            string json = new MediaPostBuilder()
                .AddFlickrImage(post)
                .Build();

            string expected = $"<a data-flickr-embed=\"true\" href=\"{post.FlickrListUrl}\" title=\"{post.Title}\"><img class=\"img-responsive\" src=\"{post.ImageUrl}\" alt=\"{post.Title}\"></a>";

            string fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentFieldForAYouTubePost()
        {
            var video = new YouTubeMediaItemBuilder()
                .UseRandom()
                .Build();

            string json = new MediaPostBuilder()
                .AddYouTubeVideo(video)
                .Build();

            string expected = $"<img class=\"img-responsive\"  title=\"{video.Title}\" src=\"{video.VideoUrl}\" alt=\"{video.Title}\" />";
            string fieldValueDelegate(ContentItem c) => c.Content;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTrueThatThePostIsPublished()
        {
            // Currently, all posts in the repo are considered 
            // published since they can just be removed from 
            // the repo if they are not. Thus, all must return
            // a ContentItem that has IsPublished = True

            string fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();
            bool expectedValue = true;
            string json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Build();
            string expected = expectedValue.ToString();
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInThePubDateField()
        {
            string fieldValueDelegate(ContentItem c) => c.PublicationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string expected = expectedValue.ToString();
            string json = new MediaPostBuilder()
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

            string fieldValueDelegate(ContentItem c) => c.LastModificationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string expected = expectedValue.ToString();

            var flickrItem = new FlickrMediaItemBuilder()
                .UseRandom()
                .CreateDate(expectedValue)
                .Build();

            string json = new MediaPostBuilder()
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
        public void ReturnTheProperValueInTheSlugField(string title, string expected)
        {
            string json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Title(title)
                .Build();
            string fieldValueDelegate(ContentItem c) => c.Slug;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheByLineField()
        {
            string author = string.Empty.GetRandom();
            string expected = $"by {author}";
            string json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Author(author)
                .Build();
            string fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnAnEmptyStringInTheByLineFieldIfAuthorFieldIsEmpty()
        {
            string expected = string.Empty;
            string json = new MediaPostBuilder()
                .UseRandomFlickrPost()
                .Author(expected)
                .Build();
            string fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        private static void ExecutePropertyTest(string expected, Func<ContentItem, string> fieldValueDelegate, string json)
        {
            string rootPath = $"c:\\{string.Empty.GetRandom()}";
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
