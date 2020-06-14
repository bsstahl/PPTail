using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Extensions;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Links
{
    public class LinkProvider: Interfaces.ILinkProvider
    {
        readonly IServiceProvider _serviceProvider;

        public LinkProvider(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        public String GetUrl(String pathToRoot, String relativePath, String fileName, String fileExtension)
        {
            // HACK: Use native HTML pathing
            string fileNameWithExtension = $"{fileName}.{fileExtension}";
            string fullPath = System.IO.Path.Combine(pathToRoot, relativePath, fileNameWithExtension).ToHttpSlashes().RemoveLeadingDotSlash();
            return fullPath;
        }

        public String GetUrl(String pathToRoot, String relativePath, String fileName)
        {
            _serviceProvider.ValidateService<IContentRepository>();
            var contentRepo = _serviceProvider.GetService<IContentRepository>();
            var siteSettings = contentRepo.GetSiteSettings();
            return this.GetUrl(pathToRoot, relativePath, fileName, siteSettings.OutputFileExtension);
        }

    }
}
