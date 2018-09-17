﻿using Microsoft.Extensions.DependencyInjection;
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

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_GetAllPosts_Should
    {
        const string _dataFolder = "App_Data";
        const string _connectionStringFilepathKey = "FilePath";

        [Fact]
        public void ReturnAllPostsIfAllAreValid()
        {
            var files = new List<string>
            {
                "28C65CCD-D504-44D3-A54B-9E3DBB163D43.xml",
                "8EE89C80-760E-4980-B980-5A4B70A563E2.xml",
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("<post/>");

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var posts = target.GetAllPosts();

            Assert.Equal(files.Count(), posts.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutXmlExtension()
        {
            var files = new List<string>
            {
                "82B52DBC-9D33-4C9E-A933-AF515E4FF140",
                "28C65CCD-D504-44D3-A54B-9E3DBB163D43.xml",
                "0F716B73-9A2F-46D9-A576-3CA03EB10327.ppt",
                "8EE89C80-760E-4980-B980-5A4B70A563E2.xml",
                "39836B5E-C330-4670-9897-1CBF0851AB5B.txt",
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml",
                "86F29FA4-29CD-4292-8000-CEAFEA7A2315.com"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("<post/>");

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var posts = target.GetAllPosts();

            Assert.Equal(3, posts.Count());
        }

        [Fact]
        public void RequestFilesFromThePostsFolder()
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string expectedPath = System.IO.Path.Combine(rootPath, _dataFolder, "posts");

            var settings = new Settings() { SourceConnection = $"Provider=this;{_connectionStringFilepathKey}={rootPath}" };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(expectedPath))
                .Returns(files).Verifiable();

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("<post/>");

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem.Object);
            container.AddSingleton<IDirectory>(directoryProvider.Object);
            container.AddSingleton<ISettings>(settings);

            var target = (null as IContentRepository).Create(container.BuildServiceProvider());
            var posts = target.GetAllPosts();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void SkipPostsWithInvalidSchema()
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("Not valid XML");

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var posts = target.GetAllPosts();

            Assert.Empty(posts);
        }

        [Fact]
        public void SkipPostsWithTheWrongRootNode()
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("<posts/>");

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var posts = target.GetAllPosts();

            Assert.Empty(posts);
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
        public void ReturnTrueIfThePostIsPublished()
        {
            string fieldName = "ispublished";
            string fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();

            bool expectedValue = true;
            string expected = expectedValue.ToString();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";

            ExecutePropertyTest(expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnFalseIfThePostIsNotPublished()
        {
            string fieldName = "ispublished";
            string fieldValueDelegate(ContentItem c) => c.IsPublished.ToString();

            bool expectedValue = false;
            string expected = expectedValue.ToString();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";

            ExecutePropertyTest(expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnTheProperValueInThePubDateField()
        {
            string fieldName = "pubDate";
            string fieldValueDelegate(ContentItem c) => c.PublicationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string expected = expectedValue.ToString();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";

            ExecutePropertyTest(expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModifiedDateField()
        {
            string fieldName = "lastModified";
            string fieldValueDelegate(ContentItem c) => c.LastModificationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string expected = expectedValue.ToString();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";

            ExecutePropertyTest(expected, fieldValueDelegate, xml);
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
            string xml = $"<post><author>{author}</author></post>";
            string fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnAnEmptyStringInTheByLineFieldIfAuthorFieldIsEmpty()
        {
            string expected = string.Empty;
            string xml = $"<post/>";
            string fieldValueDelegate(ContentItem c) => c.ByLine;
            ExecutePropertyTest(expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnTheTagFromASingleTagPost()
        {
            string expected = string.Empty.GetRandom();
            string xml = $"<post><tags><tag>{expected}</tag></tags></post>";

            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var pages = target.GetAllPosts();
            var actual = pages.Single().Tags.Single();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfTags()
        {
            var expected = 10.GetRandom(3);

            string tagNodes = string.Empty;
            for (int i = 0; i < expected; i++)
                tagNodes += $"<tag>{string.Empty.GetRandom()}</tag>";
            string xml = $"<post><tags>{tagNodes}</tags></post>";

            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var pages = target.GetAllPosts();
            var actual = pages.Single().Tags.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheCategoryFromASingleCategoryPost()
        {
            var expected = Guid.NewGuid();
            string xml = $"<post><categories><category>{expected.ToString()}</category></categories></post>";

            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var pages = target.GetAllPosts();

            var actualPage = pages.Single();
            var actual = actualPage.CategoryIds.Single();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheCategoriesFromAMultipleCategoryPost()
        {
            var expected1 = Guid.NewGuid();
            var expected2 = Guid.NewGuid();
            var expected3 = Guid.NewGuid();
            var categoryIds = new List<Guid>() { expected1, expected2, expected3 };

            string xml = $"<post><categories><category>{expected1.ToString()}</category><category>{expected2.ToString()}</category><category>{expected3.ToString()}</category></categories></post>";

            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var pages = target.GetAllPosts();

            var actualPage = pages.Single();
            var actual = actualPage.CategoryIds;

            foreach (var categoryid in categoryIds)
                Assert.Contains(categoryid, actual);
        }

        private static void ExecutePropertyTest(string fieldName, Func<ContentItem, string> fieldValueDelegate)
        {
            // Added a "decoy" value to make sure we are getting the right elements
            string expected = string.Empty.GetRandom();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}><childElement><{fieldName}>{string.Empty.GetRandom()}</{fieldName}></childElement></post>";
            ExecutePropertyTest(expected, fieldValueDelegate, xml);
        }

        private static void ExecutePropertyTest(string expected, Func<ContentItem, string> fieldValueDelegate, string xml)
        {
            var files = new List<string>
            {
                "68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml"
            };

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\");
            var pages = target.GetAllPosts();
            var actual = pages.ToArray()[0];

            Assert.Equal(expected, fieldValueDelegate(actual));
        }
    }
}
