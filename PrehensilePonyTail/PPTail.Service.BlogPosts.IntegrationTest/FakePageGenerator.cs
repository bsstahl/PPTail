using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using PPTail.Entities;
using PPTail.Enumerations;

namespace PPTail.Service.BlogPosts.IntegrationTest
{
    public class FakePageGenerator : IContentItemPageGenerator
    {
        public string Generate(string sidebarContent, string navContent, ContentItem pageData, TemplateType templateType, string pathToRoot, bool xmlEncodeContent)
        {
            return pageData.Title;
        }
    }
}
