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
        IServiceProvider _serviceProvider;
        IEnumerable<Template> _templates;
        Template _searchTemplate;
        Template _itemTemplate;

        public PageGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (serviceProvider == null)
                throw new ArgumentNullException("IServiceProvider");

            _serviceProvider.ValidateService<SiteSettings>();
            _serviceProvider.ValidateService<ITemplateProcessor>();
            _serviceProvider.ValidateService<ISettings>(); // TODO: Add code coverage

            // Guard code for a null result for the 
            // IEnumerable<Category> or IEnumerable<Template> variables
            // is not required because the Service Provider will return an empty
            // array if the collections have not been added to the container
            _templates = serviceProvider.GetService<IEnumerable<Template>>();
            _searchTemplate = _templates.Find(Enumerations.TemplateType.SearchPage);
            _itemTemplate = _templates.Find(Enumerations.TemplateType.Item);
        }

        public string GenerateSearchResultsPage(string tag, IEnumerable<ContentItem> contentItems, string navigationContent, string sidebarContent, string pathToRoot)
        {
            var categories = _serviceProvider.GetService<IEnumerable<Category>>();
            var siteSettings = _serviceProvider.GetService<SiteSettings>();
            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            var settings = _serviceProvider.GetService<ISettings>();

            var category = categories.SingleOrDefault(c => c.Name.ToLower() == tag.ToLower());
            Guid categoryId = (category == null) ? Guid.Empty : category.Id;
            var posts = contentItems.Where(i => i.Tags.Contains(tag) || i.CategoryIds.Contains(categoryId));
            return templateProcessor.Process(_searchTemplate, _itemTemplate, sidebarContent, navigationContent, posts, $"Tag: {tag}", pathToRoot, settings.ItemSeparator, false, siteSettings.PostsPerPage);
        }

    }
}
