using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;

namespace PPTConvert
{
    public static class ServiceProviderExtensions
    {
        const string _connectionStringProviderKey = "Provider";

        public static T GetConfiguredService<T>(this IServiceProvider serviceProvider, string connectionString) where T: class
        {
            string providerName = connectionString.GetConnectionStringValue(_connectionStringProviderKey);
            var providers = serviceProvider.GetServices<T>();
            var provider = providers.SingleOrDefault(p => p.GetType().FullName == providerName);
            if (provider == default)
                throw new InvalidOperationException($"Unable to load provider '{providerName}'");
            return provider;
        }
    }
}
