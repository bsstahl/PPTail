using System;
using System.Collections.Generic;
using System.Text;
using PPTail.Builders;
using TestHelperExtensions;

namespace PPTail.Data.Forestry.Test
{
    public static class CategoriesBuilderExtensions
    {
        public static CategoryCollectionBuilder AddRandomCategories(this CategoryCollectionBuilder builder)
        {
            return builder.AddRandomCategories(10.GetRandom(5));            
        }

        public static CategoryCollectionBuilder AddRandomCategories(this CategoryCollectionBuilder builder, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Guid id = Guid.NewGuid();
                string name = string.Empty.GetRandom();
                string description = $"{id}_{name}";
                builder.AddCategory(id, name, description);
            }

            return builder;
        }
    }
}
