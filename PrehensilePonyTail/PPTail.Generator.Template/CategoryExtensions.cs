using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.Template
{
    public static class CategoryExtensions
    {
        internal static string CategoryLinkList(this IEnumerable<Category> categories, IServiceProvider serviceProvider, IEnumerable<Guid> selectedCategoryIds, ISettings settings, string pathToRoot, string cssClass)
        {
            var results = string.Empty;
            var selectedCategories = categories.Where(c => selectedCategoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
                results += $"{category.Name.CreateSearchLink(serviceProvider, pathToRoot, "Category", cssClass)}&nbsp;";
            return results;
        }

    }
}
