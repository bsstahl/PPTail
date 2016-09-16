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
        const string _sourceDataPathSettingName = "sourceDataPath";

        private readonly IServiceProvider _serviceProvider;
        private readonly string _rootPath;

        public Repository(IServiceCollection container)
        {
            _serviceProvider =  container.BuildServiceProvider();

            var settings = _serviceProvider.GetService<Settings>();
            if (settings == null)
                throw new Exceptions.DependencyNotFoundException(nameof(Settings));

            var fileSystem = _serviceProvider.GetService<IFileSystem>();
            if (fileSystem == null)
                throw new Exceptions.DependencyNotFoundException(nameof(IFileSystem));

            if (!settings.ExtendedSettings.HasSetting(_sourceDataPathSettingName))
                throw new Exceptions.SettingNotFoundException(_sourceDataPathSettingName);

            _rootPath = settings.ExtendedSettings.Get(_sourceDataPathSettingName);
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            var fileSystem = _serviceProvider.GetService<IFileSystem>();

            var results = new List<ContentItem>();
            string pagePath = System.IO.Path.Combine(_rootPath, "pages");
            var files = fileSystem.EnumerateFiles(pagePath);
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
            var fileSystem = _serviceProvider.GetService<IFileSystem>();

            var results = new List<ContentItem>();
            string pagePath = System.IO.Path.Combine(_rootPath, "posts");
            var files = fileSystem.EnumerateFiles(pagePath);
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".xml")))
            {
                var contentItem = fileSystem.ReadAllText(file).ParseContentItem("post");
                if (contentItem != null)
                    results.Add(contentItem);
            }
            return results;
        }
    }
}
