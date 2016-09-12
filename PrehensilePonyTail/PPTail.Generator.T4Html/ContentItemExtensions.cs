using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.T4Html
{
    public static class ContentItemExtensions
    {
        public static string ProcessTemplate(this ContentItem item, SiteSettings settings, string template, string dateTimeFormatSpecifier)
        {
            return template
                .ReplaceContentItemVariables(item, dateTimeFormatSpecifier)
                .ReplaceSettingsVariables(settings);
        }

        public static string ProcessTemplate(this IEnumerable<ContentItem> items, SiteSettings settings, string pageTemplate, string itemTemplate, string dateTimeFormatSpecifier, string itemSeparator)
        {
            string content = string.Empty;
            var recentPosts = items.Where(pub => pub.IsPublished)
                .OrderByDescending(p => p.PublicationDate)
                .Take(settings.PostsPerPage);

            var contentItems = new List<string>();
            foreach (var post in recentPosts)
                contentItems.Add(post.ProcessTemplate(settings, itemTemplate, dateTimeFormatSpecifier));

            return pageTemplate
            .ReplaceSettingsVariables(settings)
            .Replace("{Content}", string.Join(itemSeparator, contentItems));
        }
    }
}
