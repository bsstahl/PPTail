using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class CategoryExtensions
    {
        public static string CategoryLinkList(this IEnumerable<Category> categories, IEnumerable<Guid> selectedCategoryIds, ISettings settings, string pathToRoot, string cssClass)
        {
            var results = string.Empty;
            var selectedCategories = categories.Where(c => selectedCategoryIds.Contains(c.Id));
            foreach (var category in selectedCategories)
                results += $"{settings.CreateSearchLink(pathToRoot, category.Name, "Category", cssClass)}&nbsp;";
            return results;
        }

    }
}
