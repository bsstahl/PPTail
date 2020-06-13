using PPTail.Builders;
using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Generator.Navigation.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static BasicProvider Create(this BasicProvider ignore, IServiceProvider serviceProvider)
        {
            return new BasicProvider(serviceProvider);
        }

        public static BootstrapProvider Create(this BootstrapProvider ignore, IServiceProvider serviceProvider)
        {
            return new BootstrapProvider(serviceProvider);
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore)
        {
            return ignore.Create(5.GetRandom(3));
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, Int32 count)
        {
            var result = new List<ContentItem>();
            for (Int32 i = 0; i < count; i++)
                result.Add(new ContentItem()
                {
                    Title = string.Empty.GetRandom(),
                    Slug = string.Empty.GetRandom(),
                    IsPublished = true,
                    ShowInList = true
                });
            return result;
        }

    }
}
