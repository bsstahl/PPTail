using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.TagCloudStyler
{
    public class DeviationStyler : Interfaces.ITagCloudStyler
    {
        public DeviationStyler(IServiceProvider serviceProvider)
        {
        }

        public IEnumerable<Tuple<string, string>> GetStyles(IEnumerable<string> tags)
        {
            var tagCounts = GetTagCounts(tags);
            var values = tagCounts.Select(t => t.Item2);
            var average = tagCounts.Average(t => t.Item2);
            var sum = values.Sum(d => (d - average) * (d - average));
            var stdev = Math.Sqrt(sum / values.Count());

            var plus1Stdev = average + stdev;
            var plus2Stdev = plus1Stdev + stdev;
            var minus1Stdev = average - stdev;

            var results = new List<Tuple<string, string>>();
            foreach (var tagCount in tagCounts)
            {
                var result = "smallest";
                if (stdev > 0.0)
                {
                    if (tagCount.Item2 < minus1Stdev)
                        result = "smallest";
                    else if (tagCount.Item2 < average)
                        result = "small";
                    else if (tagCount.Item2 < plus1Stdev)
                        result = "medium";
                    else if (tagCount.Item2 < plus2Stdev)
                        result = "big";
                    else
                        result = "biggest";
                }

                results.Add(new Tuple<string, string>(tagCount.Item1, result));
            }

            return results;
        }

        private static IEnumerable<Tuple<string, int>> GetTagCounts(IEnumerable<string> tags)
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
