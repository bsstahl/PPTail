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
        [Fact]
        public void ReturnAllPostsIfAllAreValid()
        {
            var files = new List<string>();
            files.Add("28C65CCD-D504-44D3-A54B-9E3DBB163D43.xml");
            files.Add("8EE89C80-760E-4980-B980-5A4B70A563E2.xml");
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("<post/>");

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem.Object);

            var target = (null as IContentRepository).Create(serviceProvider);
            var posts = target.GetAllPosts();

            Assert.Equal(files.Count(), posts.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutXmlExtension()
        {
            var files = new List<string>();
            files.Add("82B52DBC-9D33-4C9E-A933-AF515E4FF140");
            files.Add("28C65CCD-D504-44D3-A54B-9E3DBB163D43.xml");
            files.Add("0F716B73-9A2F-46D9-A576-3CA03EB10327.ppt");
            files.Add("8EE89C80-760E-4980-B980-5A4B70A563E2.xml");
            files.Add("39836B5E-C330-4670-9897-1CBF0851AB5B.txt");
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");
            files.Add("86F29FA4-29CD-4292-8000-CEAFEA7A2315.com");

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("<post/>");

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem.Object);

            var target = (null as IContentRepository).Create(serviceProvider);
            var posts = target.GetAllPosts();

            Assert.Equal(3, posts.Count());
        }

        [Fact]
        public void RequestFilesFromThePostsFolder()
        {
            var files = new List<string>();
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");

            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            string expectedPath = rootPath + "\\posts";

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(f => f.EnumerateFiles(expectedPath))
                .Returns(files).Verifiable();

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("<post/>");

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem.Object);

            var target = (null as IContentRepository).Create(serviceProvider, rootPath);
            var posts = target.GetAllPosts();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void SkipPostsWithInvalidSchema()
        {
            var files = new List<string>();
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("Not valid XML");

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem.Object);

            var target = (null as IContentRepository).Create(serviceProvider);
            var posts = target.GetAllPosts();

            Assert.Equal(0, posts.Count());
        }

        [Fact]
        public void SkipPostsWithTheWrongRootNode()
        {
            var files = new List<string>();
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns("<posts/>");

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem.Object);

            var target = (null as IContentRepository).Create(serviceProvider);
            var posts = target.GetAllPosts();

            Assert.Equal(0, posts.Count());
        }

        [Fact]
        public void ReturnTheProperValueInTheAuthorField()
        {
            string fieldName = "author";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Author;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            string fieldName = "title";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Title;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheDescriptionField()
        {
            string fieldName = "description";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Description;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentField()
        {
            string fieldName = "content";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Content;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTrueIfThepostIsPublished()
        {
            string fieldName = "ispublished";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.IsPublished.ToString();

            bool expectedValue = true;
            string expected = expectedValue.ToString();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";

            ExecutePropertyTest(fieldName, expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnFalseIfThepostIsNotPublished()
        {
            string fieldName = "ispublished";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.IsPublished.ToString();

            bool expectedValue = false;
            string expected = expectedValue.ToString();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";

            ExecutePropertyTest(fieldName, expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnTheProperValueInThePubDateField()
        {
            string fieldName = "pubDate";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.PublicationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string expected = expectedValue.ToString();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";

            ExecutePropertyTest(fieldName, expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModifiedDateField()
        {
            string fieldName = "lastModified";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.LastModificationDate.ToString();

            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string expected = expectedValue.ToString();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";

            ExecutePropertyTest(fieldName, expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnTheProperValueInTheSlugField()
        {
            string fieldName = "slug";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Slug;
            ExecutePropertyTest(fieldName, fieldValueDelegate);
        }

        private static void ExecutePropertyTest(string fieldName, Func<ContentItem, string> fieldValueDelegate)
        {
            string expected = string.Empty.GetRandom();
            string xml = $"<post><{fieldName}>{expected}</{fieldName}></post>";
            ExecutePropertyTest(fieldName, expected, fieldValueDelegate, xml);
        }

        private static void ExecutePropertyTest(string fieldName, string expected, Func<ContentItem, string> fieldValueDelegate, string xml)
        {
            var files = new List<string>();
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(xml);

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem.Object);

            var target = (null as IContentRepository).Create(serviceProvider);
            var pages = target.GetAllPosts();
            var actual = pages.ToArray()[0];

            Assert.Equal(expected, fieldValueDelegate(actual));
        }
    }
}