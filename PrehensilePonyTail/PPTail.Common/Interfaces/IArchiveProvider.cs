using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IArchiveProvider
    {
        String GenerateArchive(IEnumerable<Entities.ContentItem> posts, IEnumerable<Entities.ContentItem> pages, String navigationContent, String sidebarContent, String pathToRoot);
    }
}
