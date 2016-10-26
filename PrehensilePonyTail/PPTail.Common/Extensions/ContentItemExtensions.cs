using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Extensions
{
    public static class ContentItemExtensions
    {
        public static string ProcessTemplate(this IEnumerable<ContentItem> posts, IServiceProvider serviceProvider, Template pageTemplate, Template itemTemplate, string sidebarContent, string navContent, string pageTitle, string pathToRoot, bool xmlEncodeContent, int maxPostCount)
        {
            // MaxPosts is not pulled from the SiteSettings because
            // there are 2 possible values that might be used to
            // define the value, they are PostsPerPage & PostsPerFeed
            string content = string.Empty;
            var recentPosts = posts.OrderByDescending(p => p.PublicationDate).Where(pub => pub.IsPublished);

            var settings = serviceProvider.GetService<ISettings>();
            string itemSeparator = settings.ItemSeparator;

            if (maxPostCount > 0)
                recentPosts = recentPosts.Take(maxPostCount);

            var contentItems = new List<string>();
            foreach (var post in recentPosts)
                contentItems.Add(itemTemplate.ProcessContentItemTemplate(serviceProvider, post, sidebarContent, navContent, pathToRoot, xmlEncodeContent));

            var pageContent = string.Join(itemSeparator, contentItems);
            return pageTemplate.ProcessNonContentItemTemplate(serviceProvider, sidebarContent, navContent, pageContent, pageTitle);
        }

        //public static string GetLinkUrl(this ContentItem post, string pathToRoot, string outputFileExtension)
        //{
        //    string filename = $"{post.Slug.ToString()}.{outputFileExtension}";
        //    return System.IO.Path.Combine(pathToRoot, "Posts", filename).ToHttpSlashes();
        //}

        //public static string GetPermalink(this ContentItem post, string pathToRoot, string outputFileExtension, string linkText)
        //{
        //    string filename = $"{post.Id.ToString()}.{outputFileExtension}";
        //    string url = System.IO.Path.Combine(pathToRoot, "Permalinks", filename).ToHttpSlashes();
        //    return $"<a href=\"{url}\" rel=\"bookmark\">{linkText}</a>";
        //}
    }
}
