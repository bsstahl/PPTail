using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class EmptyMediaItemBuilder
    {
        private String _title = string.Empty;
        private Int32 _displayWidth = 0;
        private Int32 _displayHeight = 0;
        private DateTime _createDate = DateTime.MinValue;

        public EmptyMediaItem Build()
        {
            return new EmptyMediaItem(_title, _displayWidth, _displayHeight, _createDate);
        }

        public EmptyMediaItemBuilder Title(String title)
        {
            _title = title;
            return this;
        }

        public EmptyMediaItemBuilder DisplayWidth(Int32 displayWidth)
        {
            _displayWidth = displayWidth;
            return this;
        }

        public EmptyMediaItemBuilder DisplayHeight(Int32 displayHeight)
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
