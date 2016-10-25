using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IHomePageGenerator
    {
        string GenerateHomepage(string sidebarContent, string navigationContent, Entities.SiteSettings siteSettings, IEnumerable<Entities.ContentItem> posts);
    }
}
