using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.WordpressFiles
{
    public class Tag
    {
        public int id { get; set; }
        public int count { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string taxonomy { get; set; }
        public object[] meta { get; set; }
        public _Links _links { get; set; }
    }

}
