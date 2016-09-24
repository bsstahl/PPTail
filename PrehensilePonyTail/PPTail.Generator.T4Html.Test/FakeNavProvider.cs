using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;

namespace PPTail.Generator.T4Html.Test
{
    public class FakeNavProvider : Interfaces.INavigationProvider
    {
        public string CreateNavigation(IEnumerable<ContentItem> pages, string homeUrl, string outputFileExtension)
        {
            return "<div class=\"menu\">Place Nav Here</div>";
        }
    }
}
