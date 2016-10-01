using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface ISearchProvider
    {
        string GenerateSearchResultsPage(string tag, IEnumerable<ContentItem> contentItems, string navigationContent, string sidebarContent, string pathToRoot);
    }
}
