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
        IServiceProvider _serviceProvider;
        IEnumerable<Template> _templates;

        public SyndicationProvider(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<IContentRepository>();
            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<ITemplateProcessor>();

            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
            _templates.Validate(Enumerations.TemplateType.Syndication);
            _templates.Validate(Enumerations.TemplateType.SyndicationItem);
        }

        public string GenerateFeed(IEnumerable<ContentItem> posts)
        {
            var syndicationTemplate = _templates.Find(Enumerations.TemplateType.Syndication);
            var syndicationItemTemplate = _templates.Find(Enumerations.TemplateType.SyndicationItem);

            // var siteSettings = _serviceProvider.GetService<SiteSettings>();
            var settings = _serviceProvider.GetService<ISettings>();
            var contentRepo = _serviceProvider.GetContentRepository(settings.SourceConnection);
            var siteSettings = contentRepo.GetSiteSettings();

            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            return templateProcessor.Process(syndicationTemplate, syndicationItemTemplate, string.Empty, string.Empty, posts, "Syndication", ".", string.Empty, true, siteSettings.PostsPerFeed);
        }
    }
}
