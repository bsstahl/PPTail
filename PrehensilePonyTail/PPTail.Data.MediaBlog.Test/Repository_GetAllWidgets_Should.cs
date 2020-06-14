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
using PPTail.Builders;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_GetAllWidgets_Should
    {
        [Fact]
        public void ReturnAllTextboxWidgets()
        {
            Int32 widgetCount = 20.GetRandom(6);

            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var widgets = new WidgetFileBuilder()
                .AddTextBoxWidgets(widgetCount)
                .Build();

            Int32 expected = widgets.Count(w => w.Active);

            var fileSystem = new MockFileServiceBuilder()
                .AddWidgets(widgets)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build(connectionString);

            var actual = target.GetAllWidgets();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void ReturnAllTagCloudWidgets()
        {
            Int32 widgetCount = 20.GetRandom(6);

            string rootPath = $"c:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var widgets = new WidgetFileBuilder()
                .AddTagCloudWidgets(widgetCount)
                .Build();

            Int32 expected = widgets.Count(w => w.Active);

            var fileSystem = new MockFileServiceBuilder()
                .AddWidgets(widgets)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build(connectionString);

            var actual = target.GetAllWidgets();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void ReturnAllTagListWidgets()
        {
            Int32 widgetCount = 20.GetRandom(6);

            string rootPath = $"C:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var widgets = new WidgetFileBuilder()
                .AddTagListWidgets(widgetCount)
                .Build();

            Int32 expected = widgets.Count(w => w.Active);

            var fileSystem = new MockFileServiceBuilder()
                .AddWidgets(widgets)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build(connectionString);

            var actual = target.GetAllWidgets();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void IgnoreUnknownWidgetTypes()
        {
            Int32 widgetCount = 20.GetRandom(6);

            string rootPath = $"C:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var widgets = new WidgetFileBuilder()
                .AddRandomWidgets(widgetCount)
                .Build();

            Int32 expected = widgets.Count(w => w.Active && w.WidgetType.ToString() != WidgetType.Unknown.ToString());

            var fileSystem = new MockFileServiceBuilder()
                .AddWidgets(widgets)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build(connectionString);

            var actual = target.GetAllWidgets();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void ReturnTheProperValueInTheIdField()
        {
            var widget = new WidgetZoneBuilder()
                .UseRandom(true)
                .UseKnownWidgetType()
                .Build();
            ExecutePropertyTest(widget, widget.Id.ToString(), w => w.Id.ToString());
        }

        [Fact]
        public void ReturnTheProperValueInTheTitleField()
        {
            var widget = new WidgetZoneBuilder()
                .UseRandom(true)
                .UseKnownWidgetType()
                .Build();
            ExecutePropertyTest(widget, widget.Title, w => w.Title);
        }

        [Fact]
        public void ReturnTheProperValueInTheShowTitleField()
        {
            var widget = new WidgetZoneBuilder()
                .UseRandom(true)
                .UseKnownWidgetType()
                .Build();
            ExecutePropertyTest(widget, widget.ShowTitle.ToString(), w => w.ShowTitle.ToString());
        }

        [Fact]
        public void ReturnTheProperValueInTheWidgetTypeField()
        {
            var widget = new WidgetZoneBuilder()
                .UseRandom(true)
                .UseKnownWidgetType()
                .Build();
            ExecutePropertyTest(widget, widget.WidgetType.ToString(), w => w.WidgetType.ToString());
        }

        private static void ExecutePropertyTest(WidgetZone widget, String expected, Func<Widget, string> fieldValueDelegate)
        {
            string rootPath = $"C:\\{string.Empty.GetRandom()}";
            var connectionString = new ConnectionStringBuilder("this")
                    .AddFilePath(rootPath)
                    .Build();

            var fileSystem = new MockFileServiceBuilder()
                .AddWidget(widget)
                .Build(rootPath);

            var target = new ContentRepositoryBuilder()
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build(connectionString);

            var actualWidgets = target.GetAllWidgets();
            var actual = actualWidgets.Single();

            Assert.Equal(expected, fieldValueDelegate(actual));
        }

    }
}
