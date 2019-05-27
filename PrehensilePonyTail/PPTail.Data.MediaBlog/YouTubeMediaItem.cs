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

        public YouTubeMediaItem(string title, int displayWidth, int displayHeight, DateTime createDate, string videoUrl)
            : base(title, displayWidth, displayHeight, createDate)
        {
            this.VideoUrl = videoUrl;
        }

        public string VideoUrl { get; set; }

        public override String CreateContent() => $"<img class=\"img-responsive\"  title=\"{this.Title}\" src=\"{this.VideoUrl}\" alt=\"{this.Title}\" />";

    }

}
