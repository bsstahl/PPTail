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
        public static string ProcessTemplate(this IEnumerable<ContentItem> posts, ISettings settings, SiteSettings siteSettings, IEnumerable<Category> categories, Template pageTemplate, Template itemTemplate, string sidebarContent, string navContent, string pageTitle, int maxPostCount, string pathToRoot, string itemSeparator, bool xmlEncodeContent)
        {
            string content = string.Empty;
            var recentPosts = posts.OrderByDescending(p => p.PublicationDate).Where(pub => pub.IsPublished);

            if (maxPostCount > 0)
                recentPosts = recentPosts.Take(maxPostCount);

            var contentItems = new List<string>();
            foreach (var post in recentPosts)
                contentItems.Add(itemTemplate.ProcessContentItemTemplate(post, sidebarContent, navContent, siteSettings, settings, categories, pathToRoot, xmlEncodeContent));

            var pageContent = string.Join(itemSeparator, contentItems);
            return pageTemplate.ProcessNonContentItemTemplate(sidebarContent, navContent, siteSettings, settings, pageContent, pageTitle);
        }

        public static string GetLinkUrl(this ContentItem post, string pathToRoot, string outputFileExtension)
        {
            string filename = $"{post.Slug.ToString()}.{outputFileExtension}";
            return System.IO.Path.Combine(pathToRoot, "Posts", filename).ToHttpSlashes();
        }

        public static string GetPermalink(this ContentItem post, string pathToRoot, string outputFileExtension, string linkText)
        {
            string filename = $"{post.Id.ToString()}.{outputFileExtension}";
            string url = System.IO.Path.Combine(pathToRoot, "Permalinks", filename).ToHttpSlashes();
            return $"<a href=\"{url}\" rel=\"bookmark\">{linkText}</a>";
        }
    }
}
