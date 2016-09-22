using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.T4Html
{
    public static class WidgetExtensions
    {
        public static string Render(this Widget widget, Settings settings, IEnumerable<ContentItem> posts)
        {
            string results = $"<div class=\"widget {widget.WidgetType.ToString().ToLowerInvariant().Replace("_","")}\">";

            if (widget.WidgetType == Enumerations.WidgetType.TextBox)
                results += widget.RenderTextBoxWidget();
            if (widget.WidgetType == Enumerations.WidgetType.Tag_cloud)
                results += widget.RenderTagCloudWidget(settings, posts);

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

        private static string RenderTagCloudWidget(this Widget widget, Settings settings, IEnumerable<ContentItem> posts)
        {
            string results = string.Empty;
            if (widget.ShowTitle)
                results += $"<h4>{widget.Title}</h4>";

            results += "<div class=\"content\"><ul>";
            foreach (var post in posts)
                if (post.Tags != null)
                    foreach (var tag in post.Tags)
                        results += $"<li><a title=\"Tag: {tag}\" class=\"smallest\" href=\"/search/{tag.Replace(" ", "_")}.{settings.outputFileExtension}\">{tag}</a></li> ";

            results += "</ul></div>";
            return results;
        }

        public static string FirstDictionaryItemContent(this Widget widget)
        {
            return widget.Dictionary.First().Item2;
        }
    }
}
