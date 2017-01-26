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

        public static IEnumerable<Guid> GetCategoryIds(this string idString)
        {
            var result = new List<Guid>();
            var rawIds = string.IsNullOrWhiteSpace(idString) ? new string[] { } : idString.Split(';');
            foreach (var item in rawIds)
            {
                Guid thisGuid;
                if (!string.IsNullOrWhiteSpace(item) && Guid.TryParse(item, out thisGuid))
                    result.Add(new Guid(item));
            }
            return result;
        }
    }
}
