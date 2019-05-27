using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class WidgetZone
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool ShowTitle { get; set; }
        public string WidgetType { get; set; }
        public string Content { get; set; }
        public bool Active { get; set; }
    }
}
