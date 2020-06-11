using System;
using System.Collections.Generic;
using System.Text;
using PPTail.Builders;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class CategoryBuilderExtensions
    {
        public static CategoryBuilder UseRandom(this CategoryBuilder builder)
        {
            return builder
                .Id(Guid.NewGuid())
                .Name(string.Empty.GetRandom())
                .Description(string.Empty.GetRandom());
        }
    }
}
