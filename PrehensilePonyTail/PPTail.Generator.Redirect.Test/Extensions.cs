using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.Redirect.Test
{
    public static class Extensions
    {
        public static IRedirectProvider Create(this IRedirectProvider ignore, String redirectTemplateText)
        {
            var redirectTemplate = new Template()
            {
                Content = redirectTemplateText,
                TemplateType = Enumerations.TemplateType.Redirect
            };

            return ignore.Create(redirectTemplate);
        }

        private static IRedirectProvider Create(this IRedirectProvider ignore, Template redirectTemplate)
        {
            var templates = new List<Template>() { redirectTemplate };
            return ignore.Create(templates);
        }

        private static IRedirectProvider Create(this IRedirectProvider ignore, List<Template> templates)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IEnumerable<Template>>(templates);
            return ignore.Create(serviceCollection);
        }

        private static IRedirectProvider Create(this IRedirectProvider ignore, IServiceCollection serviceCollection)
        {
            return new RedirectProvider(serviceCollection.BuildServiceProvider());
        }
    }
}
