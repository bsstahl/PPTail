using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public abstract class MediaItem
    {
        public MediaItem(JObject json)
            :this(json["Title"].Value<string>(), json["DisplayWidth"].Value<int>(), json["DisplayHeight"].Value<int>(), json["CreateDate"].Value<DateTime>())
        {}

        public MediaItem(string title, int displayWidth, int displayHeight, DateTime createDate)
        {
            this.Title = title;
            this.DisplayWidth = displayWidth;
            this.DisplayHeight = displayHeight;
            this.CreateDate = createDate;
        }

        public string Title { get; set; }
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public DateTime CreateDate { get; set; }

        public abstract string CreateContent();
    }
}
