using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Web.Permalinks
{
    public static class HeaderDictionaryExtensions
    {
        public static void AddHeader(this IHeaderDictionary headers, string key, string value)
        {
            var values = new Microsoft.Extensions.Primitives.StringValues(value);
            var pair = new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>(key, values);
            headers.Add(pair);
        }
    }
}
