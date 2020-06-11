using PPTail.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class WidgetZoneBuilder: WidgetZone
    {
        internal WidgetZone Build()
        {
            return this;
        }

        public new WidgetZoneBuilder Active(bool active)
        {
            base.Active = active;
            return this;
        }

        public new WidgetZoneBuilder Content(String content)
        {
            base.Content = content;
            return this;
        }

        public new WidgetZoneBuilder Id(String id)
        {
            base.Id = id;
            return this;
        }

        public new WidgetZoneBuilder ShowTitle(bool showTitle)
        {
            base.ShowTitle = showTitle;
            return this;
        }

        public new WidgetZoneBuilder Title(String title)
        {
            base.Title = title;
            return this;
        }

        public new WidgetZoneBuilder WidgetType(String widgetType)
        {
            base.WidgetType = widgetType;
            return this;
        }

        public WidgetZoneBuilder UseKnownWidgetType()
        {
            var widgetTypeNames = new string[]
            {
                Enumerations.WidgetType.TagList.ToString(),
                Enumerations.WidgetType.Tag_cloud.ToString(),
                Enumerations.WidgetType.TextBox.ToString()
            };
            base.WidgetType = widgetTypeNames.GetRandom();
            return this;
        }

        public WidgetZoneBuilder UseRandom(bool active)
        {
            var widgetTypeNames = Enum.GetNames(typeof(Enumerations.WidgetType));
            return this.AddWidget(
                Guid.NewGuid(),
                active,
                string.Empty.GetRandom(),
                true.GetRandom(),
                string.Empty.GetRandom(),
                widgetTypeNames.GetRandom());
        }

        public WidgetZoneBuilder UseRandom()
        {
            var widgetTypeNames = Enum.GetNames(typeof(Enumerations.WidgetType));
            return this.AddWidget(
                Guid.NewGuid(),
                true.GetRandom(),
                string.Empty.GetRandom(),
                true.GetRandom(),
                string.Empty.GetRandom(),
                widgetTypeNames.GetRandom());
        }

        public WidgetZoneBuilder AddWidget(Guid id,
            bool active,
            String content,
            bool showTitle,
            String title,
            String widgetTypeName)
        {
            return this
                .Active(active)
                .Content(content)
                .Id(id.ToString())
                .ShowTitle(showTitle)
                .Title(title)
                .WidgetType(widgetTypeName);
        }
    }
}
