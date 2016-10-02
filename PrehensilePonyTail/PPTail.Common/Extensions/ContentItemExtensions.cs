using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Extensions;

namespace PPTail.Extensions
{
    public static class ContentItemExtensions
    {
        public static string ProcessTemplate(this IEnumerable<ContentItem> posts, Settings settings, SiteSettings siteSettings, Template pageTemplate, Template itemTemplate, string sidebarContent, string navContent, string pageTitle, int maxPostCount)
        {
            string content = string.Empty;
            var recentPosts = posts.Where(pub => pub.IsPublished);

            if (maxPostCount > 0)
                recentPosts = recentPosts.Take(maxPostCount);

            recentPosts = recentPosts.OrderByDescending(p => p.PublicationDate);

            var contentItems = new List<string>();
            foreach (var post in recentPosts)
                contentItems.Add(itemTemplate.ProcessContentItemTemplate(post, sidebarContent, navContent, siteSettings, settings));

            var pageContent = string.Join(settings.ItemSeparator, contentItems);
            return pageTemplate.ProcessNonContentItemTemplate(sidebarContent, navContent, siteSettings, settings, pageContent, pageTitle);
        }

    }
}
