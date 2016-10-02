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
        private readonly Settings _settings;
        private readonly IEnumerable<Template> _templates;

        public PageGenerator(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _settings = _serviceProvider.GetService<Settings>();
            if (_settings == null)
                throw new Exceptions.DependencyNotFoundException(nameof(Settings));

            _templates = _serviceProvider.GetService<IEnumerable<Template>>();
            if (!_templates.Any())
                throw new Exceptions.DependencyNotFoundException("IEnumerable<Template>");

            _navProvider = _serviceProvider.GetService<INavigationProvider>();
            if (_navProvider == null)
                throw new Exceptions.DependencyNotFoundException("INavigationProvider");
        }

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


        private string DateTimeFormatSpecifier
        {
            get
            {
                return _settings.DateTimeFormatSpecifier;
            }
        }

        private string ItemSeparator
        {
            get
            {
                return _settings.ItemSeparator;
            }
        }


        public string GenerateHomepage(string sidebarContent, string navigationContent, SiteSettings siteSettings, IEnumerable<ContentItem> posts)
        {
            return posts.ProcessTemplate(_settings, siteSettings, this.HomePageTemplate, this.ItemTemplate, sidebarContent, navigationContent, "Home", siteSettings.PostsPerPage);
        }

        public string GenerateStylesheet(SiteSettings siteSettings)
        {
            //TODO: Process template against additional data (such as Settings and SiteSettings)
            return this.StyleTemplate.Content;
        }

        public string GenerateBootstrapPage()
        {
            if (this.BootstrapTemplate == null)
                return string.Empty;
            else
                return this.BootstrapTemplate.Content;
        }

        public string GenerateSidebarContent(Settings settings, SiteSettings siteSettings, IEnumerable<ContentItem> posts, IEnumerable<ContentItem> pages, IEnumerable<Widget> widgets, string pathToRoot)
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
