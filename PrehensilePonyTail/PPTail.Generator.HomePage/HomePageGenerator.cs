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
        private readonly INavigationProvider _navProvider;
        private readonly ISettings _settings;
        private readonly IEnumerable<Template> _templates;

        public HomePageGenerator(IServiceProvider serviceProvider)
        {
            // Note: Validation that required templates have been supplied
            // is being done in the methods where they are required

            // TODO: Move the service validation into the methods where they are required

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<ISettings>();

            _settings = _serviceProvider.GetService<ISettings>();
            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
        }

        public string GenerateHomepage(string sidebarContent, string navigationContent, SiteSettings siteSettings, IEnumerable<ContentItem> posts)
        {
            var homepageTemplate = _templates.Find(Enumerations.TemplateType.HomePage);
            var itemTemplate = _templates.Find(Enumerations.TemplateType.Item);
            var categories = _serviceProvider.GetService<IEnumerable<Category>>();
            var settings = _serviceProvider.GetService<ISettings>();
            return posts.ProcessTemplate(_settings, siteSettings, categories, homepageTemplate, itemTemplate, sidebarContent, navigationContent, "Home", siteSettings.PostsPerPage, ".", settings.ItemSeparator, false);
        }

    }
}