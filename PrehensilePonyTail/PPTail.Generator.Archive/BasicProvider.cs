using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Enumerations;
using PPTail.Extensions;

namespace PPTail.Generator.Archive
{
    public class BasicProvider : IArchiveProvider
    {
        private IServiceProvider _serviceProvider;
        private IEnumerable<Template> _templates;

        public BasicProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (_serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            // TODO: Add code coverage
            _serviceProvider.ValidateService<ITemplateProcessor>();
            _serviceProvider.ValidateService<SiteSettings>();

            _templates = serviceProvider.GetService<IEnumerable<Template>>();
            _templates.Validate(TemplateType.Archive);
            _templates.Validate(TemplateType.ArchiveItem);
        }

        public string GenerateArchive(IEnumerable<ContentItem> posts, IEnumerable<ContentItem> pages, string navContent, string sidebarContent, string pathToRoot)
        {
            var pageTemplate = _templates.Find(TemplateType.Archive);
            var itemTemplate = _templates.Find(TemplateType.ArchiveItem);

            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            return templateProcessor.Process(pageTemplate, itemTemplate, sidebarContent, navContent, posts, "Archive", ".", string.Empty, false, 0);
        }

    }
}
