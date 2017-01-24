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
    }
}
