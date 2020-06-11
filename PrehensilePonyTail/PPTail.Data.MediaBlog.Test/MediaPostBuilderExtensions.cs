using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class MediaPostBuilderExtensions
    {
        const Int32 _maxTags = 30;

        public static MediaPostBuilder UseRandomFlickrPost(this MediaPostBuilder builder)
        {
            var tags = new List<string>();
            for (Int32 i = 0; i < _maxTags.GetRandom(); i++)
            {
                tags.Add(string.Empty.GetRandom());
            }

            var flickrImage = new FlickrMediaItemBuilder()
                .UseRandom()
                .Build();

            return builder
                .AddFlickrImage(flickrImage)
                .Author(string.Empty.GetRandom())
                .Description(string.Empty.GetRandom())
                .Posted(DateTime.Now.AddYears(30).GetRandom(DateTime.Now.AddYears(-10)))
                .AddTags(tags)
                .Title(string.Empty.GetRandom());
        }

        public static MediaPostBuilder UseRandomYouTubePost(this MediaPostBuilder builder)
        {
            var tags = new List<string>();
            for (Int32 i = 0; i < _maxTags.GetRandom(); i++)
            {
                tags.Add(string.Empty.GetRandom());
            }

            var youTubeVideo = new YouTubeMediaItemBuilder()
                .UseRandom()
                .Build();

            return builder
                .AddYouTubeVideo(youTubeVideo)
                .Author(string.Empty.GetRandom())
                .Description(string.Empty.GetRandom())
                .Posted(DateTime.Now.AddYears(30).GetRandom(DateTime.Now.AddYears(-10)))
                .AddTags(tags)
                .Title(string.Empty.GetRandom());
        }

        public static MediaPostBuilder UseRandomEmptyPost(this MediaPostBuilder builder)
        {
            var tags = new List<string>();
            for (Int32 i = 0; i < _maxTags.GetRandom(); i++)
            {
                tags.Add(string.Empty.GetRandom());
            }

            var emptyPost = new EmptyMediaItemBuilder()
                .UseRandom()
                .Build();

            return builder
                .AddEmptyPost(emptyPost)
                .Author(string.Empty.GetRandom())
                .Description(string.Empty.GetRandom())
                .Posted(DateTime.Now.AddYears(30).GetRandom(DateTime.Now.AddYears(-10)))
                .AddTags(tags)
                .Title(string.Empty.GetRandom());
        }
    }
}
