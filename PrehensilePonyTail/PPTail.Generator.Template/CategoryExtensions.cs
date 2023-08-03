using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PPTail.Entities;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPTail.Generator.Template
{
    public static class CategoryExtensions
    {
        internal static String CategoryLinkList(this IEnumerable<Category> categories, IServiceProvider serviceProvider, IEnumerable<Guid> selectedCategoryIds, SiteSettings siteSettings, String pathToRoot, String cssClass)
        {
            var results = string.Empty;

            var logger = serviceProvider.GetService<ILogger<TemplateProcessor>>();

            if (selectedCategoryIds.IsNotNull() && selectedCategoryIds.Any())
            {
                if (logger.IsNotNull())
                    logger.LogInformation("Categories: {Categories} - SelectedCategoryIds: {SelectedCategoryIds}", categories, selectedCategoryIds);

                var selectedCategories = categories.Where(c => selectedCategoryIds.Contains(c.Id));
                if (logger.IsNotNull())
                    logger.LogInformation("Selected Categories: {SelectedCategories}", selectedCategories);

                foreach (var category in selectedCategories)
                    results += $"{category.Name.ToLower().CreateSearchLink(serviceProvider, pathToRoot, "Category", cssClass)}&nbsp;";
            
                if (logger.IsNotNull())
                    logger.LogInformation("Category Link List: {CategoryLinkList}", results);
            }

            return results;
        }

    }
}
