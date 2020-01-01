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

        public static IContentRepository GetContentRepository(this IServiceProvider serviceProvider, String connectionString)
        {
            String instanceName = connectionString.GetConnectionStringValue("Provider");
            return serviceProvider.GetNamedService<IContentRepository>(instanceName);
        }
    }
}
