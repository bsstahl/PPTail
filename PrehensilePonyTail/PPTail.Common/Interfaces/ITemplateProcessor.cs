using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface ITemplateProcessor
    {
        string Process(Template pageTemplate, Template itemTemplate, string sidebarContent, string navContent, IEnumerable<ContentItem> posts, string pageTitle, string pathToRoot, bool xmlEncodeContent, int maxPostCount);
    }
}
