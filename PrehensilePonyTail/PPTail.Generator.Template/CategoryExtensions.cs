using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.Template
{
    public static class CategoryExtensions
    {
        internal static String CategoryLinkList(this IEnumerable<Category> categories, IServiceProvider serviceProvider, IEnumerable<Guid> selectedCategoryIds, ISettings settings, String pathToRoot, String cssClass)
        {
            var results = string.Empty;
            if (selectedCategoryIds.IsNotNull() && selectedCategoryIds.Any())
            {
                var selectedCategories = categories.Where(c => selectedCategoryIds.Contains(c.Id));
                foreach (var category in selectedCategories)
                    results += $"{category.Name.ToLower().CreateSearchLink(serviceProvider, pathToRoot, "Category", cssClass)}&nbsp;";
            }
            return results;
        }

    }
}
