using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Redirect
{
    public class RedirectProvider : IRedirectProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<Template> _templates;

        public RedirectProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _templates = serviceProvider.GetTemplates();
            _templates.Validate(Enumerations.TemplateType.Redirect);
        }

        public String GenerateRedirect(String redirectToUrl)
        {
            var template = _templates.Find(Enumerations.TemplateType.Redirect);
            return template.Content.Replace("{Url}", redirectToUrl);
        }
    }
}
