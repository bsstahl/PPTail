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

        public MediaItem(String title, Int32 displayWidth, Int32 displayHeight, DateTime createDate)
        {
            this.Title = title;
            this.DisplayWidth = displayWidth;
            this.DisplayHeight = displayHeight;
            this.CreateDate = createDate;
        }

        public String Title { get; set; }
        public Int32 DisplayWidth { get; set; }
        public Int32 DisplayHeight { get; set; }
        public DateTime CreateDate { get; set; }

        public abstract String CreateContent();
    }
}
