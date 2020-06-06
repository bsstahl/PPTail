using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface INavigationProvider
    {
        String CreateNavigation(IEnumerable<ContentItem> pages, String relativePathToRootFolder, String outputFileExtension);
    }
}
