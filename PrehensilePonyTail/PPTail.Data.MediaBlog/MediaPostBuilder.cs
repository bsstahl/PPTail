using Newtonsoft.Json.Linq;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    /// <summary>
    /// Creates a serialized form of the MediaPost object
    /// that matches the disk format
    /// </summary>
    public class MediaPostBuilder : PPTail.Data.MediaBlog.MediaPost
    {
        public String Build()
        {
            return this.ToString();
        }

        public new MediaPostBuilder Author(String author)
        {
            base.Author = author;
            return this;
        }

        public new MediaPostBuilder Description(String description)
        {
            base.Description = description;
            return this;
        }

        public new MediaPostBuilder Posted(DateTime posted)
        {
            base.Posted = posted;
            return this;
        }

        public new MediaPostBuilder Title(String title)
        {
            base.Title = title;
            return this;
        }

        public MediaPostBuilder ClearTags()
        {
            base.Tags = null;
            return this;
        }

        public MediaPostBuilder AddTag(String tag)
        {
            return this.AddTags(new string[] { tag });
        }

        public MediaPostBuilder AddTags(IEnumerable<string> tags)
        {
            var newTags = new List<string>();

            if (base.Tags.IsNotNull() && base.Tags.Any())
                newTags.AddRange(base.Tags);

            if (tags.IsNotNull() && tags.Any())
                newTags.AddRange(tags);

            if (newTags.Any())
                base.Tags = newTags;

            return this;
        }

        public MediaPostBuilder AddFlickrImage(String title, Int32 displayWidth, Int32 displayHeight, DateTime createDate, String flickrListUrl, String imageUrl)
        {
            return this.AddFlickrImage(new FlickrMediaItem(title, displayWidth, displayHeight, createDate, flickrListUrl, imageUrl));
        }

        public MediaPostBuilder AddFlickrImage(FlickrMediaItem flickrImage)
        {
            base.MediaItem = JObject.FromObject(flickrImage);
            base.MediaType = "FlickrImage";
            return this;
        }

        public MediaPostBuilder AddYouTubeVideo(String title, Int32 displayWidth, Int32 displayHeight, DateTime createDate, String videoUrl)
        {
            return this.AddYouTubeVideo(new YouTubeMediaItem(title, displayWidth, displayHeight, createDate, videoUrl));
        }

        public MediaPostBuilder AddYouTubeVideo(YouTubeMediaItem video)
        {
            base.MediaItem = JObject.FromObject(video);
            base.MediaType = "YouTubeVideo";
            return this;
        }

        public MediaPostBuilder AddEmptyPost(String title, Int32 displayWidth, Int32 displayHeight, DateTime createDate)
        {
            return this.AddEmptyPost(new EmptyMediaItem(title,displayWidth, displayHeight, createDate));
        }

        public MediaPostBuilder AddEmptyPost(EmptyMediaItem item)
        {
            base.MediaItem = JObject.FromObject(item);
            base.MediaType = "None";
            return this;
        }

        public new MediaPostBuilder MediaItem(JObject value)
        {
            base.MediaItem = value;
            return this;
        }

        public new MediaPostBuilder MediaType(string value)
        {
            base.MediaType = value;
            return this;
        }
    }
}
