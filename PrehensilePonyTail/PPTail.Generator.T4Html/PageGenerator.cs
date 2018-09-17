using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;
using PPTail.Interfaces;
using PPTail.Extensions;
using PPTail.Enumerations;

namespace PPTail.Generator.T4Html
{
    public class PageGenerator : Interfaces.IPageGenerator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationProvider _navProvider;
        private readonly ISettings _settings;
        private readonly IEnumerable<Template> _templates;

        public PageGenerator(IServiceProvider serviceProvider)
        {
            // Note: Validation that required templates have been supplied
            // is being done in the methods where they are required

            // TODO: Move the service validation into the methods where they are required

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<INavigationProvider>();

            _settings = _serviceProvider.GetService<ISettings>();
            _navProvider = _serviceProvider.GetService<INavigationProvider>();

            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
        }

        public string GenerateStylesheet()
        {
            //TODO: Process template against additional data (such as Settings and SiteSettings)
            var template = _templates.Find(Enumerations.TemplateType.Style);
            return template.Content;
        }

        public string GenerateSidebarContent(IEnumerable<ContentItem> posts, IEnumerable<ContentItem> pages, IEnumerable<Widget> widgets, string pathToRoot)
        {
            _serviceProvider.ValidateService<ISettings>();
            var settings = _serviceProvider.GetService<ISettings>();

            var results = "<div class=\"widgetzone\">";
            foreach (var widget in widgets)
                results += widget.Render(_serviceProvider, settings, posts, pathToRoot);
            results += "</div>";
            return results;
        }

        public string GenerateBootstrapPage()
        {
            string result = string.Empty;
            var templateType = TemplateType.Bootstrap;
            if (_templates.Contains(templateType))
                result = _templates.Find(templateType).Content;
            return result;
        }
    }
}
