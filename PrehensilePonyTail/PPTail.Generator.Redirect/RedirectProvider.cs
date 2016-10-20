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
        IServiceProvider _serviceProvider;
        IEnumerable<Template> _templates;

        public RedirectProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            // Guard code for a null _templates variable is not required
            // because the Service Provider will return an empty array
            // if the templates collection has not been added to the container
            _templates = serviceProvider.GetService<IEnumerable<Template>>();
            _templates.Validate(Enumerations.TemplateType.Redirect);
        }

        public string GenerateRedirect(string redirectToUrl)
        {
            var template = _templates.Find(Enumerations.TemplateType.Redirect);
            return template.Content.Replace("{Url}", redirectToUrl);
        }
    }
}
