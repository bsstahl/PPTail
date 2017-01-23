using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Data.Ef.Test
{
    public static class Extensions
    {
        public static IServiceCollection AddInMemoryContext(this IServiceCollection container)
        {
            return container.AddInMemoryContext(string.Empty.GetRandom());
        }

        public static IServiceCollection AddInMemoryContext(this IServiceCollection container, string dbName)
        {
            return container.AddDbContext<ContentContext>(p => p.UseInMemoryDatabase(databaseName: string.Empty.GetRandom()), ServiceLifetime.Transient);
        }

        public static ContentItem Create(this ContentItem ignore)
        {
            return new ContentItem() { Id = Guid.NewGuid(), Title = string.Empty.GetRandom() };
        }
    }
}
