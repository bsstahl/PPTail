using PPTail.Entities;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using PPTail.Interfaces;

namespace PPTail.Generator.Navigation
{
    public class BootstrapProvider : Interfaces.INavigationProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public BootstrapProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            if (_serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider.ValidateService<IContentRepository>();
            _serviceProvider.ValidateService<ILinkProvider>();
        }

        public String CreateNavigation(IEnumerable<ContentItem> pages, String relativePathToRootFolder, String outputFileExtension)
        {
            var contentRepo = _serviceProvider.GetService<IContentRepository>();
            var linkProvider = _serviceProvider.GetService<ILinkProvider>();

            var siteSettings = contentRepo.GetSiteSettings();

            var homePageUri = linkProvider.GetUrl(relativePathToRootFolder, "", "index", outputFileExtension);
            var archiveUri = linkProvider.GetUrl(relativePathToRootFolder, "", "archive", outputFileExtension);
            var contactUri = linkProvider.GetUrl(relativePathToRootFolder, "", "contact", outputFileExtension);
            var syndicationUri = linkProvider.GetUrl(relativePathToRootFolder, "", "syndication", "xml");
            var syndicationImageUri = linkProvider.GetUrl(relativePathToRootFolder, "Pics", "rssButton", "gif");

            var pagesToList = pages
                .Where(p => p.IsPublished && p.ShowInList)
                .OrderBy(p => p.MenuOrder)
                .ThenBy(p => p.Title);

            var sb = new StringBuilder();

            sb.AppendLine("<nav class=\"navbar navbar-expand-lg navbar-dark bg-dark\">");

            if (siteSettings.DisplayTitleInNavbar)
                sb.AppendLine($"<a class=\"navbar-brand\" href=\"{homePageUri}\">{siteSettings.Title}</a>");

            sb.AppendLine("<button class=\"navbar-toggler\" type=\"button\" data-toggle=\"collapse\" data-target=\"#navbarSupportedContent\" aria-controls=\"navbarSupportedContent\" aria-expanded=\"false\" aria-label=\"Toggle navigation\">");
            sb.AppendLine("<span class=\"navbar-toggler-icon\"></span>");
            sb.AppendLine("</button>");

            sb.AppendLine("<div class=\"collapse navbar-collapse\" id=\"navbarSupportedContent\">");
            sb.AppendLine("<ul class=\"navbar-nav mr-auto\">");

            if (!siteSettings.DisplayTitleInNavbar)
                sb.AppendRootLink("Home", homePageUri);

            sb.AppendRootLink("Archive", archiveUri);
            sb.AppendRootLink("Contact", contactUri);

            if (pagesToList.Any())
            {
                var useDropdown = siteSettings.UseAdditionalPagesDropdown;
                var childMenuName = siteSettings.AdditionalPagesDropdownLabel;

                if (useDropdown)
                {
                    sb.AppendLine("<li class=\"nav-item dropdown\">");
                    sb.AppendLine($"<a class=\"nav-link dropdown-toggle\" href=\"#\" id=\"navbarDropdown\" role=\"button\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">");
                    sb.AppendLine(childMenuName);
                    sb.AppendLine("</a>");
                    sb.AppendLine("<div class=\"dropdown-menu\" aria-labelledby=\"navbarDropdown\">");
                }

                foreach (var page in pagesToList)
                {
                    string uri = linkProvider.GetUrl(relativePathToRootFolder, "Pages", page.Slug, outputFileExtension);
                    if (useDropdown)
                        sb.AppendChildLink(page.Title, uri);
                    else
                        sb.AppendRootLink(page.Title, uri);
                }

                if (useDropdown)
                {
                    sb.AppendLine("</div>");
                    sb.AppendLine("</li>");
                }
            }

            string syndicationLinkImage = $"<img align=\"absbottom\" id=\"rssIcon\" src=\"{syndicationImageUri}\" />";
            sb.AppendRootLink(syndicationLinkImage, syndicationUri);

            sb.AppendLine("</ul>");
            sb.AppendLine("</div>");
            sb.AppendLine("</nav>");

            return sb.ToString();
        }
    }
}
