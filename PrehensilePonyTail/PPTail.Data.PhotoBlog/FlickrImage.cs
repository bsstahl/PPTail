using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.PhotoBlog
{
    internal class FlickrImage
    {
        public string FlickrListUrl { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public DateTime ImageTaken { get; set; }
    }

}
