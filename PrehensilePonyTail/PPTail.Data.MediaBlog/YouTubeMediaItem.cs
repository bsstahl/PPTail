using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class YouTubeMediaItem : MediaItem
    {
        public YouTubeMediaItem(JObject json) : base(json)
        {
            this.VideoUrl = json["VideoUrl"].Value<string>();
        }

        public YouTubeMediaItem(String title, Int32 displayWidth, Int32 displayHeight, DateTime createDate, String videoUrl)
            : base(title, displayWidth, displayHeight, createDate)
        {
            this.VideoUrl = videoUrl;
        }

        public String VideoUrl { get; set; }

        public override String CreateContent() => $"<img class=\"img-responsive\"  title=\"{this.Title}\" src=\"{this.VideoUrl}\" alt=\"{this.Title}\" />";

    }

}
