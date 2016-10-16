using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
