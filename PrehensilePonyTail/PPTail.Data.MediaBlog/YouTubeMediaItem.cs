using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public override String MediaTypeName => "Video";

        public override String CreateContent()
        {
            string width = (this.DisplayWidth > 0) ? this.DisplayWidth.ToString(CultureInfo.InvariantCulture) : "auto";
            string height = (this.DisplayHeight > 0) ? this.DisplayHeight.ToString(CultureInfo.InvariantCulture) : "auto";

            var sb = new StringBuilder()
                .AppendLine("<div class=\"embed-responsive embed-responsive-16by9\">")
                .AppendLine($"<iframe class=\"embed-responsive-item\" width=\"{width}\" height=\"{height}\" src=\"{this.VideoUrl}\" frameborder=\"0\" allow=\"accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>")
                .AppendLine("</div>")
                .ToString();
            
            return sb.ToString();
        }

    }

}
