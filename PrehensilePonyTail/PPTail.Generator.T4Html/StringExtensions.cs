using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.T4Html
{
    public static class StringExtensions
    {
        public static string ReplaceContentItemVariables(this string template, ContentItem item, string dateTimeFormatSpecifier)
        {
            return template.Replace("{Title}", item.Title)
                .Replace("{Content}", item.Content)
                .Replace("{Author}", item.Author)
                .Replace("{Description}", item.Description)
                .Replace("{PublicationDate}", item.PublicationDate.ToString(dateTimeFormatSpecifier))
                .Replace("{LastModificationDate}", item.LastModificationDate.ToString(dateTimeFormatSpecifier))
                .Replace("{ByLine}", item.ByLine);
        }

        public static string ReplaceSettingsVariables(this string template, SiteSettings settings)
        {
            return template.Replace("{SiteTitle}", settings.Title)
                .Replace("{SiteDescription}", settings.Description);
        }
    }
}

