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
        public static string Render(this Widget widget, IServiceProvider serviceProvider, ISettings settings, IEnumerable<ContentItem> posts, string pathToRoot)
        {
            string results = $"<div class=\"widget {widget.WidgetType.ToString().ToLowerInvariant().Replace("_", "")}\">";

            if (widget.WidgetType == Enumerations.WidgetType.TextBox)
                results += widget.RenderTextBoxWidget();
            if (widget.WidgetType == Enumerations.WidgetType.Tag_cloud)
                results += widget.RenderTagCloudWidget(serviceProvider, settings, posts, pathToRoot);

            results += "</div>";
            return results;
        }

        private static string RenderTextBoxWidget(this Widget widget)
        {
            string results = string.Empty;
            if (widget.ShowTitle)
                results += $"<h4>{widget.Title}</h4>";
            results += $"<div class=\"content\">{widget.FirstDictionaryItemContent()}</div>";
            return results;
        }

        private static string RenderTagCloudWidget(this Widget widget, IServiceProvider serviceProvider, ISettings settings, IEnumerable<ContentItem> posts, string pathToRoot)
        {
            string results = string.Empty;

            //TODO: Add code coverage (currently intermittent)
            if (widget.ShowTitle)
                results += $"<h4>{widget.Title}</h4>";

            var tags = posts.SelectMany(p => p.Tags);

            var styler = serviceProvider.GetService<ITagCloudStyler>();
            var styles = styler.GetStyles(tags).OrderBy(s => s.Item1).Distinct();

            serviceProvider.ValidateService<ILinkProvider>();
            var linkProvider = serviceProvider.GetService<ILinkProvider>();

            serviceProvider.ValidateService<IContentEncoder>();
            var contentEncoder = serviceProvider.GetService<IContentEncoder>();

            results += "<div class=\"content\"><ul>";
            foreach (var style in styles)
            {
                string title = contentEncoder.UrlEncode(style.Item1);
                string url = linkProvider.GetUrl(pathToRoot, "search", title);
                results += $"<li><a title=\"Tag: {title}\" class=\"{style.Item2}\" href=\"{url}\">{title}</a></li>";
            }

            results += "</ul></div>";
            return results;
        }

        public static string FirstDictionaryItemContent(this Widget widget)
        {
            return widget.Dictionary.First().Item2;
        }
    }
}
