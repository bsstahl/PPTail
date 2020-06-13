using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;
using PPTail.Enumerations;

namespace PPTail.Templates.FileSystem
{
    public class ReadRepository : Interfaces.ITemplateRepository
    {
        const string _connectionStringFilePathKey = "FilePath";

        private readonly IServiceProvider _serviceProvider;
        private readonly String _templatePath;

        private IEnumerable<Template>? _templates = null;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be corrected in Globalization project")]
        public ReadRepository(IServiceProvider serviceProvider, string templateConnection)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _templatePath = templateConnection.GetConnectionStringValue(_connectionStringFilePathKey);

            _serviceProvider.ValidateService<IFile>();

            if (string.IsNullOrWhiteSpace(_templatePath))
                throw new ArgumentException("FilePath not provided in Template Connection String", nameof(templateConnection));
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            if (_templates is null)
            {
                var fileProvider = _serviceProvider.GetService<IFile>();
                _templates = new TemplateCollectionBuilder(fileProvider, _templatePath)
                    .AddTemplate(TemplateType.Style, "Style.template.css")
                    .AddTemplate(TemplateType.HomePage, "HomePage.template.html")
                    .AddTemplate(TemplateType.SearchPage, "SearchPage.template.html")
                    .AddTemplate(TemplateType.ContentPage, "ContentPage.template.html")
                    .AddTemplate(TemplateType.PostPage, "PostPage.template.html")
                    .AddTemplate(TemplateType.Redirect, "Redirect.template.html")
                    .AddTemplate(TemplateType.Archive, "Archive.template.html")
                    .AddTemplate(TemplateType.ArchiveItem, "ArchiveItem.template.html")
                    .AddTemplate(TemplateType.Syndication, "Syndication.template.xml")
                    .AddTemplate(TemplateType.SyndicationItem, "SyndicationItem.template.xml")
                    .AddTemplate(TemplateType.ContactPage, "ContactPage.template.html")
                    .AddTemplate(TemplateType.Item, "ContentItem.template.html")
                    // .AddTemplate(TemplateType.Bootstrap, "bootstrap.min.css")
                    .Build();
            }
            return _templates;
        }
    }
}
