using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.T4Html
{
    public static class WidgetExtensions
    {
        public static String Render(this Widget widget, IServiceProvider serviceProvider, ISettings settings, IEnumerable<ContentItem> posts, String pathToRoot)
        {
            String results = $"<div class=\"widget {widget.WidgetType.ToString().ToLowerInvariant().Replace("_", "")}\">";

            if (widget.WidgetType == Enumerations.WidgetType.TextBox)
            {
                serviceProvider.ValidateService<ITemplateProcessor>();
                var templateProcessor = serviceProvider.GetService<ITemplateProcessor>();
                results += widget.RenderTextBoxWidget(templateProcessor, string.Empty, string.Empty, pathToRoot);
            }

            if (widget.WidgetType == Enumerations.WidgetType.Tag_cloud)
                results += widget.RenderTagCloudWidget(serviceProvider, settings, posts, pathToRoot);

            if (widget.WidgetType == Enumerations.WidgetType.TagList)
                results += widget.RenderTagListWidget(serviceProvider, settings, posts, pathToRoot);

            results += "</div>";
            return results;
        }

        private static String RenderTextBoxWidget(this Widget widget, ITemplateProcessor templateProcessor, String content, String pageTitle, String pathToRoot)
        {
            String results = string.Empty;
            string templateContent = widget.FirstDictionaryItemContent().Replace("{PathToRoot}", pathToRoot);
            var template = new Template() { Content = templateContent, TemplateType = Enumerations.TemplateType.Raw };
            String widgetContent = templateProcessor.ProcessNonContentItemTemplate(template, string.Empty, string.Empty, content, pageTitle, pathToRoot);
            if (widget.ShowTitle)
                results += $"<h4>{widget.Title}</h4>";
            results += $"<div class=\"content\">{widgetContent}</div>";
            return results;
        }

        private static String RenderTagCloudWidget(this Widget widget, IServiceProvider serviceProvider, ISettings settings, IEnumerable<ContentItem> posts, String pathToRoot)
        {
            String results = string.Empty;

            if (widget.ShowTitle)
                results += $"<h4>{widget.Title}</h4>";

            var tags = posts.GetAllTags();

            var styler = serviceProvider.GetService<ITagCloudStyler>();
            var styles = styler.GetStyles(tags).OrderBy(s => s.Item1).Distinct();

            serviceProvider.ValidateService<ILinkProvider>();
            var linkProvider = serviceProvider.GetService<ILinkProvider>();

            serviceProvider.ValidateService<IContentEncoder>();
            var contentEncoder = serviceProvider.GetService<IContentEncoder>();

            results += "<div class=\"content\"><ul>";
            foreach (var style in styles)
            {
                String title = contentEncoder.UrlEncode(style.Item1);
                String url = linkProvider.GetUrl(pathToRoot, "Search", title);
                results += $"<li><a title=\"Tag: {title}\" class=\"{style.Item2}\" href=\"{url}\">{title}</a></li> ";
            }

            results += "</ul></div>";
            return results;
        }

        private static String RenderTagListWidget(this Widget widget, IServiceProvider serviceProvider, ISettings settings, IEnumerable<ContentItem> posts, String pathToRoot)
        {
            const String style = "medium";
            const Int32 topTagCountRows = 20;
            const Int32 topTagCountCols = 3;

            Int32 topTagCountItems = topTagCountRows * topTagCountCols;
            String results = string.Empty;

            if (widget.ShowTitle)
                results += $"<h4>{widget.Title}</h4>";

            var tags = posts.GetAllTags();
            var tagCounts = tags.GetTagCounts().OrderByDescending(tc => tc.Item2).Take(topTagCountItems).ToArray();

            serviceProvider.ValidateService<ILinkProvider>();
            var linkProvider = serviceProvider.GetService<ILinkProvider>();

            serviceProvider.ValidateService<IContentEncoder>();
            var contentEncoder = serviceProvider.GetService<IContentEncoder>();

            results += "<div class=\"content\"><table>";
            for (Int32 i = 0; i < topTagCountItems; i = i + topTagCountCols)
            {
                results += "<tr>";
                for (Int32 j = 0; j < topTagCountCols; j++)
                {
                    var tagCount = tagCounts[i + j];
                    String title = contentEncoder.UrlEncode(tagCount.Item1);
                    String url = linkProvider.GetUrl(pathToRoot, "search", title);
                    results += $"<td><a title=\"Tag: {title}\" class=\"{style}\" href=\"{url}\">{title}</a></td>";
                }
                results += "</tr>";
            }

            results += "</table></div>";
            return results;
        }

        public static String FirstDictionaryItemContent(this Widget widget)
        {
            return widget.Dictionary.First().Item2;
        }
    }
}
