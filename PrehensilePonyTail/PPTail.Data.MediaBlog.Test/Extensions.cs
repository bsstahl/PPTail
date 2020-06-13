using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static String GetRandomUrl(this String ignore)
        {
            return $"http://www.{string.Empty.GetRandom()}.com/{string.Empty.GetRandom()}";
        }

        public static String AsHash(this IEnumerable<String> value)
        {
            return string.Join(';', value.OrderBy(t => t));
        }
    }
}
