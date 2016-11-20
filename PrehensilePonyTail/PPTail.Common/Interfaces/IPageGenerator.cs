using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IPageGenerator
    {
        string GenerateStylesheet();
        string GenerateBootstrapPage();
        string GenerateSidebarContent(IEnumerable<Entities.ContentItem> posts, IEnumerable<Entities.ContentItem> pages, IEnumerable<Entities.Widget> widgets, string pathToRoot);
    }
}