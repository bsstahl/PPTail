using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Builders;
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
            var categories = new CategoryCollectionBuilder()
                .AddRandomCategories()
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Count(), actual.Count());
        }

        [Fact]
        public void ReturnTheProperIdForEachCategory()
        {
            var categories = new CategoryCollectionBuilder()
                .AddRandomCategories()
                .Build();

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
            var categories = new CategoryCollectionBuilder()
                .AddRandomCategories()
                .Build();

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
            var categories = new CategoryCollectionBuilder()
                .AddRandomCategories()
                .Build();

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
            var categories = new CategoryCollectionBuilder()
                .AddCategory(Guid.Empty, string.Empty.GetRandom(), "Category with empty id")
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }

        [Fact]
        public void SkipACategoryIfItHasNoNameValue()
        {
            var categories = new CategoryCollectionBuilder()
                .AddCategory(Guid.NewGuid(), string.Empty, "Category with empty name")
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }


        [Fact]
        public void SkipACategoryIfItHasWhitespaceForTheNameValue()
        {
            var categories = new CategoryCollectionBuilder()
                .AddCategory(Guid.NewGuid(), "  \t ", "Category with just whitespace in the name")
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Empty(actual);
        }

        [Fact]
        public void LoadACategoryAnywayEvenIfItHasNoDescriptionAttribute()
        {
            var categories = new CategoryCollectionBuilder()
                .AddCategory(Guid.NewGuid(), String.Empty.GetRandom(), string.Empty)
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, _rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, _rootPath);
            var actual = target.GetCategories();

            Assert.Single(actual);
        }
    }
}
