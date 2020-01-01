using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.WordpressFiles
{
    public class Tag
    {
        public Int32 id { get; set; }
        public Int32 count { get; set; }
        public String description { get; set; }
        public String link { get; set; }
        public String name { get; set; }
        public String slug { get; set; }
        public String taxonomy { get; set; }
        public object[] meta { get; set; }
        public _Links _links { get; set; }
    }

}
