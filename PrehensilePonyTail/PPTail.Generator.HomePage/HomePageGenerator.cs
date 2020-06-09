using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;
using PPTail.Interfaces;
using PPTail.Extensions;
using PPTail.Enumerations;

namespace PPTail.Generator.HomePage
{
    public class HomePageGenerator : Interfaces.IHomePageGenerator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<Template> _templates;

        public HomePageGenerator(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<IContentRepository>();
            _serviceProvider.ValidateService<ITemplateProcessor>();
            _serviceProvider.ValidateService<ISettings>(); // TODO: Add code coverage

            _templates = _serviceProvider.GetTemplates();
            _templates.Validate(TemplateType.HomePage);
            _templates.Validate(TemplateType.Item);

        }

        public String GenerateHomepage(String sidebarContent, String navigationContent, IEnumerable<ContentItem> posts)
        {
            var homepageTemplate = _templates.Find(Enumerations.TemplateType.HomePage);
            var itemTemplate = _templates.Find(Enumerations.TemplateType.Item);
            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            var settings = _serviceProvider.GetService<ISettings>();

            var contentRepo = _serviceProvider.GetContentRepository(settings.SourceConnection);
            var siteSettings = contentRepo.GetSiteSettings(); 

            return templateProcessor.Process(homepageTemplate, itemTemplate, sidebarContent, navigationContent, posts, "Home", ".", settings.ItemSeparator, false, siteSettings.PostsPerPage);
        }

    }
}