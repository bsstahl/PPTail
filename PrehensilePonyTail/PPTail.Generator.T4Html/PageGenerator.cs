using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;
using PPTail.Interfaces;
using PPTail.Extensions;

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

        #region Properties 

        private Template ContentPageTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.ContentPage);
            }
        }

        private Template PostPageTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.PostPage);
            }
        }

        private Template HomePageTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.HomePage);
            }
        }

        private Template ItemTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.Item);
            }
        }

        private Template StyleTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.Style);
            }
        }

        private Template BootstrapTemplate
        {
            get
            {
                return _templates.SingleOrDefault(t => t.TemplateType == Enumerations.TemplateType.Bootstrap);
            }
        }

        #endregion

        public string GenerateHomepage(string sidebarContent, string navigationContent, SiteSettings siteSettings, IEnumerable<ContentItem> posts)
        {
            _templates.Validate(Enumerations.TemplateType.HomePage);
            _templates.Validate(Enumerations.TemplateType.Item);
            return posts.ProcessTemplate(_settings, siteSettings, this.HomePageTemplate, this.ItemTemplate, sidebarContent, navigationContent, "Home", siteSettings.PostsPerPage);
        }

        public string GenerateStylesheet(SiteSettings siteSettings)
        {
            //TODO: Process template against additional data (such as Settings and SiteSettings)
            _templates.Validate(Enumerations.TemplateType.Style);
            return this.StyleTemplate.Content;
        }

        public string GenerateBootstrapPage()
        {
            if (this.BootstrapTemplate == null)
                return string.Empty;
            else
                return this.BootstrapTemplate.Content;
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
            var template = this.ContentPageTemplate;
            if (template == null)
                throw new TemplateNotFoundException(Enumerations.TemplateType.ContentPage, string.Empty);

            return template.ProcessContentItemTemplate(pageData, sidebarContent, navContent, siteSettings, _settings);
        }

        public string GeneratePostPage(string sidebarContent, string navContent, SiteSettings siteSettings, ContentItem article)
        {
            var template = this.PostPageTemplate;
            if (template == null)
                throw new TemplateNotFoundException(Enumerations.TemplateType.PostPage, string.Empty);

            return template.ProcessContentItemTemplate(article, sidebarContent, navContent, siteSettings, _settings);
        }

    }
}
