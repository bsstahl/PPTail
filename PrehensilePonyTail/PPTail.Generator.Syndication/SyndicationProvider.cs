using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Syndication
{
    public class SyndicationProvider: ISyndicationProvider
    {
        readonly IServiceProvider _serviceProvider;
        readonly IEnumerable<Template> _templates;

        public SyndicationProvider(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<IContentRepository>();
            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<ITemplateProcessor>();

            _templates = _serviceProvider.GetTemplates();
            _templates.Validate(Enumerations.TemplateType.Syndication);
            _templates.Validate(Enumerations.TemplateType.SyndicationItem);
        }

        public String GenerateFeed(IEnumerable<ContentItem> posts)
        {
            var syndicationTemplate = _templates.Find(Enumerations.TemplateType.Syndication);
            var syndicationItemTemplate = _templates.Find(Enumerations.TemplateType.SyndicationItem);

            // var siteSettings = _serviceProvider.GetService<SiteSettings>();
            var settings = _serviceProvider.GetService<ISettings>();
            var contentRepo = _serviceProvider.GetContentRepository(settings.SourceConnection);
            var siteSettings = contentRepo.GetSiteSettings();

            // HACK: Get the fully qualified link to the site from Settings
            // It should be Settings and not SiteSettings because it will be different
            // depending on the execution context
            var pathToRoot = "https://www.cognitiveinheritance.com/";

            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            return templateProcessor.Process(syndicationTemplate, syndicationItemTemplate, string.Empty, string.Empty, posts, "Syndication", pathToRoot, string.Empty, true, siteSettings.PostsPerFeed);
        }
    }
}
