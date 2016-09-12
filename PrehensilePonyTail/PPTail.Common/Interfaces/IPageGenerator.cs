using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IPageGenerator
    {
        string GenerateStylesheet(Entities.SiteSettings settings);
        string GenerateHomepage(Entities.SiteSettings settings, IEnumerable<Entities.ContentItem> posts);
        string GenerateContentPage(Entities.SiteSettings settings, Entities.ContentItem pageData);
        string GeneratePostPage(Entities.SiteSettings settings, Entities.ContentItem article);
    }
}
