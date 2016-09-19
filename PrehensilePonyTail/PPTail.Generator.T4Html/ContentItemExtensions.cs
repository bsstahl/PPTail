using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.T4Html
{
    public static class ContentItemExtensions
    {
        public static string ProcessTemplate(this ContentItem item, string sidebarContent, SiteSettings siteSettings, string template, string dateTimeFormatSpecifier)
        {
            var updatedTemplate = template.Replace("{Sidebar}", sidebarContent);
            return updatedTemplate
                .ReplaceContentItemVariables(item, dateTimeFormatSpecifier)
                .ReplaceSettingsVariables(siteSettings);
        }

        public static string ProcessTemplate(this IEnumerable<ContentItem> items, string sidebarContent, SiteSettings siteSettings, string pageTemplate, string itemTemplate, string dateTimeFormatSpecifier, string itemSeparator)
        {
            string content = string.Empty;
            var recentPosts = items.Where(pub => pub.IsPublished)
                .OrderByDescending(p => p.PublicationDate)
                .Take(siteSettings.PostsPerPage);

            var contentItems = new List<string>();
            foreach (var post in recentPosts)
                contentItems.Add(post.ProcessTemplate(sidebarContent, siteSettings, itemTemplate, dateTimeFormatSpecifier));

            return pageTemplate
            .ReplaceSettingsVariables(siteSettings)
            .Replace("{Sidebar}", sidebarContent)
            .Replace("{Content}", string.Join(itemSeparator, contentItems));
        }

    }
}
