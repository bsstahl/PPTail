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
                LastModificationDate = DateTime.UtcNow.GetRandom(DateTime.UtcNow.AddDays(-100)),
                IsPublished = true,
                ShowInList = true,
                Tags = string.Empty.CreateTagList(),
                CategoryIds = string.Empty.CreateCategoryIdList()
            };
        }

        public static IServiceCollection AddInMemoryContext(this IServiceCollection container)
        {
            return container.AddInMemoryContext(string.Empty.GetRandom());
        }

        public static IServiceCollection AddInMemoryContext(this IServiceCollection container, String dbName)
        {
            return container.AddDbContext<ContentContext>(p => p.UseInMemoryDatabase(databaseName: string.Empty.GetRandom()), ServiceLifetime.Transient);
        }

        public static String CreateTagList(this String ignore)
        {
            var tags = new List<string>();
            Int32 tagCount = 7.GetRandom(3);
            for (Int32 i = 0; i < tagCount; i++)
                tags.Add(string.Empty.GetRandom().Replace(";", ""));
            return string.Join(";", tags);
        }

        public static String CreateCategoryIdList(this String ignore)
        {
            var resultList = new List<string>();
            Int32 count = 7.GetRandom(3);
            for (Int32 i = 0; i < count; i++)
                resultList.Add(Guid.NewGuid().ToString());
            return string.Join(";", resultList);
        }

        public static IServiceProvider Create(this IServiceProvider ignore)
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            return container.BuildServiceProvider();
        }

        public static void AddToDataStore(this ContentItem expectedObject, Action<ContentContext, ContentItem> addData, IServiceProvider serviceProvider)
        {
            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                addData(dataContext, expectedObject);
                dataContext.SaveChanges();
            }
        }

        #region Property Test Helpers

        public static void ExecuteContentItemPropertyTest<T>(
            this Func<ContentItem, T> getExpectedPropertyValue,
            Func<Entities.ContentItem, T> getActualPropertyValue,
            Action<ContentContext, ContentItem> addData,
            Func<Repository, Entities.ContentItem> getResult)
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.AddToDataStore(addData, serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = getResult(target);

            var expected = getExpectedPropertyValue(expectedObject);
            var actual = getActualPropertyValue(actualEntity);

            Assert.False(expected == null, $"Test is invalid if using a null {typeof(T).Name} value");
            Assert.False(expected.Equals(default(T)), $"Test is invalid if using a default {typeof(T).Name} value");
            Assert.Equal(expected, actual);
        }

        public static void ExecutePostPropertyTest<T>(this Func<ContentItem, T> getExpectedPropertyValue, Func<Entities.ContentItem, T> getActualPropertyValue)
        {
            Action<ContentContext, ContentItem> addData = (c, i) => c.Posts.Add(i);
            Func<Repository, Entities.ContentItem> getResult = c => c.GetAllPosts().Single();
            getExpectedPropertyValue.ExecuteContentItemPropertyTest(getActualPropertyValue, addData, getResult);
        }

        public static void ExecutePagePropertyTest<T>(this Func<ContentItem, T> getExpectedPropertyValue, Func<Entities.ContentItem, T> getActualPropertyValue)
        {
            Action<ContentContext, ContentItem> addData = (c, i) => c.Pages.Add(i);
            Func<Repository, Entities.ContentItem> getResult = c => c.GetAllPages().Single();
            getExpectedPropertyValue.ExecuteContentItemPropertyTest(getActualPropertyValue, addData, getResult);
        }

        #endregion

    }
}
