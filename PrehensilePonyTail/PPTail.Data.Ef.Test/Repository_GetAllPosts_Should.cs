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
            Assert.Equal(1, actual.Count());
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
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            var expected = (null as ContentItem).Create();

            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                dataContext.Posts.Add(expected);
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPosts();

            Debug.Assert(Guid.Empty.CompareTo(expected.Id) != 0);
            Assert.Equal(expected.Id, actual.Single().Id);
        }

        [Fact]
        public void ReturnTheCorrectPostTitle()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Title;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Title;
            getExpectedPropertyValue.ExecutePostStringPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostAuthor()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Author;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Author;
            getExpectedPropertyValue.ExecutePostStringPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostDescription()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Description;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Description;
            getExpectedPropertyValue.ExecutePostStringPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostContent()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Content;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Content;
            getExpectedPropertyValue.ExecutePostStringPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostSlug()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.Slug;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.Slug;
            getExpectedPropertyValue.ExecutePostStringPropertyTest(getActualPropertyValue);
        }

        [Fact]
        public void ReturnTheCorrectPostByline()
        {
            Func<ContentItem, string> getExpectedPropertyValue = i => i.ByLine;
            Func<Entities.ContentItem, string> getActualPropertyValue = i => i.ByLine;
            getExpectedPropertyValue.ExecutePostStringPropertyTest(getActualPropertyValue);
        }
    }
}
