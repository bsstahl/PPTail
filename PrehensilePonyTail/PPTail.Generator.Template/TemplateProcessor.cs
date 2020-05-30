using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;
using PPTail.Entities;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Template
{
    public class TemplateProcessor : ITemplateProcessor
    {
        readonly IServiceProvider _serviceProvider;

        public TemplateProcessor(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public String Process(Entities.Template pageTemplate, Entities.Template itemTemplate, String sidebarContent, String navContent, IEnumerable<ContentItem> posts, String pageTitle, String pathToRoot, String itemSeparator, Boolean xmlEncodeContent, Int32 maxPostCount)
        {
            // MaxPosts is not pulled from the SiteSettings because
            // there are multiple possible values that might be used to
            // define the value: PostsPerPage, PostsPerFeed & 0 (unlimited)

            var recentPosts = posts.OrderByDescending(p => p.PublicationDate).Where(pub => pub.IsPublished);
            if (maxPostCount > 0)
            {
                recentPosts = recentPosts.Take(maxPostCount);
            }

            var contentItems = new List<String>();
            foreach (var post in recentPosts)
            {
                contentItems.Add(this.ProcessContentItemTemplate(itemTemplate, post, sidebarContent, navContent, pathToRoot, xmlEncodeContent));
            }

            var pageContent = String.Join(itemSeparator, contentItems);
            return this.ProcessNonContentItemTemplate(pageTemplate, sidebarContent, navContent, pageContent, pageTitle, pathToRoot);
        }

        public String ProcessContentItemTemplate(Entities.Template template, ContentItem item, String sidebarContent, String navContent, String pathToRoot, Boolean xmlEncodeContent)
        {
            return template.Content
                .ReplaceContentItemVariables(_serviceProvider, item, pathToRoot, xmlEncodeContent)
                .ReplaceNonContentItemSpecificVariables(_serviceProvider, sidebarContent, navContent, String.Empty, pathToRoot);
        }

        public String ProcessNonContentItemTemplate(Entities.Template template, String sidebarContent, String navContent, String content, String pageTitle, String pathToRoot)
        {
            return template.Content
                  .Replace("{Title}", pageTitle)
                  .Replace("{ByLine}", String.Empty)
                  .ReplaceNonContentItemSpecificVariables(_serviceProvider, sidebarContent, navContent, content, pathToRoot);
        }


    }
}
