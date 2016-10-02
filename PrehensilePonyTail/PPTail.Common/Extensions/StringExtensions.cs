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

        public static string CreateSlug(this string title)
        {
            return title.Trim()
                .Replace(' ', '-')
                .HTMLEncode()
                .RemoveConsecutiveDashes();
        }

        public static string HTMLEncode(this string data)
        {
            return data.Replace("&quot;", "")
                .Replace("\"", "")
                .Replace("'", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace("&lt;", "")
                .Replace(">", "")
                .Replace("&gt;", "")
                .Replace("!", "bang")
                .Replace("“", "")
                .Replace("”", "")
                .Replace("–", "-")
                .Replace(".", "dot")
                .Replace("e28093", "-")
                .Replace("e2809c", "")
                .Replace("e2809d", "");
        }

        public static string RemoveConsecutiveDashes(this string data)
        {
            string original = string.Empty;
            string current = data;

            do
            {
                original = current;
                current = current.Replace("--", "-");
            } while (current != original);

            return current;
        }

    }
}
