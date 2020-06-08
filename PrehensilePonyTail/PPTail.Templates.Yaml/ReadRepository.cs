using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using PPTail.Exceptions;
using PPTail.Extensions;

namespace PPTail.Templates.Yaml
{
    public class ReadRepository : Interfaces.ITemplateRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly String _templatePath;

        public ReadRepository(IServiceProvider serviceProvider, String templatePath)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _templatePath = templatePath;
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            // Uses the FileSystem provider under the covers
            var templateProvider = new PPTail.Templates.FileSystem.ReadRepository(_serviceProvider, _templatePath);

            // TODO: Modify parsed YAML

            return templateProvider.GetAllTemplates();
        }
    }
}
