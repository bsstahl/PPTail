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

namespace PPTail.Generator.T4Html
{
    public class PageGenerator : Interfaces.IPageGenerator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationProvider _navProvider;
        private readonly ISettings _settings;
        private readonly IEnumerable<Template> _templates;

        public PageGenerator(IServiceProvider serviceProvider)
        {
            // Note: Validation that required templates have been supplied
            // is being done in the methods where they are required
            // TODO: Do the same for the service validation

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<INavigationProvider>();

            _settings = _serviceProvider.GetService<ISettings>();
            _navProvider = _serviceProvider.GetService<INavigationProvider>();

            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
        }

        public string GenerateHomepage(string sidebarContent, string navigationContent, SiteSettings siteSettings, IEnumerable<ContentItem> posts)
        {
            var homepageTemplate = _templates.Find(Enumerations.TemplateType.HomePage);
            var itemTemplate = _templates.Find(Enumerations.TemplateType.Item);
            return posts.ProcessTemplate(_settings, siteSettings, homepageTemplate, itemTemplate, sidebarContent, navigationContent, "Home", siteSettings.PostsPerPage);
        }

        public string GenerateStylesheet(SiteSettings siteSettings)
        {
            //TODO: Process template against additional data (such as Settings and SiteSettings)
            var template = _templates.Find(Enumerations.TemplateType.Style);
            return template.Content;
        }

        public string GenerateBootstrapPage()
        {
            string result = string.Empty;
            var templateType = TemplateType.Bootstrap;
            var template = _templates.SingleOrDefault(t => t.TemplateType == templateType);
            if (_templates.Contains(templateType))
                result = _templates.Find(templateType).Content;
            return result;
        }

        public string GenerateSidebarContent(ISettings settings, SiteSettings siteSettings, IEnumerable<ContentItem> posts, IEnumerable<ContentItem> pages, IEnumerable<Widget> widgets, string pathToRoot)
        {
            var results = "<div class=\"widgetzone\">";
            foreach (var widget in widgets)
                results += widget.Render(_serviceProvider, settings, posts, pathToRoot);
            results += "</div>";
            return results;
        }

        public string GenerateContentPage(string sidebarContent, string navContent, SiteSettings siteSettings, ContentItem pageData)
        {
            var template = _templates.Find(TemplateType.ContentPage);
            return template.ProcessContentItemTemplate(pageData, sidebarContent, navContent, siteSettings, _settings);
        }

        public string GeneratePostPage(string sidebarContent, string navContent, SiteSettings siteSettings, ContentItem article)
        {
            var template = _templates.Find(TemplateType.PostPage);
            return template.ProcessContentItemTemplate(article, sidebarContent, navContent, siteSettings, _settings);
        }

    }
}
