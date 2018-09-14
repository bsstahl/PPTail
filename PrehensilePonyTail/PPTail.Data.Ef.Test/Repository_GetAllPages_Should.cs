using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Data.Ef.Test
{
    public class Repository_GetAllPages_Should
    {
        [Fact]
        public void ReturnAnEmptyCollectionIfNoPagesExist()
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPages();
            Assert.Empty(actual);
        }

        [Fact]
        public void ReturnOneItemIfOnlyOneItemExists()
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                dataContext.Pages.Add((null as ContentItem).Create());
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPages();
            Assert.Single(actual);
        }

        [Fact]
        public void ReturnAllItemsIfMultipleItemsExists()
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            int itemCount = 99.GetRandom(5);
            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                for (int i = 0; i < itemCount; i++)
                    dataContext.Pages.Add((null as ContentItem).Create());
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPages();
            Assert.Equal(itemCount, actual.Count());
        }

        [Fact]
        public void ReturnTheCorrectPageId()
        {
            Func<ContentItem, Guid> getExpectedPropertyValue = i => i.Id;
            Func<Entities.ContentItem, Guid> getActualPropertyValue = i => i.Id;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageTitle()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Title;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Title;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageAuthor()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Author;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Author;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageDescription()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Description;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Description;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageContent()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Content;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Content;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageSlug()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Slug;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Slug;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageByline()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.ByLine;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.ByLine;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPagePublicationDate()
        {
            Func<ContentItem, DateTime> getExpectedPropertyValue = i => i.PublicationDate;
            Func<Entities.ContentItem, DateTime> getActualPropertyValue = i => i.PublicationDate;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageLastModificationDate()
        {
            Func<ContentItem, DateTime> getExpectedPropertyValue = i => i.LastModificationDate;
            Func<Entities.ContentItem, DateTime> getActualPropertyValue = i => i.LastModificationDate;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageIsPublishedValue()
        {
            Func<ContentItem, bool> getExpectedPropertyValue = i => i.IsPublished;
            Func<Entities.ContentItem, bool> getActualPropertyValue = i => i.IsPublished;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageShowInListValue()
        {
            Func<ContentItem, bool> getExpectedPropertyValue = i => i.ShowInList;
            Func<Entities.ContentItem, bool> getActualPropertyValue = i => i.ShowInList;
            getExpectedPropertyValue.ExecutePagePropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPageTagsCollection()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.AddToDataStore((c,i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            var expected = expectedObject.Tags;
            Assert.NotNull(expected);

            var expectedCollection = expected.Split(';').OrderBy(t => t).ToArray();
            var actualCollection = actualEntity.Tags.OrderBy(t => t).ToArray();

            Assert.False(string.IsNullOrWhiteSpace(expected), $"Test is invalid if using a null string value");
            Assert.Equal(expectedCollection.Count(), actualCollection.Count());
            for (int i = 0; i < expectedCollection.Count(); i++)
                Assert.Equal(expectedCollection[i], actualCollection[i]);
        }

        [Fact]
        public void ReturnAnEmptyTagCollectionIfThePageHasNoTags()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.Tags = null;
            expectedObject.AddToDataStore((c, i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            Assert.Empty(actualEntity.Tags);
        }

        [Fact]
        public void ReturnAnEmptyTagCollectionIfThePageHasOnlyOneWhitespaceTag()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.Tags = string.Empty;
            expectedObject.AddToDataStore((c, i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            Assert.Empty(actualEntity.Tags);
        }

        [Fact]
        public void ReturnAnEmptyTagCollectionIfThePageHasOnlyWhitespaceTags()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.Tags = "; ;";
            expectedObject.AddToDataStore((c, i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            Assert.Empty(actualEntity.Tags);
        }

        [Fact]
        public void ReturnTheCorrectPageCategoryIdCollection()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.AddToDataStore((c, i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            var expected = expectedObject.CategoryIds;
            Assert.NotNull(expected);

            var expectedCollection = expected.Split(';').OrderBy(t => t).ToArray();
            var actualCollection = actualEntity.CategoryIds.OrderBy(t => t).ToArray();

            Assert.False(string.IsNullOrWhiteSpace(expected), $"Test is invalid if using a null string value");
            Assert.Equal(expectedCollection.Count(), actualCollection.Count());
            for (int i = 0; i < expectedCollection.Count(); i++)
                Assert.Equal(new Guid(expectedCollection[i]), actualCollection[i]);
        }

        [Fact]
        public void ReturnAnEmptyCategoryIdCollectionIfThePageHasNoCategory()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.CategoryIds = null;
            expectedObject.AddToDataStore((c, i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            Assert.Empty(actualEntity.CategoryIds);
        }

        [Fact]
        public void ReturnAnEmptyCategoryIdCollectionIfThePageHasOnlyOneWhitespaceCategoryId()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.CategoryIds = string.Empty;
            expectedObject.AddToDataStore((c, i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            Assert.Empty(actualEntity.CategoryIds);
        }

        [Fact]
        public void ReturnAnEmptyCategoryIdCollectionIfThePageHasOnlyWhitespaceCategoryIds()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.CategoryIds = "; ;";
            expectedObject.AddToDataStore((c, i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            Assert.Empty(actualEntity.CategoryIds);
        }

        [Fact]
        public void ReturnOnlyValidGuidsAsPageCategoryIds()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.CategoryIds = $";{string.Empty.GetRandom()};{Guid.NewGuid()}";
            expectedObject.AddToDataStore((c, i) => c.Pages.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPages().Single();

            Assert.Single(actualEntity.CategoryIds);
        }
    }
}
