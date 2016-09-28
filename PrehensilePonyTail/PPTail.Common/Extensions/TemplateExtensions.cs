using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class TemplateExtensions
    {
        public static string ProcessTemplate(this Template template, ContentItem item, string sidebarContent, string navContent, SiteSettings siteSettings, string dateTimeFormatSpecifier)
        {
            var updatedTemplate = template.Content.Replace("{Sidebar}", sidebarContent);
            return updatedTemplate
                .Replace("{NavigationMenu}", navContent)
                .ReplaceContentItemVariables(item, dateTimeFormatSpecifier)
                .ReplaceSettingsVariables(siteSettings);
        }



    }
}
