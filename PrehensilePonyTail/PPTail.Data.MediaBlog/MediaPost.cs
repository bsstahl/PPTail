using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PPTail.Extensions;

namespace PPTail.Data.MediaBlog
{
    public class MediaPost
    {

        public string Author { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Posted { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public string MediaType { get; set; }
        public JObject MediaItem { get; internal set; }

        public MediaItem Media { get; private set; }

        internal static MediaPost Create(string json)
        {
            var post = Newtonsoft.Json.Linq.JObject.Parse(json);
            var posted = post["Posted"];

            var mediaItem = post["MediaItem"]?.Value<JObject>();
            var result = new MediaPost()
            {
                Author = post["Author"]?.Value<string>(),
                Description = post["Description"]?.Value<string>(),
                MediaType = post["MediaType"]?.Value<string>(),
                MediaItem = mediaItem,
                Posted = (posted == null) ? DateTime.MinValue : posted.Value<DateTime>(),
                Tags = post["Tags"]?.Select(t => t.Value<string>()),
                Title = post["Title"]?.Value<string>()
            };
            result.Media = result.CreateMediaItem();

            return result;
        }

        private MediaItem CreateMediaItem()
        {
            MediaItem result = null;
            string mediaTypeName = this.MediaType ?? "None";

            switch (mediaTypeName.ToLower())
            {
                case "flickrimage":
                    result = new FlickrMediaItem(this.MediaItem);
                    break;

                case "youtubevideo":
                    result = new YouTubeMediaItem(this.MediaItem);
                    break;

                case "none":
                    result = new EmptyMediaItem(this.MediaItem);
                    break;

                default:
                    throw new InvalidOperationException("Unknown media type");
            }

            return result;
        }

        public Entities.ContentItem AsContentItem()
        {
            return this.AsContentItem(Guid.NewGuid());
        }

        public Entities.ContentItem AsContentItem(Guid Id)
        {
            return new Entities.ContentItem()
            {
                Author = this.Author,
                ByLine = string.IsNullOrWhiteSpace(this.Author) ? string.Empty : $"by {this.Author}",
                CategoryIds = new Guid[] { Guid.Parse("663D2D20-6B79-47B1-AFAD-615F15E226A7") },
                Content = this.Media?.CreateContent(),
                Description = this.Description,
                Id = Id,
                IsPublished = true,
                LastModificationDate = this.Media?.CreateDate ?? DateTime.MinValue,
                MenuOrder = 0,
                PublicationDate = this.Posted,
                ShowInList = true,
                Slug = this.Title?.CreateSlug(),
                Tags = this.Tags,
                Title = this.Title
            };
        }

        public override String ToString() => JsonConvert.SerializeObject(this);
    }
}
