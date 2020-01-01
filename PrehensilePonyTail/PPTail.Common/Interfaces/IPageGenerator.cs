using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IPageGenerator
    {
        String GenerateStylesheet();
        String GenerateBootstrapPage();
        String GenerateSidebarContent(IEnumerable<Entities.ContentItem> posts, IEnumerable<Entities.ContentItem> pages, IEnumerable<Entities.Widget> widgets, String pathToRoot);
    }
}