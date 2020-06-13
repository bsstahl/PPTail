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
        const string _connectionStringFilePathKey = "FilePath";

        private readonly IServiceProvider _serviceProvider;
        private readonly String _templatePath;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be fixed in Globalization effort")]
        public ReadRepository(IServiceProvider serviceProvider, String templateConnection)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;

            _templatePath = templateConnection.GetConnectionStringValue(_connectionStringFilePathKey);
            if (String.IsNullOrWhiteSpace(_templatePath))
                throw new ArgumentException("FilePath not supplied in Template Connection String", nameof(templateConnection));
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            // Uses the FileSystem provider under the covers
            string connectionString = $"Provider=PPTail.Templates.FileSystem.ReadRepository;FilePath={_templatePath}";
            var templateProvider = new PPTail.Templates.FileSystem.ReadRepository(_serviceProvider, connectionString);
            var yamlTemplates = templateProvider.GetAllTemplates();

            // Modify parsed YAML
            var templates = yamlTemplates
                .Select(t => t.ToHtmlContent());

            return templates;
        }
    }
}
