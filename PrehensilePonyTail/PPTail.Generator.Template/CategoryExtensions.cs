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

            if (selectedCategoryIds is not null && selectedCategoryIds.Any())
            {
                if (logger is not null)
                    logger.LogInformation("Categories: {Categories} - SelectedCategoryIds: {SelectedCategoryIds}", categories, selectedCategoryIds);

                var selectedCategories = categories.Where(c => selectedCategoryIds.Contains(c.Id));
                if (logger is not null)
                    logger.LogInformation("Selected Categories: {SelectedCategories}", selectedCategories);

                foreach (var category in selectedCategories)
                    results += $"{category.Name.ToLower().CreateSearchLink(serviceProvider, pathToRoot, "Category", cssClass)}&nbsp;";
            
                if (logger is not null)
                    logger.LogInformation("Category Link List: {CategoryLinkList}", results);
            }

            return results;
        }

    }
}
