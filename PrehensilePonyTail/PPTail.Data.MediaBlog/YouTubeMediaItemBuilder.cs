using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class YouTubeMediaItemBuilder
    {
        private String _title = string.Empty;
        private Int32 _displayWidth = 0;
        private Int32 _displayHeight = 0;
        private DateTime _createDate = DateTime.MinValue;

        private String _videoUrl = string.Empty;

        public YouTubeMediaItem Build()
        {
            return new YouTubeMediaItem(_title, _displayWidth, _displayHeight, _createDate, _videoUrl);
        }

        public YouTubeMediaItemBuilder Title(String title)
        {
            _title = title;
            return this;
        }

        public YouTubeMediaItemBuilder DisplayWidth(Int32 displayWidth)
        {
            _displayWidth = displayWidth;
            return this;
        }

        public YouTubeMediaItemBuilder DisplayHeight(Int32 displayHeight)
        {
            _displayHeight = displayHeight;
            return this;
        }

        public YouTubeMediaItemBuilder CreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

        public YouTubeMediaItemBuilder VideoUrl(String videoUrl)
        {
            _videoUrl = videoUrl;
            return this;
        }

    }
}
