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
            _serviceProvider.ValidateService<SiteSettings>();
            _serviceProvider.ValidateService<ITemplateProcessor>();

            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
            _templates.Validate(TemplateType.HomePage);
            _templates.Validate(TemplateType.Item);
        }

        public string GenerateHomepage(string sidebarContent, string navigationContent, IEnumerable<ContentItem> posts)
        {
            var homepageTemplate = _templates.Find(Enumerations.TemplateType.HomePage);
            var itemTemplate = _templates.Find(Enumerations.TemplateType.Item);
            var siteSettings = _serviceProvider.GetService<SiteSettings>();
            var templateProcessor = _serviceProvider.GetService<ITemplateProcessor>();
            return templateProcessor.Process(homepageTemplate, itemTemplate, sidebarContent, navigationContent, posts, "Home", ".", false, siteSettings.PostsPerPage);
        }

    }
}