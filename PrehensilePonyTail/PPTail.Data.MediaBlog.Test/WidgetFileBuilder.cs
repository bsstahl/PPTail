using PPTail.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog.Test
{
    public class WidgetFileBuilder
    {
        readonly List<WidgetZone> _widgets = new List<WidgetZone>();

        public IEnumerable<WidgetZone> Build()
        {
            return _widgets;
        }

        public WidgetFileBuilder AddWidget(WidgetZone widget)
        {
            _widgets.Add(widget);
            return this;
        }

        public WidgetFileBuilder AddTextBoxWidgets(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.AddWidget(new WidgetZoneBuilder()
                    .UseRandom()
                    .WidgetType(WidgetType.TextBox.ToString())
                    .Build());
            }

            return this;
        }

        public WidgetFileBuilder AddTagCloudWidgets(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.AddWidget(new WidgetZoneBuilder()
                    .UseRandom()
                    .WidgetType(WidgetType.Tag_cloud.ToString())
                    .Build());
            }

            return this;
        }

        public WidgetFileBuilder AddTagListWidgets(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.AddWidget(new WidgetZoneBuilder()
                    .UseRandom()
                    .WidgetType(WidgetType.TagList.ToString())
                    .Build());
            }

            return this;
        }

        public WidgetFileBuilder AddRandomWidget()
        {
            return this.AddWidget(new WidgetZoneBuilder()
                .UseRandom()
                .Build());
        }

        public WidgetFileBuilder AddRandomWidgets(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.AddRandomWidget();
            }

            return this;
        }
    }
}
