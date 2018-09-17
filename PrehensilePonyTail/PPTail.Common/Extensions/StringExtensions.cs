using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<Tuple<string, int>> GetTagCounts(this IEnumerable<string> tags)
        {
            var tagCounts = new List<Tuple<string, int>>();
            foreach (var tag in tags)
            {
                int startingCount = 0;
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


        public static string GetConnectionStringValue(this string connectionString, string key)
        {
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

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
    }
}
