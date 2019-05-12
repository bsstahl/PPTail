using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public abstract class MediaItem
    {
        public MediaItem(JObject json)
        {
            this.Title = json["Title"].Value<string>();
            this.DisplayWidth = json["DisplayWidth"].Value<int>();
            this.DisplayHeight = json["DisplayHeight"].Value<int>();
            this.CreateDate = json["CreateDate"].Value<DateTime>();
        }

        public string Title { get; set; }
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public DateTime CreateDate { get; set; }

        public abstract string CreateContent();
    }
}
