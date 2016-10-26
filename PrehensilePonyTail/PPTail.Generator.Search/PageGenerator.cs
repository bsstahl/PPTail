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

            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<SiteSettings>();


            // Guard code for a null _templates variable is not required
            // because the Service Provider will return an empty array
            // if the templates collection has not been added to the container
            _templates = serviceProvider.GetService<IEnumerable<Template>>();
            _searchTemplate = _templates.Find(Enumerations.TemplateType.SearchPage);
            _itemTemplate = _templates.Find(Enumerations.TemplateType.Item);
        }

        public string GenerateSearchResultsPage(string tag, IEnumerable<ContentItem> contentItems, string navigationContent, string sidebarContent, string pathToRoot)
        {
            var categories = _serviceProvider.GetService<IEnumerable<Category>>();
            var settings = _serviceProvider.GetService<ISettings>();
            var siteSettings = _serviceProvider.GetService<SiteSettings>();

            var category = categories.SingleOrDefault(c => c.Name.ToLower() == tag.ToLower());
            Guid categoryId = (category == null) ? Guid.Empty : category.Id;
            var posts = contentItems.Where(i => i.Tags.Contains(tag) || i.CategoryIds.Contains(categoryId));
            return posts.ProcessTemplate(_serviceProvider, _searchTemplate, _itemTemplate, sidebarContent, navigationContent, $"Tag: {tag}", pathToRoot, false, siteSettings.PostsPerPage);
        }

    }
}
