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
            var fileSystem = _serviceProvider.GetService<IFileSystem>();

            var results = new List<ContentItem>();
            // string pagePath = System.IO.Path.Combine(_rootPath, "pages");
            var files = fileSystem.EnumerateFiles(""); // pagePath
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".xml")))
            {
                var contentItem = fileSystem.ReadAllText(file).ParseContentItem("page");
                if (contentItem != null)
                    results.Add(contentItem);
            }
            return results;
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            throw new NotImplementedException();
        }
    }
}
