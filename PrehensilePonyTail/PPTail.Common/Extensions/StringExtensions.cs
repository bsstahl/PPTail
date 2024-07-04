using PPTail.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Extensions
{
    public static class StringExtensions
    {
        public static string ToUrl(this string filePath)
        {
            return filePath.Replace("\\", "/");
        }

        public static Boolean AsBoolean(this String value)
        {
            _ = bool.TryParse(value, out var result);
            return result;
        }

        public static String ReadAllTextFromFile(this String path)
        {
            String result = string.Empty;
            if (System.IO.File.Exists(path))
                result = System.IO.File.ReadAllText(path);
            return result;
        }

        public static IEnumerable<Tuple<string, int>> GetTagCounts(this IEnumerable<string> tags)
        {
            var tagCounts = new List<Tuple<string, int>>();
            foreach (var tag in tags)
            {
                Int32 startingCount = 0;
                var tagCount = tagCounts.SingleOrDefault(t => t.Item1 == tag);
                if (tagCount != default(Tuple<string, int>))
                {
                    tagCounts.Remove(tagCount);
                    startingCount = tagCount.Item2;
                }
                tagCounts.Add(new Tuple<string, int>(tag, startingCount + 1));
            }
            return tagCounts;
        }


        public static String GetConnectionStringValue(this String connectionString, String key)
        {
            String result = string.Empty;

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new SettingNotFoundException(key);

            var parts = connectionString.Split(';');
            foreach (var part in parts)
            {
                var values = part.Split('=');
                if (values.Length == 2)
                {
                    if (values[0].ToLower() == key.ToLower())
                        result = values[1];
                }
            }

            return result;
        }

        public static String CreateSlug(this String title)
        {
            return title.Trim()
                .Replace("&quot;", "-")
                .Replace('?', '-')
                .Replace(':', '-')
                .Replace(' ', '-')
                .Replace(',', '-')
                .Replace("\'", "")
                .HTMLEncode()
                .RemoveConsecutiveDashes()
                .RemoveTrailingDash();
        }

        public static String HTMLEncode(this String data)
        {
            return System.Net.WebUtility.HtmlEncode(data);
        }

        public static String RemoveConsecutiveDashes(this String data)
        {
            String original = string.Empty;
            String current = data;

            do
            {
                original = current;
                current = current.Replace("--", "-");
            } while (current != original);

            return current;
        }

        public static String RemoveTrailingDash(this String data)
        {
            string result = data;
            if (data is not null && data.EndsWith("-", StringComparison.InvariantCulture))
                result = data.Substring(0, data.Length - 1);
            return result;
        }
    }
}
