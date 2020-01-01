using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.PhotoBlog
{
    internal class FlickrImage
    {
        public String FlickrListUrl { get; set; }
        public String Title { get; set; }
        public String ImageUrl { get; set; }
        public Int32 ImageWidth { get; set; }
        public Int32 ImageHeight { get; set; }
        public DateTime ImageTaken { get; set; }
    }

}
