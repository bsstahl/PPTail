using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;

namespace PPTail.Data.FileSystem
{
    public class Repository: Interfaces.IContentRepository
    {
        private readonly string _rootPath;

        public Repository(string rootPath)
        {
            _rootPath = rootPath;
        }

        public SiteSettings GetSiteSettings()
        {
            string settingsPath = System.IO.Path.Combine(_rootPath, "settings.xml");
            var result = System.IO.File.ReadAllText(settingsPath).ParseSettings();
            return result;
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            var results = new List<ContentItem>();
            string pagePath = System.IO.Path.Combine(_rootPath, "pages");
            var files = System.IO.Directory.EnumerateFiles(pagePath);
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".xml")))
            {
                var contentItem = System.IO.File.ReadAllText(file).ParseContentItem("page");
                if (contentItem != null)
                    results.Add(contentItem);
            }
            return results;
        }

        //TODO: Unit test
        public IEnumerable<ContentItem> GetAllPosts()
        {
            var results = new List<ContentItem>();
            string pagePath = System.IO.Path.Combine(_rootPath, "posts");
            var files = System.IO.Directory.EnumerateFiles(pagePath);
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".xml")))
            {
                var contentItem = System.IO.File.ReadAllText(file).ParseContentItem("post");
                if (contentItem != null)
                    results.Add(contentItem);
            }
            return results;
        }
    }
}
