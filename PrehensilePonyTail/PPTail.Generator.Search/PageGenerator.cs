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
                throw new DependencyNotFoundException("IServiceProvider");

            // Add test coverage
            _templates = serviceProvider.GetService<IEnumerable<Template>>();
            if (_templates == null || !_templates.Any())
                throw new DependencyNotFoundException("Templates");

            _settings = serviceProvider.GetService<ISettings>();
            if (_settings == null)
                throw new DependencyNotFoundException("Settings");

            _siteSettings = serviceProvider.GetService<SiteSettings>();
            if (_siteSettings == null)
                throw new DependencyNotFoundException("SiteSettings");

            _searchTemplate = _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.SearchPage);
            if (_searchTemplate == null)
                throw new TemplateNotFoundException(Enumerations.TemplateType.SearchPage, "SearchPage");

            _itemTemplate = _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.Item);
            if (_itemTemplate == null)
                throw new TemplateNotFoundException(Enumerations.TemplateType.Item, "Item");

        }

        public string GenerateSearchResultsPage(string tag, IEnumerable<ContentItem> contentItems, string navigationContent, string sidebarContent, string pathToRoot)
        {
            var posts = contentItems.Where(i => i.Tags.Contains(tag));
            return posts.ProcessTemplate(_settings, _siteSettings, _searchTemplate, _itemTemplate, sidebarContent, navigationContent, $"Tag: {tag}", 0);
        }

    }
}
