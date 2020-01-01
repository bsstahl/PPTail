using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class SiteSettings
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 PostsPerPage { get; set; }
        public Int32 PostsPerFeed { get; set; }
        public String Theme { get; set; }
        public String Copyright { get; set; }
    }
}
