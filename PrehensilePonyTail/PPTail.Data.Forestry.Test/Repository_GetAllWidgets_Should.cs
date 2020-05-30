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
using PPTail.Enumerations;
using System.Runtime.CompilerServices;

namespace PPTail.Data.Forestry.Test
{
    public class Repository_GetAllWidgets_Should
    {
        const String _rootPath = "c:\\";


        [Fact]
        public void ReturnAllTextboxWidgets()
        {
            ExecuteWidgetTypeCountTest(WidgetType.TextBox);
        }

        [Fact]
        public void ReturnAllTagCloudWidgets()
        {
            ExecuteWidgetTypeCountTest(WidgetType.Tag_cloud);
        }

        [Fact]
        public void ReturnAllTagListWidgets()
        {
            ExecuteWidgetTypeCountTest(WidgetType.TagList);
        }

        [Fact]
        public void NotFailIfAnUnknownWidgetTypeIsFound()
        {
            var fileSystem = new Mock<IFile>();
            var directory = new Mock<IDirectory>();
            var widgets = fileSystem.ConfigureWidgets(directory, _rootPath, true);

            var target = (null as IContentRepository).Create(fileSystem.Object, directory.Object, _rootPath);
            var actual = target.GetAllWidgets();

            // var knownWidgetTypes = (int[])Enum.GetValues(typeof(WidgetType));
            // var expectedCount = widgets.Count(w => knownWidgetTypes.Contains((int)w.WidgetType) && w.WidgetType != WidgetType.Unknown);
            
            var expectedCount = widgets.Count();
            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public void ReturnTheProperValueInTheIdField()
        {
            ExecutePropertyTest((Widget w) => w.Id);
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            ExecutePropertyTest((Widget w) => w.Title);
        }

        [Fact]
        public void ReturnTheProperValueInTheShowTitleFieldIfEnabled()
        {
            var widget = Enumerations.WidgetType.TextBox.CreateWidget();
            widget.ShowTitle = true;
            ExecutePropertyTest((Widget w) => w.ShowTitle);
        }

        [Fact]
        public void ReturnTheProperValueInTheShowTitleFieldIfDisabled()
        {
            var widget = Enumerations.WidgetType.TextBox.CreateWidget();
            widget.ShowTitle = false;
            ExecutePropertyTest((Widget w) => w.ShowTitle);
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
            var widget = Enumerations.WidgetType.TextBox.CreateWidget();
            String expectedValue = $"<p>{widget.Dictionary.First().Item2}</p>";
            Func<Widget, String> actualValueDelegate = (Widget w) => w.Dictionary.First().Item2.Trim();
            ExecutePropertyTest(widget, expectedValue, actualValueDelegate);
        }

        private static void ExecuteWidgetTypeCountTest(WidgetType widgetType)
        {
            var fileSystem = new Mock<IFile>();
            var directory = new Mock<IDirectory>();
            var widgets = fileSystem.ConfigureWidgets(directory, _rootPath, false);

            var target = (null as IContentRepository).Create(fileSystem.Object, directory.Object, _rootPath);
            var actual = target.GetAllWidgets();

            Func<Widget, bool> predicate = w => w.WidgetType == widgetType;
            var expectedCount = widgets.Count(predicate);
            var actualCount = actual.Count(predicate);

            Assert.Equal(expectedCount, actualCount);
        }

        private static void ExecutePropertyTest<T>(Func<Widget, T> fieldValueDelegate)
        {
            ExecutePropertyTest(Enumerations.WidgetType.TextBox.CreateWidget(), fieldValueDelegate);
        }

        private static void ExecutePropertyTest<T>(Widget widget, Func<Widget, T> fieldValueDelegate)
        {
            ExecutePropertyTest(widget, fieldValueDelegate(widget), fieldValueDelegate);
        }

        private static void ExecutePropertyTest<T>(Widget widget, T expectedValue, Func<Widget, T> actualValueDelegate)
        {
            var fileSystem = new Mock<IFile>();
            var directory = new Mock<IDirectory>();
            var widgets = fileSystem.ConfigureWidgets(directory, new[] { widget }, _rootPath, false);

            var target = (null as IContentRepository).Create(fileSystem.Object, directory.Object, _rootPath);
            var actual = target.GetAllWidgets();

            Assert.Equal(expectedValue, actualValueDelegate(actual.Single()));
        }

    }
}
