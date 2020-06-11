using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Generator.TagCloudStyler.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IEnumerable<string> GetTagList(this IEnumerable<string> ignore)
        {
            return ignore.GetTagList(50.GetRandom(30));
        }

        public static IEnumerable<string> GetTagList(this IEnumerable<string> ignore, Int32 count)
        {
            var result = new List<string>();
            for (Int32 i = 0; i < count; i++)
                result.Add(string.Empty.GetRandom());
            return result;
        }
    }
}
