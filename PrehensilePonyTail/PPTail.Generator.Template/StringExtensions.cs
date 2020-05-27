using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;

namespace PPTail.Generator.Template
{
    public static class StringExtensions
    {
        internal static String ReplaceContentItemVariables(this String template, IServiceProvider serviceProvider, ContentItem item, String pathToRoot, Boolean xmlEncodeContent)
        {
            serviceProvider.ValidateService<IContentRepository>();
            serviceProvider.ValidateService<ISettings>();
            serviceProvider.ValidateService<ILinkProvider>();
            serviceProvider.ValidateService<IContentEncoder>();

            var contentRepo = serviceProvider.GetService<IContentRepository>();
            var settings = serviceProvider.GetService<ISettings>();
            var linkProvider = serviceProvider.GetService<ILinkProvider>();
            var contentEncoder = serviceProvider.GetService<IContentEncoder>();

            var categories = contentRepo.GetCategories();

            var content = item.Content;
            var description = item.Description;
            var pubDate = item.PublicationDate.ToString(settings.DateFormatSpecifier);
            var pubDateTime = item.PublicationDate.ToString(settings.DateTimeFormatSpecifier);
            var lastModDate = item.LastModificationDate.IsMinDate() ? String.Empty : item.LastModificationDate.ToString(settings.DateFormatSpecifier);
            var lastModDateTime = item.LastModificationDate.IsMinDate() ? String.Empty : item.LastModificationDate.ToString(settings.DateTimeFormatSpecifier);

            if (xmlEncodeContent)
            {
                content = contentEncoder.XmlEncode(item.Content);
                description = contentEncoder.XmlEncode(item.Description);
                pubDate = item.PublicationDate.Date.ToString("o");
                pubDateTime = item.PublicationDate.ToString("o");
                lastModDate = item.LastModificationDate.IsMinDate() ? String.Empty : item.LastModificationDate.Date.ToString("o");
                lastModDateTime = item.LastModificationDate.IsMinDate() ? String.Empty : item.LastModificationDate.ToString("o");
            }

            var permaLinkUrl = linkProvider.GetUrl(pathToRoot, "Permalinks", item.Id.ToString());
            var permaLink = $"<a href=\"{permaLinkUrl}\" rel=\"bookmark\">Permalink</a>";

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

        internal static String ReplaceNonContentItemSpecificVariables(this String template, IServiceProvider serviceProvider, String sidebarContent, String navContent, String content, String pathToRoot)
        {
            return template
                .Replace("{NavigationMenu}", navContent)
                .Replace("{Sidebar}", sidebarContent)
                .Replace("{Content}", content)
                .Replace("{PathToSiteRoot}", pathToRoot)
                .ReplaceSettingsVariables(serviceProvider);
        }

        internal static String ReplaceSettingsVariables(this String template, IServiceProvider serviceProvider)
        {
            serviceProvider.ValidateService<ISettings>();
            serviceProvider.ValidateService<IContentRepository>();

            var settings = serviceProvider.GetService<ISettings>();
            var contentRepo = serviceProvider.GetContentRepository(settings.SourceConnection);
            var siteSettings = contentRepo.GetSiteSettings();

            return template.Replace("{SiteTitle}", siteSettings.Title)
                .Replace("{SiteDescription}", siteSettings.Description)
                .Replace("{Copyright}", siteSettings.Copyright);
        }


        internal static String CreateSearchLink(this String title, IServiceProvider serviceProvider, String pathToRoot, String linkType, String cssClass)
        {
            serviceProvider.ValidateService<ILinkProvider>();
            serviceProvider.ValidateService<IContentEncoder>();

            var linkProvider = serviceProvider.GetService<ILinkProvider>();
            var contentEncoder = serviceProvider.GetService<IContentEncoder>();

            var encodedTitle = contentEncoder.UrlEncode(title);
            var url = linkProvider.GetUrl(pathToRoot, "Search", encodedTitle);
            return $"<a title=\"{linkType}: {title}\" class=\"{cssClass}\" href=\"{url}\">{title}</a>";
        }

        internal static String TagLinkList(this IEnumerable<String> tags, IServiceProvider serviceProvider, String pathToRoot, String cssClass)
        {
            var results = String.Empty;
            if (tags.IsNotNull())
                foreach (var tag in tags)
                    results += $"{tag.CreateSearchLink(serviceProvider, pathToRoot, "Tag", cssClass)}&nbsp;";
            return results;
        }

    }
}
