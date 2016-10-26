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
        IServiceProvider _serviceProvider;

        public LinkProvider(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<ISettings>();
        }

        public string GetUrl(string pathToRoot, string relativePath, string fileName, string fileExtension)
        {
            return System.IO.Path.Combine(pathToRoot, relativePath, $"{fileName}.{fileExtension}").ToHttpSlashes().RemoveLeadingDotSlash();
        }

        public string GetUrl(string pathToRoot, string relativePath, string fileName)
        {
            var settings = _serviceProvider.GetService<ISettings>();
            return GetUrl(pathToRoot, relativePath, fileName, settings.OutputFileExtension);
        }

    }
}
