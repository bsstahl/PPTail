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
                throw new Exceptions.DependencyNotFoundException("IServiceProvider");

            //TODO: Add test coverage
            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
            if (_templates == null || !_templates.Any(s => s.TemplateType == Enumerations.TemplateType.ContactPage))
                throw new Exceptions.DependencyNotFoundException("ContactPageTemplate");

            _siteSettings = _serviceProvider.GetService<SiteSettings>();
            if (_siteSettings == null)
                throw new Exceptions.DependencyNotFoundException("SiteSettings");

            // TODO: Add test coverage
            _settings = _serviceProvider.GetService<ISettings>();
            if (_settings == null)
                throw new Exceptions.DependencyNotFoundException("Settings");
        }

        public string GenerateContactPage(string navigationContent, string sidebarContent, string pathToRoot)
        {
            // TODO: Make it so this can handle multiple templates of the same type
            var template = _templates.Single(t => t.TemplateType == Enumerations.TemplateType.ContactPage);
            return template.ProcessNonContentItemTemplate(sidebarContent, navigationContent, _siteSettings, _settings, string.Empty, "Contact Me");
        }
    }
}
