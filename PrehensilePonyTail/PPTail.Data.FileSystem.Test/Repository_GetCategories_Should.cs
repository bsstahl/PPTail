using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_GetCategories_Should
    {
        const string _dataFolder = "App_Data";

        [Fact]
        public void ReturnAllCategories()
        {
            const string rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create();

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Count(), actual.Count());
        }

        [Fact]
        public void ReturnTheProperIdForACategory()
        {
            const string rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create(1);

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Single().Id, actual.Single().Id);
        }

        [Fact]
        public void ReturnTheProperNameForACategory()
        {
            const string rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create(1);

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Single().Name, actual.Single().Name);
        }

        [Fact]
        public void ReturnTheProperDescriptionForACategory()
        {
            const string rootPath = "c:\\";

            var categories = (null as IEnumerable<Category>).Create(1);

            var fileSystem = new Mock<IFile>();
            fileSystem.ConfigureCategories(categories, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetCategories();

            Assert.Equal(categories.Single().Description, actual.Single().Description);
        }
    }
}
