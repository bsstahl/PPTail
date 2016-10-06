using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Common.Test
{
    public static class Extensions
    {
        public static ContentItem Create(this ContentItem ignore, DateTime pubDate)
        {
            return new ContentItem()
            {
                Author = string.Empty.GetRandom(),
                CategoryIds = new List<Guid>() { Guid.NewGuid() },
                Content = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                IsPublished = true,
                LastModificationDate = pubDate.AddMinutes(10.GetRandom()),
                PublicationDate = pubDate,
                Slug = string.Empty.GetRandom(),
                Tags = (null as IEnumerable<string>).CreateTags(),
                Title = string.Empty.GetRandom()
            };
        }

        public static IEnumerable<string> CreateTags(this IEnumerable<string> ignore)
        {
            var result = new List<string>();
            int count = 10.GetRandom(1);
            for (int i = 0; i < count; i++)
                result.Add(string.Empty.GetRandom());
            return result;
        }

    }
}
