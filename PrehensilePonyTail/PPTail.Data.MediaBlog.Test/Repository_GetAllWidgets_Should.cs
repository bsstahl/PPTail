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
    public class Repository_GetAllWidgets_Should
    {
        [Fact]
        public void ReturnAllTextboxWidgets()
        {
            int widgetCount = 20.GetRandom(6);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var widgets = new WidgetFileBuilder()
                .AddTextBoxWidgets(widgetCount)
                .Build();

            int expected = widgets.Count(w => w.Active);

            var fileSystem = new MockFileServiceBuilder()
                .AddWidgets(widgets)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

            var actual = target.GetAllWidgets();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void ReturnAllTagCloudWidgets()
        {
            int widgetCount = 20.GetRandom(6);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var widgets = new WidgetFileBuilder()
                .AddTagCloudWidgets(widgetCount)
                .Build();

            int expected = widgets.Count(w => w.Active);

            var fileSystem = new MockFileServiceBuilder()
                .AddWidgets(widgets)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

            var actual = target.GetAllWidgets();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void ReturnAllTagListWidgets()
        {
            int widgetCount = 20.GetRandom(6);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var widgets = new WidgetFileBuilder()
                .AddTagListWidgets(widgetCount)
                .Build();

            int expected = widgets.Count(w => w.Active);

            var fileSystem = new MockFileServiceBuilder()
                .AddWidgets(widgets)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

            var actual = target.GetAllWidgets();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void IgnoreUnknownWidgetTypes()
        {
            int widgetCount = 20.GetRandom(6);

            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var widgets = new WidgetFileBuilder()
                .AddRandomWidgets(widgetCount)
                .Build();

            int expected = widgets.Count(w => w.Active && w.WidgetType.ToString() != WidgetType.Unknown.ToString());

            var fileSystem = new MockFileServiceBuilder()
                .AddWidgets(widgets)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

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

        private static void ExecutePropertyTest(WidgetZone widget, string expected, Func<Widget, string> fieldValueDelegate)
        {
            var settings = new SettingsBuilder()
                .UseGenericValues()
                .Build();

            var fileSystem = new MockFileServiceBuilder()
                .AddWidget(widget)
                .Build();

            var target = new ContentRepositoryBuilder()
                .AddSettingsService(settings)
                .AddFileService(fileSystem.Object)
                .UseGenericDirectory()
                .Build();

            var actualWidgets = target.GetAllWidgets();
            var actual = actualWidgets.Single();

            Assert.Equal(expected, fieldValueDelegate(actual));
        }

    }
}
