using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class SiteSettings
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PostsPerPage { get; set; }
        public int PostsPerFeed { get; set; }
        public string Theme { get; set; }
        public string Copyright { get; set; }
    }
}
