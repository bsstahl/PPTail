using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Builders;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_GetCategories_Should
    {
        [Fact]
        public void ReturnAllCategories()
        {
            Int32 categoryCount = 20.GetRandom(5);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var fileSystem = new MockFileServiceBuilder()
                .AddRandomCategories(categoryCount)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

            var actual = target.GetCategories();

            Assert.Equal(categoryCount, actual.Count());
        }

        [Fact]
        public void ReturnTheProperIdForEachCategory()
        {
            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var categories = new CategoryCollectionBuilder()
                .AddRandomCategories(20.GetRandom(5))
                .Build();

            var fileSystem = new MockFileServiceBuilder()
                .AddCategories(categories)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

            var actual = target.GetCategories();

            foreach (var category in categories)
                Assert.NotNull(actual.SingleOrDefault(c => c.Id == category.Id));
        }

        [Fact]
        public void ReturnTheProperNameForEachCategory()
        {
            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var categories = new CategoryCollectionBuilder()
                .AddRandomCategories(20.GetRandom(5))
                .Build();

            var fileSystem = new MockFileServiceBuilder()
                .AddCategories(categories)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

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
            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var categories = new CategoryCollectionBuilder()
                .AddRandomCategories(20.GetRandom(5))
                .Build();

            var fileSystem = new MockFileServiceBuilder()
                .AddCategories(categories)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

            var actual = target.GetCategories();

            foreach (var category in categories)
            {
                var actualCategory = actual.SingleOrDefault(c => c.Id == category.Id);
                Assert.Equal(category.Description, actualCategory.Description);
            }
        }

    }
}
