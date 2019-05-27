using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class FlickrMediaItem : MediaItem
    {
        public FlickrMediaItem(JObject json) : base(json)
        {
            this.FlickrListUrl = json["FlickrListUrl"].Value<string>();
            this.ImageUrl = json["ImageUrl"].Value<string>();
        }

        public FlickrMediaItem(string title, int displayWidth, int displayHeight, DateTime createDate, string flickrListUrl, string imageUrl)
            :base(title, displayWidth, displayHeight, createDate)
        {
            this.FlickrListUrl = flickrListUrl;
            this.ImageUrl = imageUrl;
        }

        public string FlickrListUrl { get; set; }
        public string ImageUrl { get; set; }

        public override String CreateContent() => $"<a data-flickr-embed=\"true\" href=\"{this.FlickrListUrl}\" title=\"{this.Title}\"><img class=\"img-responsive\" src=\"{this.ImageUrl}\" alt=\"{this.Title}\"></a>";

    }

}
