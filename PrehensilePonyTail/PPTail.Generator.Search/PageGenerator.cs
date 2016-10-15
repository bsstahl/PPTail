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
        ISettings _settings;
        SiteSettings _siteSettings;

        public PageGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (serviceProvider == null)
                throw new ArgumentNullException("IServiceProvider");

            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<SiteSettings>();

            _settings = _serviceProvider.GetService<ISettings>();
            _siteSettings = serviceProvider.GetService<SiteSettings>();

            // Guard code for a null _templates variable is not required
            // because the Service Provider will return an empty array
            // if the templates collection has not been added to the container
            _templates = serviceProvider.GetService<IEnumerable<Template>>();
            _searchTemplate = _templates.Find(Enumerations.TemplateType.SearchPage);
            _itemTemplate = _templates.Find(Enumerations.TemplateType.Item);
        }

        public string GenerateSearchResultsPage(string tag, IEnumerable<ContentItem> contentItems, string navigationContent, string sidebarContent, string pathToRoot)
        {
            var posts = contentItems.Where(i => i.Tags.Contains(tag));
            return posts.ProcessTemplate(_settings, _siteSettings, _searchTemplate, _itemTemplate, sidebarContent, navigationContent, $"Tag: {tag}", 0);
        }

    }
}
