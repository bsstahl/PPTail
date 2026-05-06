using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.NotFound
{
    public class NotFoundProvider : INotFoundProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public NotFoundProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _serviceProvider.ValidateService<IContentRepository>();
        }

        public String Generate404Page()
        {
            var siteSettings = _serviceProvider.GetSiteSettings() ?? new SiteSettings();

            var title = siteSettings.Title ?? "This Site";
            var description = siteSettings.Description ?? string.Empty;
            var copyright = siteSettings.Copyright ?? string.Empty;

            var copyrightSection = string.IsNullOrWhiteSpace(copyright)
                ? string.Empty
                : $"\r\n    <footer>\r\n      <p>{System.Net.WebUtility.HtmlEncode(copyright)}</p>\r\n    </footer>";

            return $@"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Page Not Found - {System.Net.WebUtility.HtmlEncode(title)}</title>
  </head>
  <body>
    <h1>Page Not Found</h1>
    <p>The page you are looking for could not be found on <a href=""/"">{System.Net.WebUtility.HtmlEncode(title)}</a>.</p>
    <p>{System.Net.WebUtility.HtmlEncode(description)}</p>
    <p><a href=""/"">Return to Home Page</a></p>{copyrightSection}
  </body>
</html>
";
        }
    }
}
