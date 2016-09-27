using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Archive
{
    public class BasicProvider: IArchiveProvider
    {
        public BasicProvider(IServiceProvider serviceProvider)
        {
        }

        public string GenerateArchive(Settings settings, SiteSettings siteSettings, IEnumerable<ContentItem> posts, IEnumerable<ContentItem> pages, string pathToRoot)
        {
            string result = "<table><tbody>";

            foreach (var post in posts)
            {
                result += $"<a href=\"{GetPath(post, settings, pathToRoot)}\">{post.Title}</a>";
            }

            result += "</tbody></table>";

            return result;
        }

        public string GetPath(ContentItem item, Settings settings, string pathToRoot)
        {
            return $"{pathToRoot}/Posts/{item.Slug}.{settings.outputFileExtension}";

        }

    }
}
