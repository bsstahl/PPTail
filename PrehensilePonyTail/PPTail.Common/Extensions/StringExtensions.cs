using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Extensions
{
    public static class StringExtensions
    {
        public static string CreateSlug(this string title)
        {
            return title.Trim()
                .Replace(' ', '-')
                .HTMLEncode()
                .RemoveConsecutiveDashes();
        }

        public static string HTMLEncode(this string data)
        {
            return data.Replace("&quot;", "")
                .Replace("\"", "")
                .Replace("'", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace("&lt;", "")
                .Replace(">", "")
                .Replace("&gt;", "")
                .Replace("!", "bang")
                .Replace("“", "")
                .Replace("”", "")
                .Replace("–", "-")
                .Replace(".", "dot")
                .Replace("e28093", "-")
                .Replace("e2809c", "")
                .Replace("e2809d", "");
        }

        public static string XmlEncode(this string content)
        {
            return content.Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("“", "&quot;")
                .Replace("”", "&quot;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;")
                .Replace("&", "&amp;");
        }

        public static string RemoveConsecutiveDashes(this string data)
        {
            string original = string.Empty;
            string current = data;

            do
            {
                original = current;
                current = current.Replace("--", "-");
            } while (current != original);

            return current;
        }

        public static string TagLinkList(this IEnumerable<string> tags, IServiceProvider serviceProvider, string pathToRoot, string cssClass)
        {
            var results = string.Empty;
            foreach (var tag in tags)
                results += $"{tag.CreateSearchLink(serviceProvider, pathToRoot, "Tag", cssClass)}&nbsp;";
            return results;
        }

        public static string CreateSearchLink(this string title, IServiceProvider serviceProvider, string pathToRoot, string linkType, string cssClass)
        {
            serviceProvider.ValidateService<ILinkProvider>();
            var linkProvider = serviceProvider.GetService<ILinkProvider>();
            string url = linkProvider.GetUrl(pathToRoot, "search", title.CreateSlug());
            return $"<a title=\"{linkType}: {title}\" class=\"{cssClass}\" href=\"{url}\">{title}</a>";
        }

    }
}
