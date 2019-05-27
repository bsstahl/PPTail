﻿using Newtonsoft.Json.Linq;
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
        public string Build()
        {
            return this.ToString();
        }

        public new MediaPostBuilder Author(string author)
        {
            base.Author = author;
            return this;
        }

        public new MediaPostBuilder Description(string description)
        {
            base.Description = description;
            return this;
        }

        public new MediaPostBuilder Posted(DateTime posted)
        {
            base.Posted = posted;
            return this;
        }

        public new MediaPostBuilder Title(string title)
        {
            base.Title = title;
            return this;
        }

        public MediaPostBuilder ClearTags()
        {
            base.Tags = null;
            return this;
        }

        public MediaPostBuilder AddTag(string tag)
        {
            return this.AddTags(new string[] { tag });
        }

        public MediaPostBuilder AddTags(IEnumerable<string> tags)
        {
            var newTags = new List<string>();

            if (base.Tags != null && base.Tags.Any())
                newTags.AddRange(base.Tags);

            if (tags != null && tags.Any())
                newTags.AddRange(tags);

            if (newTags.Any())
                base.Tags = newTags;

            return this;
        }

        public MediaPostBuilder AddFlickrImage(string title, int displayWidth, int displayHeight, DateTime createDate, string flickrListUrl, string imageUrl)
        {
            return this.AddFlickrImage(new FlickrMediaItem(title, displayWidth, displayHeight, createDate, flickrListUrl, imageUrl));
        }

        public MediaPostBuilder AddFlickrImage(FlickrMediaItem flickrImage)
        {
            this.MediaItem = JObject.FromObject(flickrImage);
            this.MediaType = "FlickrImage";
            return this;
        }

        public MediaPostBuilder AddYouTubeVideo(string title, int displayWidth, int displayHeight, DateTime createDate, string videoUrl)
        {
            return this.AddYouTubeVideo(new YouTubeMediaItem(title, displayWidth, displayHeight, createDate, videoUrl));
        }

        public MediaPostBuilder AddYouTubeVideo(YouTubeMediaItem video)
        {
            this.MediaItem = JObject.FromObject(video);
            this.MediaType = "YouTubeVideo";
            return this;
        }

        public MediaPostBuilder AddEmptyPost(string title, int displayWidth, int displayHeight, DateTime createDate)
        {
            return this.AddEmptyPost(new EmptyMediaItem(title,displayWidth, displayHeight, createDate));
        }

        public MediaPostBuilder AddEmptyPost(EmptyMediaItem item)
        {
            this.MediaItem = JObject.FromObject(item);
            this.MediaType = "None";
            return this;
        }
    }
}
