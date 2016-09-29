using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class TemplateExtensions
    {
        public static string ProcessContentItemTemplate(this Template template, ContentItem item, string sidebarContent, string navContent, SiteSettings siteSettings, Settings settings)
        {
            return template.Content
                .ReplaceContentItemVariables(settings, siteSettings, item)
                .ReplaceNonContentItemSpecificVariables(settings, siteSettings, sidebarContent, navContent, string.Empty);
        }

        public static string ProcessNonContentItemTemplate(this Template template, string sidebarContent, string navContent, SiteSettings siteSettings, Settings settings, string content)
        {
            return template.Content
                .ReplaceNonContentItemSpecificVariables(settings, siteSettings, sidebarContent, navContent, content);
        }

    }
}
