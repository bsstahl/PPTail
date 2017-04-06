using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Common.Extensions
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

    }
}
