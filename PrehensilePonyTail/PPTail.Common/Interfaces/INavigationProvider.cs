using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface INavigationProvider
    {
        string CreateNavigation(IEnumerable<ContentItem> pages, string currentUrl, string homeUrl, string outputFileExtension);
    }
}
