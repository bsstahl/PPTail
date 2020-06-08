using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;

namespace PPTail.Templates.FileSystem
{
    public class ReadRepository : Interfaces.ITemplateRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly String _templatePath;

        public ReadRepository(IServiceProvider serviceProvider, string templatePath)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _templatePath = templatePath;

            _serviceProvider.ValidateService<IDirectory>();
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            var directory = _serviceProvider.GetService<IDirectory>();
            var files = directory.EnumerateFiles(_templatePath);

            // TODO: Implement

            return new List<Template>();
        }
    }
}
