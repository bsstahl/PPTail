using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;

namespace PPTail.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void ValidateService<T>(this IServiceProvider serviceProvider)
        {
            T service = serviceProvider.GetService<T>();
            if (service == null)
                throw new Exceptions.DependencyNotFoundException(typeof(T).Name);
        }

        public static T GetNamedService<T>(this IServiceProvider serviceProvider, String instanceName) where T: class
        {
            var services = serviceProvider.GetServices<T>();
            var service = services.SingleOrDefault(s => s.GetType().FullName.ToLower() == instanceName.ToLower());
            if (service is null)
                throw new Exceptions.DependencyNotFoundException(typeof(T).Name, instanceName);
            return service;
        }

        public static IContentRepository GetContentRepository(this IServiceProvider serviceProvider)
        {
            // At one time there were multiple IContentRepositories in the container
            // That no longer happens but that is why this method exists
            // It can be removed if everywhere that calls it is restored
            // to the line below.
            return serviceProvider.GetService<IContentRepository>();
        }

        public static IEnumerable<Entities.Template> GetTemplates(this IServiceProvider serviceProvider)
        {
            var templateRepo = serviceProvider.GetService<ITemplateRepository>();
            return templateRepo.GetAllTemplates();
        }
    }
}
