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
        private readonly IServiceProvider _serviceProvider;
        private readonly String _templatePath;

        public ReadRepository(IServiceProvider serviceProvider, string templatePath)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _templatePath = templatePath;

            _serviceProvider.ValidateService<IFile>();
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            var fileProvider = _serviceProvider.GetService<IFile>();
            return new TemplateCollectionBuilder(fileProvider, _templatePath)
                .AddTemplate(TemplateType.Style, "Style.template.css")
                .AddTemplate(TemplateType.Bootstrap, "bootstrap.min.css")
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
                .Build();
        }
    }
}
