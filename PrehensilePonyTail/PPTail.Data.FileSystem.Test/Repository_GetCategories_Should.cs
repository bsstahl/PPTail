using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_GetCategories_Should
    {
        const string _dataFolder = "App_Data";

        [Fact]
        public void ReturnAllCategories()
        {
            const string rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Count(), actual.Count());
        }

        [Fact]
        public void ReturnTheProperIdForACategory()
        {
            const string rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create(1);

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Single().Id, actual.Single().Id);
        }

        [Fact]
        public void ReturnTheProperNameForACategory()
        {
            const string rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create(1);

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Single().Name, actual.Single().Name);
        }

        [Fact]
        public void ReturnTheProperDescriptionForACategory()
        {
            const string rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create(1);

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Single().Description, actual.Single().Description);
        }

        [Fact]
        public void SkipACategoryIfItHasNoIdAttribute()
        {
            const string rootPath = "c:\\";
            string categoryFilePath = System.IO.Path.Combine(rootPath, "App_Data\\categories.xml");

            var categories = (null as IEnumerable<Category>).Create(1);
            var categoryFileContents = categories.Serialize();
            var categoryNode = categoryFileContents.Descendants().Single();

            var attribute = categoryNode.Attributes().Single(n => n.Name.LocalName == "id");
            attribute.Remove();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(0, actual.Count());
        }

        [Fact]
        public void SkipACategoryIfItHasNoNameValue()
        {
            const string rootPath = "c:\\";
            string categoryFilePath = System.IO.Path.Combine(rootPath, "App_Data\\categories.xml");

            var categories = (null as IEnumerable<Category>).Create(1);
            categories.Single().Name = string.Empty;
            var categoryFileContents = categories.Serialize();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(0, actual.Count());
        }

        [Fact]
        public void SkipACategoryIfItHasWhitespaceForTheNameValue()
        {
            const string rootPath = "c:\\";
            string categoryFilePath = System.IO.Path.Combine(rootPath, "App_Data\\categories.xml");

            var categories = (null as IEnumerable<Category>).Create(1);
            categories.Single().Name = "   ";
            var categoryFileContents = categories.Serialize();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(0, actual.Count());
        }

        [Fact]
        public void LoadACategoryAnywayEvenIfItHasNoDescriptionAttribute()
        {
            const string rootPath = "c:\\";
            string categoryFilePath = System.IO.Path.Combine(rootPath, "App_Data\\categories.xml");

            var categories = (null as IEnumerable<Category>).Create(1);
            var categoryFileContents = categories.Serialize();
            var categoryNode = categoryFileContents.Descendants().Single();

            var attribute = categoryNode.Attributes().Single(n => n.Name.LocalName == "description");
            attribute.Remove();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(1, actual.Count());
        }
    }
}
