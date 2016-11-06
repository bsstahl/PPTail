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
        string Generate(string sidebarContent, string navContent, ContentItem pageData, TemplateType templateType, string pathToRoot, bool xmlEncodeContent);
    }
}
