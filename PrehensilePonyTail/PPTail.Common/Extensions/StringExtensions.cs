using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class StringExtensions
    {
        internal static string ReplaceContentItemVariables(this string template, ISettings settings, SiteSettings siteSettings, IEnumerable<Category> categories, ContentItem item, string pathToRoot, bool xmlEncodeContent)
        {

            string content = item.Content;
            string description = item.Description;
            string pubDate = item.PublicationDate.ToString(settings.DateTimeFormatSpecifier);
            string lastModDate = item.LastModificationDate.ToString(settings.DateTimeFormatSpecifier);

            if (xmlEncodeContent)
            {
                content = item.Content.XmlEncode();
                description = item.Description.XmlEncode();
                pubDate = item.PublicationDate.ToString("o");
                lastModDate = item.LastModificationDate.ToString("o");
            }

            return template.Replace("{Title}", item.Title)
                .Replace("{Content}", content)
                .Replace("{Author}", item.Author)
                .Replace("{Description}", description)
                .Replace("{PublicationDate}", pubDate)
                .Replace("{LastModificationDate}", lastModDate)
                .Replace("{ByLine}", item.ByLine)
                .Replace("{Tags}", item.Tags.TagLinkList(settings, pathToRoot, "small"))
                .Replace("{Categories}", categories.CategoryLinkList(item.CategoryIds, settings, pathToRoot, "small"))
                .Replace("{Link}", item.GetLinkUrl(pathToRoot, settings.OutputFileExtension))
                .Replace("{Permalink}", item.GetPermalink(pathToRoot, settings.OutputFileExtension, "Permalink"));
        }

        internal static string ReplaceNonContentItemSpecificVariables(this string template, ISettings settings, SiteSettings siteSettings, string sidebarContent, string navContent, string content)
        {
            return template
                .ReplaceSettingsVariables(settings, siteSettings)
                .Replace("{NavigationMenu}", navContent)
                .Replace("{Sidebar}", sidebarContent)
                .Replace("{Content}", content);
        }

        internal static string ReplaceSettingsVariables(this string template, ISettings settings, SiteSettings siteSettings)
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

        public static string XmlEncode(this string content)
        {
            return content.Replace("<", "&lt;")
                .Replace(">","&gt;")
                .Replace("“", "&quot;")
                .Replace("”", "&quot;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;")
                .Replace("&", "&amp;");
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

        public static string TagLinkList(this IEnumerable<string> tags, ISettings settings, string pathToRoot, string cssClass)
        {
            var results = string.Empty;
            foreach (var tag in tags)
                results += $"{settings.CreateSearchLink(pathToRoot, tag, "Tag", cssClass)}&nbsp;";
            return results;
        }

        public static string ToHttpSlashes(this string path)
        {
            return path.Replace("\\", "/");
        }
    }
}
