using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class FlickrMediaItemBuilder
    {
        private string _title = string.Empty;
        private int _displayWidth = 0;
        private int _displayHeight = 0;
        private DateTime _createDate = DateTime.MinValue;

        private string _flickrListUrl = string.Empty;
        private string _imageUrl = string.Empty;

        public FlickrMediaItem Build()
        {
            return new FlickrMediaItem(_title, _displayWidth, _displayHeight, _createDate, _flickrListUrl, _imageUrl);
        }

        public FlickrMediaItemBuilder Title(string title)
        {
            _title = title;
            return this;
        }

        public FlickrMediaItemBuilder DisplayWidth(int displayWidth)
        {
            _displayWidth = displayWidth;
            return this;
        }

        public FlickrMediaItemBuilder DisplayHeight(int displayHeight)
        {
            _displayHeight = displayHeight;
            return this;
        }

        public FlickrMediaItemBuilder CreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

        public FlickrMediaItemBuilder FlickrListUrl(string flickrListUrl)
        {
            _flickrListUrl = flickrListUrl;
            return this;
        }

        public FlickrMediaItemBuilder ImageUrl(string imageUrl)
        {
            _imageUrl = imageUrl;
            return this;
        }

    }
}
