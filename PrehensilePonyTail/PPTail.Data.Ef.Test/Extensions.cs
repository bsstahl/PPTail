using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Data.Ef.Test
{
    public static class Extensions
    {
        public static ContentItem Create(this ContentItem ignore)
        {
            return new ContentItem()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty.GetRandom(),
                Author = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                Content = string.Empty.GetRandom(),
                ByLine = string.Empty.GetRandom(),
                Slug = string.Empty.GetRandom(),
                PublicationDate = DateTime.MaxValue.GetRandom(DateTime.UtcNow.AddDays(-100)),
                LastModificationDate = DateTime.UtcNow.GetRandom(DateTime.UtcNow.AddDays(-100))
            };
        }

        public static IServiceCollection AddInMemoryContext(this IServiceCollection container)
        {
            return container.AddInMemoryContext(string.Empty.GetRandom());
        }

        public static IServiceCollection AddInMemoryContext(this IServiceCollection container, string dbName)
        {
            return container.AddDbContext<ContentContext>(p => p.UseInMemoryDatabase(databaseName: string.Empty.GetRandom()), ServiceLifetime.Transient);
        }

        #region String Property Test Helpers

        public static void ExecuteContentItemStringPropertyTest(
            this Func<ContentItem, string> getExpectedPropertyValue,
            Func<Entities.ContentItem, string> getActualPropertyValue,
            Action<ContentContext, ContentItem> addData,
            Func<Repository, Entities.ContentItem> getResult)
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            var expectedObject = (null as ContentItem).Create();

            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                addData(dataContext, expectedObject);
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actualEntity = getResult(target);

            var expected = getExpectedPropertyValue(expectedObject);
            var actual = getActualPropertyValue(actualEntity);

            Assert.False(string.IsNullOrWhiteSpace(expected), "Test is invalid if using an empty string");
            Assert.Equal(expected, actual);
        }

        public static void ExecutePostStringPropertyTest(this Func<ContentItem, string> getExpectedPropertyValue, Func<Entities.ContentItem, string> getActualPropertyValue)
        {
            Action<ContentContext, ContentItem> addData = (c, i) => c.Posts.Add(i);
            Func<Repository, Entities.ContentItem> getResult = c => c.GetAllPosts().Single();
            getExpectedPropertyValue.ExecuteContentItemStringPropertyTest(getActualPropertyValue, addData, getResult);
        }

        public static void ExecutePageStringPropertyTest(this Func<ContentItem, string> getExpectedPropertyValue, Func<Entities.ContentItem, string> getActualPropertyValue)
        {
            Action<ContentContext, ContentItem> addData = (c, i) => c.Pages.Add(i);
            Func<Repository, Entities.ContentItem> getResult = c => c.GetAllPages().Single();
            getExpectedPropertyValue.ExecuteContentItemStringPropertyTest(getActualPropertyValue, addData, getResult);
        }

        #endregion

        #region DateTime Property Test Helpers

        public static void ExecuteContentItemDateTimePropertyTest(
            this Func<ContentItem, DateTime> getExpectedPropertyValue,
            Func<Entities.ContentItem, DateTime> getActualPropertyValue,
            Action<ContentContext, ContentItem> addData,
            Func<Repository, Entities.ContentItem> getResult)
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            var expectedObject = (null as ContentItem).Create();

            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                addData(dataContext, expectedObject);
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actualEntity = getResult(target);

            var expected = getExpectedPropertyValue(expectedObject);
            var actual = getActualPropertyValue(actualEntity);

            Assert.True(DateTime.MinValue.CompareTo(expected) < 0, "Test is invalid if using a min value DateTime");
            Assert.Equal(expected, actual);
        }

        public static void ExecutePostDateTimePropertyTest(this Func<ContentItem, DateTime> getExpectedPropertyValue, Func<Entities.ContentItem, DateTime> getActualPropertyValue)
        {
            Action<ContentContext, ContentItem> addData = (c, i) => c.Posts.Add(i);
            Func<Repository, Entities.ContentItem> getResult = c => c.GetAllPosts().Single();
            getExpectedPropertyValue.ExecuteContentItemDateTimePropertyTest(getActualPropertyValue, addData, getResult);
        }

        public static void ExecutePageDateTimePropertyTest(this Func<ContentItem, DateTime> getExpectedPropertyValue, Func<Entities.ContentItem, DateTime> getActualPropertyValue)
        {
            Action<ContentContext, ContentItem> addData = (c, i) => c.Pages.Add(i);
            Func<Repository, Entities.ContentItem> getResult = c => c.GetAllPages().Single();
            getExpectedPropertyValue.ExecuteContentItemDateTimePropertyTest(getActualPropertyValue, addData, getResult);
        }

        #endregion

    }
}
