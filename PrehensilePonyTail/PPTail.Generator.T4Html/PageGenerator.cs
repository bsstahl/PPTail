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
        private readonly IEnumerable<Template> _templates;

        public PageGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _serviceProvider.ValidateService<IContentRepository>();
            _templates = _serviceProvider.GetTemplates();
        }

        public String GenerateStylesheet()
        {
            //TODO: Process template against additional data (such as SiteSettings)
            var template = _templates.Find(Enumerations.TemplateType.Style);
            return template.Content;
        }

        public String GenerateSidebarContent(IEnumerable<ContentItem> posts, IEnumerable<ContentItem> pages, IEnumerable<Widget> widgets, String pathToRoot)
        {
            var settings = _serviceProvider.GetService<ISettings>();

            var results = "<div class=\"widgetzone\">";
            foreach (var widget in widgets.OrderBy(w => w.OrderIndex))
            {
                results += widget.Render(_serviceProvider, settings, posts, pathToRoot);
            }

            results += "</div>";
            return results;
        }

        public String GenerateBootstrapPage()
        {
            var result = String.Empty;
            var templateType = TemplateType.Bootstrap;
            if (_templates.Contains(templateType))
            {
                result = _templates.Find(templateType).Content;
            }

            return result;
        }
    }
}
