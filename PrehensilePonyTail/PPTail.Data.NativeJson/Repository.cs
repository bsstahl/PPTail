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
        const string _connectionStringFilepathKey = "FilePath";

        readonly string _filePath;
        public Repository(string filePath)
        {
            _filePath = filePath;
        }

        public Repository(IServiceProvider serviceProvider)
        {
            serviceProvider.ValidateService<ISettings>();

            var settings = serviceProvider.GetService<ISettings>();
            settings.Validate(s => s.SourceConnection, nameof(settings.SourceConnection));

            _filePath = settings.SourceConnection.GetConnectionStringValue(_connectionStringFilepathKey);
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetCategories()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SourceFile> GetFolderContents(string relativePath)
        {
            throw new NotImplementedException();
        }

        public SiteSettings GetSiteSettings()
        {
            throw new NotImplementedException();
        }
    }
}
