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
                .Replace("!", "")
                .Replace("“", "")
                .Replace("”", "")
                .Replace("–", "-");
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
    }
}
