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
        const String _dataFolder = "App_Data";

        [Fact]
        public void ReturnAllCategories()
        {
            const String rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Count(), actual.Count());
        }

        [Fact]
        public void ReturnTheProperIdForEachCategory()
        {
            const String rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            foreach (var category in categories)
                Assert.NotNull(actual.SingleOrDefault(c => c.Id == category.Id));
        }

        [Fact]
        public void ReturnTheProperNameForEachCategory()
        {
            const String rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            foreach (var category in categories)
            {
                var actualCategory = actual.SingleOrDefault(c => c.Id == category.Id);
                Assert.Equal(category.Name, actualCategory.Name);
            }
        }

        [Fact]
        public void ReturnTheProperDescriptionForEachCategory()
        {
            const String rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            foreach (var category in categories)
            {
                var actualCategory = actual.SingleOrDefault(c => c.Id == category.Id);
                Assert.Equal(category.Description, actualCategory.Description);
            }
        }

        [Fact]
        public void SkipACategoryIfItHasNoIdAttribute()
        {
            const String rootPath = "c:\\";
            String categoryFilePath = System.IO.Path.Combine(rootPath, "App_Data\\categories.xml");

            var categories = (null as IEnumerable<Category>).Create(1);
            var categoryFileContents = categories.Serialize();
            var categoryNode = categoryFileContents.Descendants().Single();

            var attribute = categoryNode.Attributes().Single(n => n.Name.LocalName == "id");
            attribute.Remove();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }

        [Fact]
        public void SkipACategoryIfItHasNoNameValue()
        {
            const String rootPath = "c:\\";
            String categoryFilePath = System.IO.Path.Combine(rootPath, "App_Data\\categories.xml");

            var categories = (null as IEnumerable<Category>).Create(1);
            categories.Single().Name = string.Empty;
            var categoryFileContents = categories.Serialize();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }

        [Fact]
        public void SkipACategoryIfItHasWhitespaceForTheNameValue()
        {
            const String rootPath = "c:\\";
            String categoryFilePath = System.IO.Path.Combine(rootPath, "App_Data\\categories.xml");

            var categories = (null as IEnumerable<Category>).Create(1);
            categories.Single().Name = "   ";
            var categoryFileContents = categories.Serialize();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }

        [Fact]
        public void LoadACategoryAnywayEvenIfItHasNoDescriptionAttribute()
        {
            const String rootPath = "c:\\";
            String categoryFilePath = System.IO.Path.Combine(rootPath, "App_Data\\categories.xml");

            var categories = (null as IEnumerable<Category>).Create(1);
            var categoryFileContents = categories.Serialize();
            var categoryNode = categoryFileContents.Descendants().Single();

            var attribute = categoryNode.Attributes().Single(n => n.Name.LocalName == "description");
            attribute.Remove();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Single(actual);
        }
    }
}
