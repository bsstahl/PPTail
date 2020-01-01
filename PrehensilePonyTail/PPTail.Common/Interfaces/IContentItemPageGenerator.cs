using PPTail.Entities;
using PPTail.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IContentItemPageGenerator
    {
        String Generate(String sidebarContent, String navContent, ContentItem pageData, TemplateType templateType, String pathToRoot, bool xmlEncodeContent);
    }
}
