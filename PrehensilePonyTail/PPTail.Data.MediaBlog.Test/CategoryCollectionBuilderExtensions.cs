using System;
using System.Collections.Generic;
using System.Text;
using PPTail.Builders;

namespace PPTail.Data.MediaBlog.Test
{
    public static class CategoryCollectionBuilderExtensions
    {
        public static CategoryCollectionBuilder AddRandomCategories(this CategoryCollectionBuilder builder, int count)
        {
            for (int i = 0; i < count; i++)
            {
                builder.AddCategory(new CategoryBuilder()
                    .UseRandom()
                    .Build());
            }

            return builder;
        }
    }
}
