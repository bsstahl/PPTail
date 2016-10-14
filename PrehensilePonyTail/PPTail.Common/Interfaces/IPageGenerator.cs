using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IPageGenerator
    {
        string GenerateStylesheet(Entities.SiteSettings siteSettings);
        string GenerateBootstrapPage();
        string GenerateContentPage(string sidebarContent, string navigationContent, Entities.SiteSettings siteSettings, Entities.ContentItem pageData);
        string GeneratePostPage(string sidebarContent, string navigationContent, Entities.SiteSettings siteSettings, Entities.ContentItem article);
        string GenerateHomepage(string sidebarContent, string navigationContent, Entities.SiteSettings siteSettings, IEnumerable<Entities.ContentItem> posts);
        string GenerateSidebarContent(ISettings settings, Entities.SiteSettings siteSettings, IEnumerable<Entities.ContentItem> posts, IEnumerable<Entities.ContentItem> pages, IEnumerable<Entities.Widget> widgets, string pathToRoot);
    }
}
