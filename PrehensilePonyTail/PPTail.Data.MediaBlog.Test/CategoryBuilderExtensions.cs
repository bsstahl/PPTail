using PPTail.Common.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
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
