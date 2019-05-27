using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class EmptyMediaItem : MediaItem
    {
        public EmptyMediaItem(JObject json)
            :base(json)
        { }

        public EmptyMediaItem(string title, int displayWidth, int displayHeight, DateTime createDate)
            :base(title, displayWidth, displayHeight, createDate)
        { }

        public override String CreateContent() => string.Empty;
    }
}
