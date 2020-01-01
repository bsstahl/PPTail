using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Data.Ef.Test
{
    public class Repository_GetAllPosts_Should
    {
        [Fact]
        public void ReturnAnEmptyCollectionIfNoPostsExist()
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPosts();
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
                dataContext.Posts.Add((null as ContentItem).Create());
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPosts();
            Assert.Single(actual);
        }

        [Fact]
        public void ReturnAllItemsIfMultipleItemsExists()
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            Int32 itemCount = 99.GetRandom(5);
            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                for (Int32 i = 0; i < itemCount; i++)
                    dataContext.Posts.Add((null as ContentItem).Create());
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPosts();
            Assert.Equal(itemCount, actual.Count());
        }


        [Fact]
        public void ReturnTheCorrectPostId()
        {
            Func<ContentItem, Guid> getExpectedPropertyValue = i => i.Id;
            Func<Entities.ContentItem, Guid> getActualPropertyValue = i => i.Id;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostTitle()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Title;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Title;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostAuthor()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Author;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Author;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostDescription()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Description;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Description;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostContent()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Content;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Content;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostSlug()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Slug;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Slug;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostByline()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.ByLine;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.ByLine;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostPublicationDate()
        {
            Func<ContentItem, DateTime> getExpectedPropertyValue = i => i.PublicationDate;
            Func<Entities.ContentItem, DateTime> getActualPropertyValue = i => i.PublicationDate;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostLastModificationDate()
        {
            Func<ContentItem, DateTime> getExpectedPropertyValue = i => i.LastModificationDate;
            Func<Entities.ContentItem, DateTime> getActualPropertyValue = i => i.LastModificationDate;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostIsPublishedValue()
        {
            Func<ContentItem, bool> getExpectedPropertyValue = i => i.IsPublished;
            Func<Entities.ContentItem, bool> getActualPropertyValue = i => i.IsPublished;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostShowInListValue()
        {
            Func<ContentItem, bool> getExpectedPropertyValue = i => i.ShowInList;
            Func<Entities.ContentItem, bool> getActualPropertyValue = i => i.ShowInList;
            getExpectedPropertyValue.ExecutePostPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostTagsCollection()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            var expected = expectedObject.Tags;
            Assert.NotNull(expected);

            var expectedCollection = expected.Split(';').OrderBy(t => t).ToArray();
            var actualCollection = actualEntity.Tags.OrderBy(t => t).ToArray();

            Assert.False(string.IsNullOrWhiteSpace(expected), $"Test is invalid if using a null String value");
            Assert.Equal(expectedCollection.Count(), actualCollection.Count());
            for (Int32 i = 0; i < expectedCollection.Count(); i++)
                Assert.Equal(expectedCollection[i], actualCollection[i]);
        }

        [Fact]
        public void ReturnAnEmptyTagCollectionIfThePostHasNoTags()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.Tags = null;
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            Assert.Empty(actualEntity.Tags);
        }

        [Fact]
        public void ReturnAnEmptyTagCollectionIfThePostHasOnlyOneWhitespaceTag()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.Tags = string.Empty;
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            Assert.Empty(actualEntity.Tags);
        }

        [Fact]
        public void ReturnAnEmptyTagCollectionIfThePostHasOnlyWhitespaceTags()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.Tags = "; ;";
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            Assert.Empty(actualEntity.Tags);
        }

        [Fact]
        public void ReturnTheCorrectPostCategoryIdCollection()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            var expected = expectedObject.CategoryIds;
            Assert.NotNull(expected);

            var expectedCollection = expected.Split(';').OrderBy(t => t).ToArray();
            var actualCollection = actualEntity.CategoryIds.OrderBy(t => t).ToArray();

            Assert.False(string.IsNullOrWhiteSpace(expected), $"Test is invalid if using a null String value");
            Assert.Equal(expectedCollection.Count(), actualCollection.Count());
            for (Int32 i = 0; i < expectedCollection.Count(); i++)
                Assert.Equal(new Guid(expectedCollection[i]), actualCollection[i]);
        }

        [Fact]
        public void ReturnAnEmptyCategoryIdCollectionIfThePostHasNoCategory()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.CategoryIds = null;
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            Assert.Empty(actualEntity.CategoryIds);
        }

        [Fact]
        public void ReturnAnEmptyCategoryIdCollectionIfThePostHasOnlyOneWhitespaceCategoryId()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.CategoryIds = string.Empty;
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            Assert.Empty(actualEntity.CategoryIds);
        }

        [Fact]
        public void ReturnAnEmptyCategoryIdCollectionIfThePostHasOnlyWhitespaceCategoryIds()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.CategoryIds = "; ;";
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            Assert.Empty(actualEntity.CategoryIds);
        }

        [Fact]
        public void ReturnOnlyValidGuidsAsPostCategoryIds()
        {
            var serviceProvider = (null as IServiceProvider).Create();

            var expectedObject = (null as ContentItem).Create();
            expectedObject.CategoryIds = $";{string.Empty.GetRandom()};{Guid.NewGuid()}";
            expectedObject.AddToDataStore((c, i) => c.Posts.Add(i), serviceProvider);

            var target = new Repository(serviceProvider);
            var actualEntity = target.GetAllPosts().Single();

            Assert.Single(actualEntity.CategoryIds);
        }
    }
}
