using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Templates.Yaml
{
    public class ReadRepository : Interfaces.ITemplateRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public ReadRepository(IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            throw new NotImplementedException();
        }
    }
}
