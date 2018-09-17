using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;

namespace PPTail.Generator.Contact
{
    public class TemplateProvider: IContactProvider
    {
        IServiceProvider _serviceProvider;
        IEnumerable<Template> _templates;
        SiteSettings _siteSettings;
        ISettings _settings;

        public TemplateProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (_serviceProvider == null)
                throw new ArgumentNullException("IServiceProvider");

            _serviceProvider.ValidateService<ISettings>();

            // _siteSettings = _serviceProvider.GetService<SiteSettings>();
            _settings = _serviceProvider.GetService<ISettings>();

            // Guard code for a null _templates variable is not required
            // because the Service Provider will return an empty array
            // if the templates collection has not been added to the container
            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
            _templates.Validate(Enumerations.TemplateType.ContactPage);
        }

        public string GenerateContactPage(string navigationContent, string sidebarContent, string pathToRoot)
        {
            // TODO: Handle multiple templates of the same type
            var template = _templates.Single(t => t.TemplateType == Enumerations.TemplateType.ContactPage);
            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            return templateProcessor.ProcessNonContentItemTemplate(template, sidebarContent, navigationContent, string.Empty, "Contact Me");
        }
    }
}
