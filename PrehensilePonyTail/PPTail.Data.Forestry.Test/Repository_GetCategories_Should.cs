using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Data.Forestry.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_GetCategories_Should
    {
        const string _rootPath = "c:\\";
        const String _dataFolder = "Data";
        const String _categoriesFileName = "Categories.md";

        [Fact]
        public void ReturnAllCategories()
        {
            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Count(), actual.Count());
        }

        [Fact]
        public void ReturnTheProperIdForEachCategory()
        {
            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            var expectedHash = categories.Select(c => c.Id).AsHash();
            var actualHash = actual.Select(c => c.Id).AsHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void ReturnTheProperNameForEachCategory()
        {
            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
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
            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            foreach (var category in categories)
            {
                var actualCategory = actual.SingleOrDefault(c => c.Id == category.Id);
                Assert.Equal(category.Description, actualCategory.Description);
            }
        }

        [Fact]
        public void SkipACategoryIfItHasAnEmptyIdValue()
        {
            var categories = (null as IEnumerable<Category>).Create(1);
            var category = categories.Single();
            category.Id = Guid.Empty;

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }

        [Fact]
        public void SkipACategoryIfItHasNoNameValue()
        {
            var categories = (null as IEnumerable<Category>).Create(1);
            var category = categories.Single();
            category.Name = string.Empty;

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }


        [Fact]
        public void SkipACategoryIfItHasWhitespaceForTheNameValue()
        {
            var categories = (null as IEnumerable<Category>).Create(1);
            var category = categories.Single();
            category.Name = "  \t ";

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }

        [Fact]
        public void LoadACategoryAnywayEvenIfItHasNoDescriptionAttribute()
        {
            var categories = (null as IEnumerable<Category>).Create(1);
            var category = categories.Single();
            category.Description = string.Empty;

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Single(actual);
        }
    }
}
