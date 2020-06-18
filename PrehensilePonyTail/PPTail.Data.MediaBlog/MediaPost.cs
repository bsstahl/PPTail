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

        public String Author { get; set; }
        public String Description { get; set; }
        public String Title { get; set; }
        public DateTime Posted { get; set; }
        public Boolean IsPublished { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public String MediaType { get; set; }
        public JObject MediaItem { get; internal set; }

        public MediaItem Media { get; internal set; }

        internal static MediaPost Create(String json)
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<MediaPost>(json);

            var post = Newtonsoft.Json.Linq.JObject.Parse(json);
            result.MediaItem = post[nameof(MediaPost.MediaItem)].Value<JObject>();
            result.Media = MediaPost.CreateMediaItem(result.MediaType, result.MediaItem);

            return result;
        }

        private static MediaItem CreateMediaItem(string mediaType, JObject mediaItem)
        {
            const string unknownMediaTypeMessage = "Unknown media type";

            MediaItem result = new EmptyMediaItem(string.Empty, 0, 0, DateTime.MinValue);
            if (mediaItem.IsNotNull())
            {
                String mediaTypeName = mediaType ?? "None";

                switch (mediaTypeName.ToUpperInvariant())
                {
                    case "FLICKRIMAGE":
                        result = new FlickrMediaItem(mediaItem);
                        break;

                    case "YOUTUBEVIDEO":
                        result = new YouTubeMediaItem(mediaItem);
                        break;

                    case "NONE":
                        result = new EmptyMediaItem(mediaItem);
                        break;

                    default:
                        throw new InvalidOperationException(unknownMediaTypeMessage);
                }
            }

            return result;
        }

        public Entities.ContentItem AsContentItem(Guid Id)
        {
            var byLine = string.IsNullOrWhiteSpace(this.Author) ? string.Empty : $"{this.Media.MediaTypeName} by {this.Author}";
            var content = this.Media.CreateContent();
            var lastModificationDate = this.Media.CreateDate;
            var slug = this.Title?.CreateSlug();

            return new Entities.ContentItem()
            {
                Author = this.Author,
                ByLine = byLine,
                CategoryIds = Array.Empty<Guid>(), // Removed default category 2020-06-12
                Content = content,
                Description = this.Description ?? string.Empty,
                Id = Id,
                IsPublished = this.IsPublished,
                LastModificationDate = lastModificationDate,
                MenuOrder = 0,
                PublicationDate = this.Posted,
                ShowInList = true,
                Slug = slug,
                Tags = this.Tags,
                Title = this.Title
            };
        }

        public override String ToString() => JsonConvert.SerializeObject(this);
    }
}
