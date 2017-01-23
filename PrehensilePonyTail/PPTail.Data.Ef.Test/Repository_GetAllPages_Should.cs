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
                dataContext.Pages.Add(new ContentItem() { Id = Guid.NewGuid() });
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPages();
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
                    dataContext.Pages.Add(new ContentItem() { Id = Guid.NewGuid() });
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPages();
            Assert.Equal(itemCount, actual.Count());
        }

        [Fact]
        public void ReturnTheCorrectPageId()
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            var expected = (null as ContentItem).Create();

            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                dataContext.Pages.Add(expected);
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPages();
            Assert.Equal(expected.Id, actual.Single().Id);
        }

        [Fact]
        public void ReturnTheCorrectPageTitle()
        {
            var container = new ServiceCollection();
            container.AddInMemoryContext();
            var serviceProvider = container.BuildServiceProvider();

            var expected = (null as ContentItem).Create();

            using (var dataContext = serviceProvider.GetService<ContentContext>())
            {
                dataContext.Pages.Add(expected);
                dataContext.SaveChanges();
            }

            var target = new Repository(serviceProvider);
            var actual = target.GetAllPages();
            Assert.Equal(expected.Title, actual.Single().Title);
        }

    }
}
