using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Navigation
{
    public class BasicProvider: Interfaces.INavigationProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public BasicProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string CreateNavigation(IEnumerable<ContentItem> pages, string relativePathToRootFolder, string outputFileExtension)
        {
            string result = "<div class=\"menu\">";

            var homePageUri = System.IO.Path.Combine(relativePathToRootFolder, $"index.{outputFileExtension}");
            var archiveUri = System.IO.Path.Combine(relativePathToRootFolder, $"archive.{outputFileExtension}");
            var contactUri = System.IO.Path.Combine(relativePathToRootFolder, $"contact.{outputFileExtension}");

            result += $"<a href=\"{homePageUri}\">Home</a>";
            result += $"<a href=\"{archiveUri}\">Archive</a>";
            result += $"<a href=\"{contactUri}\">Contact</a>";

            result += "<ul class=\"pagelist\" id=\"pagelist\">";
            foreach (var page in pages.Where(p => p.IsPublished && p.ShowInList))
            {
                var pageUri = System.IO.Path.Combine(relativePathToRootFolder, "pages", $"{page.Slug}.{outputFileExtension}");
                result += $"<li><a href=\"{pageUri}\">{page.Title}</a></li>";
            }

            var syndicationUri = System.IO.Path.Combine(relativePathToRootFolder, $"syndication.xml");
            var syndicationImageUri = System.IO.Path.Combine(relativePathToRootFolder, $"Images/rssicon.gif");
            result += $"<img align=\"absbottom\" id=\"rssIcon\" src=\"{syndicationImageUri}\" />";
            result += $"<a href=\"{syndicationUri}\">Subscribe</a>";

            result += "</ul></div>";

            return result;
        }
    }
}
