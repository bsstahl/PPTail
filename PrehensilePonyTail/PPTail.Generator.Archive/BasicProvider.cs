using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Enumerations;
using PPTail.Extensions;

namespace PPTail.Generator.Archive
{
    public class BasicProvider : IArchiveProvider
    {
        private IServiceProvider _serviceProvider;
        private Template _template;

        public BasicProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (_serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            // TODO: Add code coverage
            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<SiteSettings>();

            var templates = serviceProvider.GetService<IEnumerable<Template>>();
            _template = templates.Find(TemplateType.HomePage);
        }

        public string GenerateArchive(IEnumerable<ContentItem> posts, IEnumerable<ContentItem> pages, string navContent, string sidebarContent, string pathToRoot)
        {
            var settings = _serviceProvider.GetService<ISettings>();
            var siteSettings = _serviceProvider.GetService<SiteSettings>();

            string content = "<div id=\"archive\"><h1>Archive</h1>";
            content += "<table><tbody>";
            content += "<tr><th>Date</th><th>Title</th></tr>";

            foreach (var post in posts.Where(p => p.IsPublished).OrderByDescending(p => p.PublicationDate))
                content += $"<tr><td class=\"date\">{post.PublicationDate.ToString(settings.DateFormatSpecifier)}</td><td class=\"title\"><a href=\"{GetPath(post, settings, pathToRoot)}\">{post.Title}</a></td></tr>";
            content += "</tbody></table></div>";

            return _template.ProcessNonContentItemTemplate(_serviceProvider, sidebarContent, navContent, content, "Archive");
        }

        public string GetPath(ContentItem item, ISettings settings, string pathToRoot)
        {
            return System.IO.Path.Combine(pathToRoot, "Posts", $"{item.Slug}.{settings.OutputFileExtension}");
        }

    }
}
