using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    internal class YamlWidget
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public bool ShowTitle { get; set; }
        public bool ShowInSidebar { get; set; } = true;
        public byte OrderIndex { get; set; }
        public String WidgetType { get; set; }

        internal Entities.Widget AsEntity()
        {
            return new Entities.Widget()
            {
                Id = this.Id,
                Title = this.Title,
                ShowTitle = this.ShowTitle,
                ShowInSidebar = this.ShowInSidebar,
                OrderIndex = this.OrderIndex,
                WidgetType = Enum.TryParse(this.WidgetType, out Enumerations.WidgetType result) ? result : Enumerations.WidgetType.Unknown
            };
        }
    }
}
