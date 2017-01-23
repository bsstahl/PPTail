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
                dataContext.Posts.Add(new ContentItem() { Id = Guid.NewGuid() });
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
                    dataContext.Posts.Add(new ContentItem() { Id = Guid.NewGuid() });
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPosts();
            Assert.Equal(itemCount, actual.Count());
        }

        [Fact]
        public void ReturnTheCorrectPostTitle()
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            var expected = string.Empty.GetRandom();

            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                dataContext.Posts.Add(new ContentItem() { Id = Guid.NewGuid(), Title = expected });
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPosts();
            Assert.Equal(expected, actual.Single().Title);
        }
    }
}
