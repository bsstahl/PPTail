using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class YouTubeMediaItemBuilder
    {
        private string _title = string.Empty;
        private int _displayWidth = 0;
        private int _displayHeight = 0;
        private DateTime _createDate = DateTime.MinValue;

        private string _videoUrl = string.Empty;

        public YouTubeMediaItem Build()
        {
            return new YouTubeMediaItem(_title, _displayWidth, _displayHeight, _createDate, _videoUrl);
        }

        public YouTubeMediaItemBuilder Title(string title)
        {
            _title = title;
            return this;
        }

        public YouTubeMediaItemBuilder DisplayWidth(int displayWidth)
        {
            _displayWidth = displayWidth;
            return this;
        }

        public YouTubeMediaItemBuilder DisplayHeight(int displayHeight)
        {
            _displayHeight = displayHeight;
            return this;
        }

        public YouTubeMediaItemBuilder CreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

        public YouTubeMediaItemBuilder VideoUrl(string videoUrl)
        {
            _videoUrl = videoUrl;
            return this;
        }

    }
}
