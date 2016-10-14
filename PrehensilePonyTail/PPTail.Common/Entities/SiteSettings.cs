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

        public object Create(object p)
        {
            throw new NotImplementedException();
        }
    }
}
