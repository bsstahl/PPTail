using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.Ef
{
    public static class StringExtensions
    {
        public static IEnumerable<string> GetTags(this string tagString)
        {
            var rawTags = string.IsNullOrWhiteSpace(tagString) ? new string[] { } : tagString.Split(';');
            var result = new List<string>();
            foreach (var item in rawTags)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    result.Add(item);
            }
            return result;
        }
    }
}
