using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    internal class YamlMediaItem
    {
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public String ItemTitle { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public String ItemUrl { get; set; }
        public Boolean IsPublished { get; set; }
        public IEnumerable<ExtendedProperty> ExtendedProperties { get; set; }
    }

    internal class ExtendedProperty
    {
        public String Key { get; set; }
        public String Value { get; set; }
    }
}
