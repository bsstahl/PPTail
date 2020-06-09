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
        readonly IServiceProvider _serviceProvider;
        readonly IEnumerable<Template> _templates;

        readonly ISettings _settings;

        public TemplateProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (_serviceProvider == null)
                throw new ArgumentNullException("IServiceProvider");

            _serviceProvider.ValidateService<ISettings>();

            _settings = _serviceProvider.GetService<ISettings>();

            _templates = _serviceProvider.GetTemplates();
            _templates.Validate(Enumerations.TemplateType.ContactPage);
        }

        public String GenerateContactPage(String navigationContent, String sidebarContent, String pathToRoot)
        {
            // TODO: Handle multiple templates of the same type
            var template = _templates.Single(t => t.TemplateType == Enumerations.TemplateType.ContactPage);
            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            return templateProcessor.ProcessNonContentItemTemplate(template, sidebarContent, navigationContent, string.Empty, "Contact Me", pathToRoot);
        }
    }
}
