using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class YouTubeMediaItemBuilderExtensions
    {
        public static YouTubeMediaItemBuilder UseRandom(this YouTubeMediaItemBuilder builder)
        {
            return builder
                .Title(string.Empty.GetRandom())
                .DisplayWidth(4096.GetRandom(600))
                .DisplayHeight(3072.GetRandom(240))
                .CreateDate(DateTime.Now.AddYears(30).GetRandom(DateTime.Now.AddYears(-10)))
                .VideoUrl(string.Empty.GetRandomUrl());
        }

    }
}
