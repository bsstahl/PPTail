using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    internal class FlickrMediaItem : MediaItem
    {
        public FlickrMediaItem(JObject json) : base(json)
        {
            this.FlickrListUrl = json["FlickrListUrl"].Value<string>();
            this.ImageUrl = json["ImageUrl"].Value<string>();
        }

        public string FlickrListUrl { get; set; }
        public string ImageUrl { get; set; }

        public override String CreateContent() => $"<a data-flickr-embed=\"true\" href=\"{this.FlickrListUrl}\" title=\"{this.Title}\"><img class=\"img-responsive\" src=\"{this.ImageUrl}\" alt=\"{this.Title}\"></a>";

        //private static FlickrMediaItem Create(string json)
        //{
        //    return JsonConvert.DeserializeObject<FlickrMediaItem>(json);
        //}
    }

}
