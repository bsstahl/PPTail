using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Extensions;

namespace PPTail.Generator.T4Html
{
    public static class ContentItemExtensions
    {
        public static string ProcessTemplate(this IEnumerable<ContentItem> posts, string sidebarContent, string navContent, SiteSettings siteSettings, Template pageTemplate, Template itemTemplate, string dateTimeFormatSpecifier, string itemSeparator)
        {
            string content = string.Empty;
            var recentPosts = posts.Where(pub => pub.IsPublished)
                .OrderByDescending(p => p.PublicationDate)
                .Take(siteSettings.PostsPerPage);

            var contentItems = new List<string>();
            foreach (var post in recentPosts)
                contentItems.Add(itemTemplate.ProcessTemplate(post, sidebarContent, navContent, siteSettings, dateTimeFormatSpecifier));

            return pageTemplate.Content
            .ReplaceSettingsVariables(siteSettings)
            .Replace("{NavigationMenu}", navContent)
            .Replace("{Sidebar}", sidebarContent)
            .Replace("{Content}", string.Join(itemSeparator, contentItems));
        }

    }
}
