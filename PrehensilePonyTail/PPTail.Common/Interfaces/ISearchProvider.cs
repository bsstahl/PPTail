using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface ISearchProvider
    {
        String GenerateSearchResultsPage(String tag, IEnumerable<ContentItem> contentItems, String navigationContent, String sidebarContent, String pathToRoot);
    }
}
