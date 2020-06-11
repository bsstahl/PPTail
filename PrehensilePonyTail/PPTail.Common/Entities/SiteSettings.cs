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
        public String Theme { get; set; } = string.Empty;
        public String Copyright { get; set; }
        public String ContactEmail { get; set; }
        public bool UseAdditionalPagesDropdown { get; set; } = true;
        public String AdditionalPagesDropdownLabel { get; set; } = "Community";
        public bool DisplayTitleInNavbar { get; set; } = true;
        public IEnumerable<SiteVariable> Variables { get; set; } = new List<SiteVariable>();
    }
}
