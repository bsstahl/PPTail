using ColorCode.Compilation.Languages;
using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PPTail.Data.MediaBlog
{
    internal class YamlMediaPost
    {
        public Guid Id { get; set; }
        public String Author { get; set; }
        public String Title { get; set; }
        public DateTime Posted { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public Boolean IsPublished { get; set; } = true;

        public String MediaType { get; set; }
        public YamlMediaItem MediaItem { get; set; }


        internal static (MediaPost MediaPost, Guid Id) CreateMediaPost(String fileContents, MarkdownPipeline markdownPipeline)
        {
            var (frontMatter, content) = fileContents.SplitYamlFile();

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(LowerCaseNamingConvention.Instance)
                .Build();

            var yamlMediaPost = deserializer.Deserialize<YamlMediaPost>(frontMatter);
            
            Guid id = yamlMediaPost.Id;
            var mediaPost = new MediaPost()
            {
                Author = yamlMediaPost.Author,
                Description = content.ToHtml(markdownPipeline),
                Media = CreateMediaItem(yamlMediaPost.MediaType, yamlMediaPost.MediaItem),
                MediaType = yamlMediaPost.MediaType,
                Posted = yamlMediaPost.Posted,
                Tags = yamlMediaPost.Tags,
                Title = yamlMediaPost.Title,
                IsPublished = yamlMediaPost.IsPublished
            };

            return (mediaPost, id);
        }

        internal static MediaItem CreateMediaItem(String mediaType, YamlMediaItem mediaItem)
        {
            const string flickrListUrlKey = "FlickrListUrl";

            MediaItem result = null;

            string title = mediaItem.ItemTitle;
            int displayWidth = mediaItem.DisplayWidth;
            int displayHeight = mediaItem.DisplayHeight;
            DateTime createDate = mediaItem.CreateDate.DateTime;
            string itemUrl = mediaItem.ItemUrl;

            String mediaTypeName = mediaType ?? "None";
            switch (mediaTypeName.ToUpperInvariant())
            {
                case "FLICKRIMAGE":
                    var flickrListUrl = mediaItem.ExtendedProperties.SingleOrDefault(p => p.Key == flickrListUrlKey)?.Value;
                    result = new FlickrMediaItem(title, displayWidth, displayHeight, createDate, flickrListUrl, itemUrl);
                    break;

                case "YOUTUBEVIDEO":
                    result = new YouTubeMediaItem(title, displayWidth, displayHeight, createDate, itemUrl);
                    break;

                case "NONE":
                    result = new EmptyMediaItem(title, displayWidth, displayHeight, createDate);
                    break;

                default:
                    string message = $"Unknown media type '{mediaTypeName}'";
                    throw new InvalidOperationException(message);
            }


            return result;
        }


    }
}
