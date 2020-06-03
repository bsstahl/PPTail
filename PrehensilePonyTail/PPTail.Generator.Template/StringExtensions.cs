using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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

            var content = item.Content
                .ReplacePathToRootVariables(pathToRoot)
                .ReplaceContentPageLinks(serviceProvider, pathToRoot)
                .ReplaceContentPostLinks(serviceProvider, pathToRoot)
                .ReplaceSettingsVariables(serviceProvider)
                .ReplacePageLinkVariables(serviceProvider, pathToRoot);

            var description = item.Description;
            var pubDate = item.PublicationDate.ToUniversalTime().ToString(settings.DateFormatSpecifier);
            var pubDateTime = item.PublicationDate.ToUniversalTime().ToString(settings.DateTimeFormatSpecifier);
            var lastModDate = item.LastModificationDate.ToUniversalTime().IsMinDate() ? String.Empty : item.LastModificationDate.ToString(settings.DateFormatSpecifier);
            var lastModDateTime = item.LastModificationDate.ToUniversalTime().IsMinDate() ? String.Empty : item.LastModificationDate.ToString(settings.DateTimeFormatSpecifier);

            if (xmlEncodeContent)
            {
                content = contentEncoder.XmlEncode(content);
                description = contentEncoder.XmlEncode(description);
                pubDate = item.PublicationDate.ToUniversalTime().Date.ToString("o");
                pubDateTime = item.PublicationDate.ToUniversalTime().ToString("o");
                lastModDate = item.LastModificationDate.IsMinDate() ? String.Empty : item.LastModificationDate.ToUniversalTime().Date.ToString("o");
                lastModDateTime = item.LastModificationDate.IsMinDate() ? String.Empty : item.LastModificationDate.ToUniversalTime().ToString("o");
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
            var mainContentText = content.ReplacePathToRootVariables(pathToRoot);
            var sidebarContentText = sidebarContent.ReplacePathToRootVariables(pathToRoot);

            var linkProvider = serviceProvider.GetService<ILinkProvider>();

            return template
                .Replace("{NavigationMenu}", navContent)
                .Replace("{Sidebar}", sidebarContentText)
                .Replace("{Content}", mainContentText)
                .ReplacePathToRootVariables(pathToRoot)
                .ReplaceSettingsVariables(serviceProvider)
                .ReplacePageLinkVariables(serviceProvider, pathToRoot);
        }

        internal static String ReplacePathToRootVariables(this String content, String pathToRoot)
        {
            return content
                .Replace("{PathToRoot}", pathToRoot)
                .Replace("{PathToSiteRoot}", pathToRoot)
                .Replace("%7BPathToRoot%7D", pathToRoot)
                .Replace("%7BPathToSiteRoot%7D", pathToRoot);
        }

        internal static String ReplaceSettingsVariables(this String template, IServiceProvider serviceProvider)
        {
            // Replace variables with values defined in the site settings file

            serviceProvider.ValidateService<ISettings>();
            serviceProvider.ValidateService<IContentRepository>();

            var settings = serviceProvider.GetService<ISettings>();
            var contentRepo = serviceProvider.GetContentRepository(settings.SourceConnection);
            var siteSettings = contentRepo.GetSiteSettings();

            return template
                .ReplaceSiteVariables(siteSettings.Variables)
                .Replace("{SiteTitle}", siteSettings.Title)
                .Replace("{SiteDescription}", siteSettings.Description)
                .Replace("{ContactEmail}", siteSettings.ContactEmail)
                .Replace("{Copyright}", siteSettings.Copyright);
        }

        internal static String ReplaceSiteVariables(this string template, IEnumerable<SiteVariable> variables)
        {
            // Replaces the variables stored in site settings
            // Example: {TwitterLink} replaced with <a href="https://twitter.com/mytwittername>@mytwittername"</a>

            string result = template;

            if (variables.IsNotNull())
                foreach (var variable in variables)
                {
                    string key = "{" + variable.Name + "}";
                    result = result.Replace(key, variable.Value);
                }

            return result;
        }

        internal static String ReplacePageLinkVariables(this string template, IServiceProvider serviceProvider, String pathToRoot)
        {
            var linkProvider = serviceProvider.GetService<ILinkProvider>();
            return template
                .ReplaceHomePageLinks(linkProvider, pathToRoot)
                .ReplaceArchivePageLinks(linkProvider, pathToRoot)
                .ReplaceContactPageLinks(linkProvider, pathToRoot)
                .ReplaceSearchPageLinks(linkProvider, pathToRoot)
                .ReplaceContentPageLinks(serviceProvider, pathToRoot)
                .ReplaceContentPostLinks(serviceProvider, pathToRoot);
        }

        internal static String ReplaceHomePageLinks(this string template, ILinkProvider linkProvider, String pathToRoot)
        {
            string regexPattern = @"\{HomePageLink[:]*([^\}]*)\}";
            string relativePath = ".";
            string fileName = "index";
            string defaultLinkText = "Home";

            var replacements = template.GetLinkReplacementsForRootPage(linkProvider, regexPattern, pathToRoot, relativePath, fileName, defaultLinkText);
            return template.Replace(replacements);
        }

        internal static String ReplaceArchivePageLinks(this string template, ILinkProvider linkProvider, String pathToRoot)
        {
            string regexPattern = @"\{ArchivePageLink[:]*([^\}]*)\}";
            string relativePath = ".";
            string fileName = "archive";
            string defaultLinkText = "Archive";

            var replacements = template.GetLinkReplacementsForRootPage(linkProvider, regexPattern, pathToRoot, relativePath, fileName, defaultLinkText);
            return template.Replace(replacements);
        }

        internal static String ReplaceContactPageLinks(this string template, ILinkProvider linkProvider, String pathToRoot)
        {
            string regexPattern = @"\{ContactPageLink[:]*([^\}]*)\}";
            string relativePath = ".";
            string fileName = "contact";
            string defaultLinkText = "Contact Me";

            var replacements = template.GetLinkReplacementsForRootPage(linkProvider, regexPattern, pathToRoot, relativePath, fileName, defaultLinkText);
            return template.Replace(replacements);
        }

        internal static String ReplaceContentPageLinks(this string template, IServiceProvider serviceProvider, String pathToRoot)
        {
            string regexPattern = @"\{PageLink:([^\|\}]+)\|*([^\}]*)\}";
            string relativePath = "Pages";

            var replacements = template.GetLinkReplacementsForContentItem(serviceProvider, regexPattern, pathToRoot, relativePath, Enumerations.TemplateType.ContentPage);
            return template.Replace(replacements);
        }

        internal static String ReplaceContentPostLinks(this string template, IServiceProvider serviceProvider, String pathToRoot)
        {
            string regexPattern = @"\{PostLink:([^\|\}]+)\|*([^\}]*)\}";
            string relativePath = "Posts";

            var replacements = template.GetLinkReplacementsForContentItem(serviceProvider, regexPattern, pathToRoot, relativePath, Enumerations.TemplateType.PostPage);
            return template.Replace(replacements);
        }

        internal static String ReplaceSearchPageLinks(this string template, ILinkProvider linkProvider, String pathToRoot)
        {
            string regexPattern = @"\{SearchLink:([^\|\}]+)\|*([^\}]*)\}";
            string relativePath = "Search";

            var replacements = template.GetLinkReplacementsForSearchTags(linkProvider, regexPattern, pathToRoot, relativePath);
            return template.Replace(replacements);
        }

        internal static IEnumerable<(String, InternalLink)> GetLinkReplacementsForRootPage(this String template, ILinkProvider linkProvider, String regexPattern, String pathToRoot, String relativePath, string fileName, String defaultLinkText)
        {
            var match = Regex.Match(template, regexPattern);
            var linkReplacements = new List<(String, InternalLink)>();
            foreach (Match capture in match.Captures)
            {
                var sourceText = capture.Groups[0].Value;
                var linkText = capture.Groups[1].Value ?? defaultLinkText;

                var pageLink = new InternalLink(linkProvider, linkText, pathToRoot, relativePath, fileName);
                linkReplacements.Add((sourceText, pageLink));
            }

            return linkReplacements;
        }

        internal static IEnumerable<(String, InternalLink)> GetLinkReplacementsForSearchTags(this String template, ILinkProvider linkProvider, String regexPattern, String pathToRoot, String relativePath)
        {
            var match = Regex.Match(template, regexPattern);
            var linkReplacements = new List<(String, InternalLink)>();
            foreach (Match capture in match.Captures)
            {
                var sourceText = capture.Groups[0].Value;
                var tagName = capture.Groups[1].Value;
                var group2Value = capture.Groups[2].Value;

                string linkText = String.IsNullOrWhiteSpace(group2Value) ? tagName : group2Value;

                var pageLink = new InternalLink(linkProvider, linkText, pathToRoot, relativePath, tagName.ToLower());
                linkReplacements.Add((sourceText, pageLink));
            }

            return linkReplacements;
        }

        internal static IEnumerable<(String, InternalLink)> GetLinkReplacementsForContentItem(this String template, IServiceProvider serviceProvider, String regexPattern, String pathToRoot, String relativePath, Enumerations.TemplateType contentItemType)
        {
            var linkProvider = serviceProvider.GetService<ILinkProvider>();
            var contentRepo = serviceProvider.GetService<IContentRepository>();

            var match = Regex.Match(template, regexPattern);
            var linkReplacements = new List<(String, InternalLink)>();
            foreach (Match capture in match.Captures)
            {
                var sourceText = capture.Groups[0].Value;
                var itemSlug = capture.Groups[1].Value;
                var linkText = capture.Groups[2].Value;

                ContentItem item = null;
                if (contentItemType == Enumerations.TemplateType.ContentPage)
                    item = contentRepo.GetPageBySlug(itemSlug);
                else if (contentItemType == Enumerations.TemplateType.PostPage)
                    item = contentRepo.GetPostBySlug(itemSlug);
                else
                    throw new ArgumentException($"Invalid template type for link replacement '{contentItemType.ToString()}'. For anything other than a Page or Post, the link text must be supplied.", nameof(contentItemType));

                if (item is null)
                {
                    throw new Exceptions.ContentItemNotFoundException(itemSlug);
                }
                else
                {
                    // LinkText defaults to the item title if none supplied
                    linkText = String.IsNullOrWhiteSpace(linkText) ? item.Title : linkText;
                    var pageLink = new InternalLink(linkProvider, linkText, pathToRoot, relativePath, item.Slug);
                    linkReplacements.Add((sourceText, pageLink));
                }

            }

            return linkReplacements;
        }

        internal static String Replace(this String template, IEnumerable<(String, InternalLink)> linkReplacements)
        {
            foreach (var (sourceText, pageLink) in linkReplacements)
            {
                var result = pageLink.ToString();
                template = template.Replace(sourceText, result);
            }

            return template;
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
