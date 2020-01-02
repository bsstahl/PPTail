using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface ITemplateProcessor
    {
        String Process(Template pageTemplate, Template itemTemplate, String sidebarContent, String navContent, IEnumerable<ContentItem> posts, String pageTitle, String pathToRoot, String itemSeparator, Boolean xmlEncodeContent, Int32 maxPostCount);
        String ProcessContentItemTemplate(Entities.Template itemTemplate, ContentItem item, String sidebarContent, String navContent, String pathToRoot, Boolean xmlEncodeContent);
        String ProcessNonContentItemTemplate(Entities.Template itemTemplate, String sidebarContent, String navContent, String content, String pageTitle, String pathToRoot);
    }
}
