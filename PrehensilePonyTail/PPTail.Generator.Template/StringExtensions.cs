using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Template
{
    public static class StringExtensions
    {
        internal static string ReplaceContentItemVariables(this string template, IServiceProvider serviceProvider, ContentItem item, string pathToRoot, bool xmlEncodeContent)
        {
            serviceProvider.ValidateService<ISettings>();
            serviceProvider.ValidateService<ILinkProvider>();
            serviceProvider.ValidateService<IContentEncoder>();
            serviceProvider.ValidateService<IEnumerable<Category>>();

            var settings = serviceProvider.GetService<ISettings>();
            var linkProvider = serviceProvider.GetService<ILinkProvider>();
            var contentEncoder = serviceProvider.GetService<IContentEncoder>();
            var categories = serviceProvider.GetService<IEnumerable<Category>>();

            string content = item.Content;
            string description = item.Description;
            string pubDate = item.PublicationDate.ToString(settings.DateFormatSpecifier);
            string pubDateTime = item.PublicationDate.ToString(settings.DateTimeFormatSpecifier);
            string lastModDate = item.LastModificationDate.ToString(settings.DateFormatSpecifier);
            string lastModDateTime = item.LastModificationDate.ToString(settings.DateTimeFormatSpecifier);

            if (xmlEncodeContent)
            {
                content = contentEncoder.XmlEncode(item.Content);
                description = contentEncoder.XmlEncode(item.Description);
                pubDate = item.PublicationDate.Date.ToString("o");
                pubDateTime = item.PublicationDate.ToString("o");
                lastModDate = item.LastModificationDate.Date.ToString("o");
                lastModDateTime = item.LastModificationDate.ToString("o");
            }

            string permaLinkUrl = linkProvider.GetUrl(pathToRoot, "Permalinks", item.Id.ToString());
            string permaLink = $"<a href=\"{permaLinkUrl}\" rel=\"bookmark\">Permalink</a>";

            return template.Replace("{Title}", item.Title)
                .Replace("{Content}", content)
                .Replace("{Author}", item.Author)
                .Replace("{Description}", description)
                .Replace("{ByLine}", item.ByLine)
                .Replace("{PublicationDate}", pubDate)
                .Replace("{PublicationDateTime}", pubDateTime)
                .Replace("{LastModificationDate}", lastModDate)
                .Replace("{LastModificationDateTime}", lastModDateTime)
                .Replace("{Link}", linkProvider.GetUrl(pathToRoot, "Posts", item.Slug))
                .Replace("{Permalink}", permaLink)
                .Replace("{PermalinkUrl}", permaLinkUrl)
                .Replace("{Tags}", item.Tags.TagLinkList(serviceProvider, pathToRoot, "small"))
                .Replace("{Categories}", categories.CategoryLinkList(serviceProvider, item.CategoryIds, settings, pathToRoot, "small"));
        }

        internal static string ReplaceNonContentItemSpecificVariables(this string template, IServiceProvider serviceProvider, string sidebarContent, string navContent, string content)
        {
            return template
                .Replace("{NavigationMenu}", navContent)
                .Replace("{Sidebar}", sidebarContent)
                .Replace("{Content}", content)
                .ReplaceSettingsVariables(serviceProvider);
        }

        internal static string ReplaceSettingsVariables(this string template, IServiceProvider serviceProvider)
        {
            serviceProvider.ValidateService<SiteSettings>();
            var siteSettings = serviceProvider.GetService<SiteSettings>();

            return template.Replace("{SiteTitle}", siteSettings.Title)
                .Replace("{SiteDescription}", siteSettings.Description);
        }


        internal static string CreateSearchLink(this string title, IServiceProvider serviceProvider, string pathToRoot, string linkType, string cssClass)
        {
            serviceProvider.ValidateService<ILinkProvider>();
            serviceProvider.ValidateService<IContentEncoder>();

            var linkProvider = serviceProvider.GetService<ILinkProvider>();
            var contentEncoder = serviceProvider.GetService<IContentEncoder>();

            string encodedTitle = contentEncoder.UrlEncode(title);
            string url = linkProvider.GetUrl(pathToRoot, "search", encodedTitle);
            return $"<a title=\"{linkType}: {title}\" class=\"{cssClass}\" href=\"{url}\">{title}</a>";
        }

        internal static string TagLinkList(this IEnumerable<string> tags, IServiceProvider serviceProvider, string pathToRoot, string cssClass)
        {
            var results = string.Empty;
            if (tags != null)
                foreach (var tag in tags)
                    results += $"{tag.CreateSearchLink(serviceProvider, pathToRoot, "Tag", cssClass)}&nbsp;";
            return results;
        }

    }
}
