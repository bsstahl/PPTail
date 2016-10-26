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
            _serviceProvider.ValidateService<SiteSettings>();
            _serviceProvider.ValidateService<ISettings>();

            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
            _templates.Validate(Enumerations.TemplateType.Syndication);
            _templates.Validate(Enumerations.TemplateType.SyndicationItem);
        }

        public string GenerateFeed(IEnumerable<ContentItem> posts)
        {
            var syndicationTemplate = _templates.Find(Enumerations.TemplateType.Syndication);
            var syndicationItemTemplate = _templates.Find(Enumerations.TemplateType.SyndicationItem);
            var settings = _serviceProvider.GetService<ISettings>();
            var siteSettings = _serviceProvider.GetService<SiteSettings>();
            var categories = _serviceProvider.GetService<IEnumerable<Category>>();

            return posts.ProcessTemplate(_serviceProvider, syndicationTemplate, syndicationItemTemplate, string.Empty, string.Empty, "Syndication", ".", true, siteSettings.PostsPerFeed);
        }
    }
}
