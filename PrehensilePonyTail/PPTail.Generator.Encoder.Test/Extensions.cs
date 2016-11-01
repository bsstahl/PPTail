using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.Encoder.Test
{
    public static class Extensions
    {
        public static IServiceCollection Create(this IServiceCollection ignore)
        {
            var container = new ServiceCollection();

            return container;
        }
    }
}
