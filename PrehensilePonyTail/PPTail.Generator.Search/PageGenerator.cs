using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;
using PPTail.Extensions;
using PPTail.Interfaces;

namespace PPTail.Generator.Search
{
    public class PageGenerator: Interfaces.ISearchProvider
    {
        readonly IServiceProvider _serviceProvider;
        readonly IEnumerable<Template> _templates;
        readonly Template _searchTemplate;
        readonly Template _itemTemplate;

        public PageGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider.ValidateService<IContentRepository>();
            _serviceProvider.ValidateService<ITemplateProcessor>();

            _templates = serviceProvider.GetTemplates();
            _searchTemplate = _templates.Find(Enumerations.TemplateType.SearchPage);
            _itemTemplate = _templates.Find(Enumerations.TemplateType.Item);
        }

        public String GenerateSearchResultsPage(String tag, IEnumerable<ContentItem> contentItems, String navigationContent, String sidebarContent, String pathToRoot)
        {
            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();

            var contentRepo = _serviceProvider.GetService<IContentRepository>();
            var siteSettings = contentRepo.GetSiteSettings();
            var categories = contentRepo.GetCategories();

            var category = categories.SingleOrDefault(c => c.Name.ToUpperInvariant() == tag.ToUpperInvariant());
            var categoryId = (category == null) ? Guid.Empty : category.Id;
            var posts = contentItems.Where(i => (i.Tags.IsNotNull() && i.Tags.Contains(tag)) || i.CategoryIds.Contains(categoryId));
            return templateProcessor.Process(_searchTemplate, _itemTemplate, sidebarContent, navigationContent, posts, $"Tag: {tag}", pathToRoot, siteSettings.ItemSeparator, false, 0);
        }

    }
}
