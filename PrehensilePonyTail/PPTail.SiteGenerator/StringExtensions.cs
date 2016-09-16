using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.SiteGenerator
{
    public static class StringExtensions
    {
        public static string CreateSlug(this string title)
        {
            return title.Trim()
                .Replace(' ', '-')
                .Replace("&quot;", "")
                .Replace("\"", "")
                .Replace("'", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace("&lt;", "")
                .Replace(">", "")
                .Replace("&gt;", "")
                .Replace("--", "-");
        }
    }
}
