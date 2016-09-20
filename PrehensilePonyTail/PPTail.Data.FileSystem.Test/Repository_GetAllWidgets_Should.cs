using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using Moq;
using System.Linq.Expressions;
using PPTail.Entities;
using System.Xml.Linq;

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_GetAllWidgets_Should
    {
        [Fact]
        public void ReturnAllTextboxWidgets()
        {
            const string rootPath = "c:\\";

            var widgets = (null as IEnumerable<Widget>).Create();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.ConfigureWidgets(widgets, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetAllWidgets();

            Assert.Equal(widgets.Count(w => w.WidgetType != Enumerations.WidgetType.Unknown), actual.Count());
        }

        [Fact]
        public void NotFailIfAnUnknownWidgetTypeIsFound()
        {
            const string rootPath = "c:\\";

            var widgets = (null as IEnumerable<Widget>).Create();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.ConfigureWidgets(widgets, rootPath, true);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetAllWidgets();

            Assert.Equal(widgets.Count(w => w.WidgetType != Enumerations.WidgetType.Unknown), actual.Count());
        }

        [Fact]
        public void NotFailIfASeparateDetailFileIsNotFound()
        {
            const string rootPath = "c:\\";

            var widgets = (null as IEnumerable<Widget>).Create();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.ConfigureWidgets(widgets, rootPath, false);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetAllWidgets();

            Assert.Equal(widgets.Count(), actual.Count());
        }

        //[Fact]
        //public void SkipWidgetsWithInvalidSchema()
        //{
        //}

        //[Fact]
        //public void SkipWidgetsWithTheWrongRootNode()
        //{
        //}

        [Fact]
        public void ReturnTheProperValueInTheIdField()
        {
            ExecutePropertyTest((Widget w) => w.Id.ToString());
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            ExecutePropertyTest((Widget w) => w.Title);
        }

        [Fact]
        public void ReturnTheProperValueInTheShowTitleField()
        {
            ExecutePropertyTest((Widget w) => w.ShowTitle.ToString());
        }

        [Fact]
        public void ReturnTheProperValueInTheWidgetTypeField()
        {
            ExecutePropertyTest((Widget w) => w.WidgetType.ToString());
        }

        [Fact]
        public void ReturnTheProperValueInTheDictionaryKeyField()
        {
            ExecutePropertyTest((Widget w) => w.Dictionary.First().Item1);
        }

        [Fact]
        public void ReturnTheProperValueInTheDictionaryValueField()
        {
            ExecutePropertyTest((Widget w) => w.Dictionary.First().Item2);
        }

        private static void ExecutePropertyTest(Func<Widget, string> fieldValueDelegate)
        {
            const string rootPath = "c:\\";

            var widgets = new List<Widget>() { Enumerations.WidgetType.TextBox.CreateWidget() };
            var fileSystem = new Mock<IFileSystem>();

            fileSystem.ConfigureWidgets(widgets, rootPath);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetAllWidgets();

            Assert.Equal(fieldValueDelegate(widgets.Single()), fieldValueDelegate(actual.Single()));
        }

    }
}
