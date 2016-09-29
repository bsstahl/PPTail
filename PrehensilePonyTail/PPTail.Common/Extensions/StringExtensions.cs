using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class StringExtensions
    {
        internal static string ReplaceContentItemVariables(this string template, Settings settings, SiteSettings siteSettings, ContentItem item)
        {
            return template.Replace("{Title}", item.Title)
                .Replace("{Content}", item.Content)
                .Replace("{Author}", item.Author)
                .Replace("{Description}", item.Description)
                .Replace("{PublicationDate}", item.PublicationDate.ToString(settings.DateTimeFormatSpecifier))
                .Replace("{LastModificationDate}", item.LastModificationDate.ToString(settings.DateTimeFormatSpecifier))
                .Replace("{ByLine}", item.ByLine);
        }

        internal static string ReplaceNonContentItemSpecificVariables(this string template, Settings settings, SiteSettings siteSettings, string sidebarContent, string navContent, string content)
        {
            return template
                .ReplaceSettingsVariables(settings, siteSettings)
                .Replace("{NavigationMenu}", navContent)
                .Replace("{Sidebar}", sidebarContent)
                .Replace("{Content}", content);
        }

        internal static string ReplaceSettingsVariables(this string template, Settings settings, SiteSettings siteSettings)
        {
            return template.Replace("{SiteTitle}", siteSettings.Title)
                .Replace("{SiteDescription}", siteSettings.Description);
        }

    }
}
