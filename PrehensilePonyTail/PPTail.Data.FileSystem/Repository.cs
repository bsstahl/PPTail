using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Data.FileSystem
{
    public class Repository: Interfaces.IContentRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            throw new NotImplementedException();
        }
    }
}
