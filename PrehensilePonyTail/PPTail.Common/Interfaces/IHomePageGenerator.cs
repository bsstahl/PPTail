using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IHomePageGenerator
    {
        String GenerateHomepage(String sidebarContent, String navigationContent, IEnumerable<Entities.ContentItem> posts);
    }
}
