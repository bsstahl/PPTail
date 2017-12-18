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
using Newtonsoft.Json.Linq;

namespace PPTail.Data.FileSystem.Wordpress.Test
{
    public class Repository_GetAllPages_Should
    {
        [Fact]
        public void ReturnAllPagesIfAllAreValid()
        {
            var page1 = new PageJsonBuilder().AddRandomValues().Build();
            var page2 = new PageJsonBuilder().AddRandomValues().Build();

            string json = new string[] { page1, page2 }.AsJsonArray();

            var files = new List<string>();
            files.Add("pages.json");

            var directoryProvider = new Mock<IDirectory>();
            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(json);

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\", null);
            var pages = target.GetAllPages();

            Assert.Equal(2, pages.Count());
        }

        [Fact]
        public void OnlyReadPagesDotJsonFile()
        {
            string json = new PageJsonBuilder().AddRandomValues().Build().AsJsonArray();

            var files = new List<string>();
            files.Add("82B52DBC-9D33-4C9E-A933-AF515E4FF140");
            files.Add("28C65CCD-D504-44D3-A54B-9E3DBB163D43.json");
            files.Add("0F716B73-9A2F-46D9-A576-3CA03EB10327.ppt");
            files.Add("8EE89C80-760E-4980-B980-5A4B70A563E2.json");
            files.Add("39836B5E-C330-4670-9897-1CBF0851AB5B.txt");
            files.Add("pages.json");
            files.Add("6D0CDE45-0B8B-4B3A-8817-0FDA6C7D932C.xml");
            files.Add("86F29FA4-29CD-4292-8000-CEAFEA7A2315.com");

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            foreach (var file in files)
                fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                    .Returns(json);

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\", null);
            var pages = target.GetAllPages();

            Assert.Single(pages); // Only 1 file, with 1 page, is the right file
        }

        [Fact]
        public void SkipFilesWithInvalidFormat()
        {
            var files = new List<string>();
            files.Add("pages.json");

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                .Returns(files);

            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns("Not valid Json");

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\", null);
            var pages = target.GetAllPages();

            Assert.Empty(pages);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            string expected = string.Empty.GetRandom();

            var json = new PageJsonBuilder().AddRandomValues().UseTitle(expected).Build().AsJsonArray();
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Title;

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheDescriptionField()
        {
            string expected = string.Empty.GetRandom();

            var json = new PageJsonBuilder().AddRandomValues().UseExcerpt(expected).Build().AsJsonArray();
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Description;

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheContentField()
        {
            string expected = string.Empty.GetRandom();

            var json = new PageJsonBuilder().AddRandomValues().UseContent(expected).Build().AsJsonArray();
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Content;

            ExecutePropertyTest(expected, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTrueForIsPublishedIfThePageStatusIsPublish()
        {
            var json = new PageJsonBuilder().AddRandomValues().UseStatus("publish").Build().AsJsonArray();
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.IsPublished.ToString();
            ExecutePropertyTest(true.ToString(), fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnFalseForIsPublishedIfThePageStatusIsNotPublish()
        {
            var json = new PageJsonBuilder().AddRandomValues().UseStatus(string.Empty.GetRandom()).Build().AsJsonArray();
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.IsPublished.ToString();
            ExecutePropertyTest(false.ToString(), fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInThePubDateField()
        {
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.PublicationDate.ToString();
            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string json = new PageJsonBuilder().AddRandomValues().UseDateGmt(expectedValue).Build().AsJsonArray();
            ExecutePropertyTest(expectedValue.ToString(), fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheLastModifiedDateField()
        {
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.LastModificationDate.ToString();
            DateTime expectedValue = DateTime.UtcNow.AddHours(20.GetRandom(10));
            string json = new PageJsonBuilder().AddRandomValues().UseModifiedDateGmt(expectedValue).Build().AsJsonArray();
            ExecutePropertyTest(expectedValue.ToString(), fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheSlugField()
        {
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Slug;
            string expectedValue = string.Empty.GetRandom();
            string json = new PageJsonBuilder().AddRandomValues().UseSlug(expectedValue).Build().AsJsonArray();
            ExecutePropertyTest(expectedValue, fieldValueDelegate, json);
        }

        [Fact]
        public void ReturnTheProperValueInTheAuthorField()
        {
            int userId = 100.GetRandom(1);
            string userName = string.Empty.GetRandom();

            var users = new Dictionary<int, string>();
            users.Add(200.GetRandom(100), string.Empty.GetRandom());
            users.Add(userId, userName);
            users.Add(300.GetRandom(200), string.Empty.GetRandom());

            var json = new PageJsonBuilder().AddRandomValues().UseAuthor(userId).Build().AsJsonArray();
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.Author;
            ExecutePropertyTest(userName, fieldValueDelegate, json, users);
        }

        [Fact]
        public void ReturnTheProperValueInTheByLineField()
        {
            int userId = 100.GetRandom(1);
            string userName = string.Empty.GetRandom();

            var users = new Dictionary<int, string>();
            users.Add(200.GetRandom(100), string.Empty.GetRandom());
            users.Add(userId, userName);
            users.Add(300.GetRandom(200), string.Empty.GetRandom());

            var json = new PageJsonBuilder().AddRandomValues().UseAuthor(userId).Build().AsJsonArray();
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.ByLine;

            string expected = $"by {userName}";
            ExecutePropertyTest(expected, fieldValueDelegate, json, users);
        }

        [Fact]
        public void ReturnTrueIfThePageIsMarkedShowInList()
        {
            string fieldName = "showinlist";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.ShowInList.ToString();

            bool expectedValue = true;
            string expected = expectedValue.ToString();
            string xml = $"<page><{fieldName}>{expected}</{fieldName}></page>";

            ExecutePropertyTest(expected, fieldValueDelegate, xml);
        }

        [Fact]
        public void ReturnFalseIfThePageIsNotMarkedShowInList()
        {
            string fieldName = "showinlist";
            Func<ContentItem, string> fieldValueDelegate = (ContentItem c) => c.ShowInList.ToString();

            bool expectedValue = false;
            string expected = expectedValue.ToString();
            string xml = $"<page><{fieldName}>{expected}</{fieldName}></page>";

            ExecutePropertyTest(expected, fieldValueDelegate, xml);
        }

        private static void ExecutePropertyTest(string expected, Func<ContentItem, string> fieldValueDelegate, string json, IDictionary<int, string> users = null)
        {
            var files = new List<string>();
            files.Add("pages.json");

            var fileSystem = new Mock<IFile>();
            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(files);

            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(json);

            var target = (null as IContentRepository).Create(fileSystem.Object, directoryProvider.Object, "c:\\", users);
            var pages = target.GetAllPages();
            var actual = pages.ToArray()[0];

            Assert.Equal(expected, fieldValueDelegate(actual));
        }
    }
}
