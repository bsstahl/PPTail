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
                throw new Exceptions.DependencyNotFoundException(nameof(IServiceProvider));

            _template = serviceProvider.GetService<IEnumerable<Template>>().SingleOrDefault(t => t.TemplateType == TemplateType.HomePage);
            if (_template == null)
                throw new Exceptions.TemplateNotFoundException(Enumerations.TemplateType.HomePage, "HomePage");
        }

        public string GenerateArchive(Settings settings, SiteSettings siteSettings, IEnumerable<ContentItem> posts, IEnumerable<ContentItem> pages, string navContent, string sidebarContent, string pathToRoot)
        {
            string content = "<div id=\"archive\"><h1>Archive</h1>";
            content += "<table><tbody>";
            content += "<tr><th>Date</th><th>Title</th></tr>";

            foreach (var post in posts.Where(p => p.IsPublished).OrderByDescending(p => p.PublicationDate))
                content += $"<tr><td class=\"date\">{post.PublicationDate.ToString(settings.DateFormatSpecifier)}</td><td class=\"title\"><a href=\"{GetPath(post, settings, pathToRoot)}\">{post.Title}</a></td></tr>";
            content += "</tbody></table></div>";

            return _template.Content
                .ReplaceSettingsVariables(siteSettings)
                .Replace("{NavigationMenu}", navContent)
                .Replace("{Sidebar}", sidebarContent)
                .Replace("{Content}", content);
        }

        public string GetPath(ContentItem item, Settings settings, string pathToRoot)
        {
            return System.IO.Path.Combine(pathToRoot, "Posts", $"{item.Slug}.{settings.outputFileExtension}");
        }

    }
}
