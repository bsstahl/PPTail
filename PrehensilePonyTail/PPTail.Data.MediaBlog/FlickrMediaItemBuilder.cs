using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class FlickrMediaItemBuilder
    {
        private String _title = string.Empty;
        private Int32 _displayWidth = 0;
        private Int32 _displayHeight = 0;
        private DateTime _createDate = DateTime.MinValue;

        private String _flickrListUrl = string.Empty;
        private String _imageUrl = string.Empty;

        public FlickrMediaItem Build()
        {
            return new FlickrMediaItem(_title, _displayWidth, _displayHeight, _createDate, _flickrListUrl, _imageUrl);
        }

        public FlickrMediaItemBuilder Title(String title)
        {
            _title = title;
            return this;
        }

        public FlickrMediaItemBuilder DisplayWidth(Int32 displayWidth)
        {
            _displayWidth = displayWidth;
            return this;
        }

        public FlickrMediaItemBuilder DisplayHeight(Int32 displayHeight)
        {
            _displayHeight = displayHeight;
            return this;
        }

        public FlickrMediaItemBuilder CreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

        public FlickrMediaItemBuilder FlickrListUrl(String flickrListUrl)
        {
            _flickrListUrl = flickrListUrl;
            return this;
        }

        public FlickrMediaItemBuilder ImageUrl(String imageUrl)
        {
            _imageUrl = imageUrl;
            return this;
        }

    }
}
