using System;
using System.Collections.Generic;
using PPTail.Entities;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;

namespace PPTail.Data.NativeJson
{
    public class Repository : IContentRepository
    {
        const String _connectionStringFilepathKey = "FilePath";

        readonly String _filePath;
        public Repository(String filePath)
        {
            _filePath = filePath;
        }

        private Context _context;
        private Context Context
        {
            get
            {
                if (_context == null)
                    Load();
                return _context;
            }
        }

        public Repository(IServiceProvider serviceProvider, String connectionString)
        {
            _filePath = connectionString.GetConnectionStringValue(_connectionStringFilepathKey);
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            return this.Context.Pages;
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            return this.Context.Posts;
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            return this.Context.Widgets;
        }

        public IEnumerable<Category> GetCategories()
        {
            return this.Context.Categories;
        }

        public IEnumerable<SourceFile> GetFolderContents(String relativePath)
        {
            // TODO: Implement
            return new List<SourceFile>();
        }

        public SiteSettings GetSiteSettings()
        {
            return this.Context.SiteSettings;
        }

        private void Load()
        {
            var jsonData = System.IO.File.ReadAllText(_filePath);
            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Context>(jsonData);
            _context = context;
        }
    }
}
