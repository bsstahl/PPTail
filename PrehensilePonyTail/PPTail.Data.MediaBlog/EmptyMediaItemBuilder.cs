using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class EmptyMediaItemBuilder
    {
        private string _title = string.Empty;
        private int _displayWidth = 0;
        private int _displayHeight = 0;
        private DateTime _createDate = DateTime.MinValue;

        public EmptyMediaItem Build()
        {
            return new EmptyMediaItem(_title, _displayWidth, _displayHeight, _createDate);
        }

        public EmptyMediaItemBuilder Title(string title)
        {
            _title = title;
            return this;
        }

        public EmptyMediaItemBuilder DisplayWidth(int displayWidth)
        {
            _displayWidth = displayWidth;
            return this;
        }

        public EmptyMediaItemBuilder DisplayHeight(int displayHeight)
        {
            _displayHeight = displayHeight;
            return this;
        }

        public EmptyMediaItemBuilder CreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

    }
}
