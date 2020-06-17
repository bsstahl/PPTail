using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog
{
    public class RepositoryWriter : IContentRepositoryWriter
    {
        const string _connectionStringFilePathKey = "FilePath";
        const string _connectionStringUseYamlKey = "UseYaml";

        private readonly IServiceProvider _serviceProvider;

        private readonly string _rootFilePath;
        private readonly bool _useYaml;

        public RepositoryWriter(IServiceProvider serviceProvider, string connectionString)
        {
            _serviceProvider = serviceProvider;
            _rootFilePath = connectionString.GetConnectionStringValue(_connectionStringFilePathKey);
            _useYaml = connectionString.GetConnectionStringValue(_connectionStringUseYamlKey).AsBoolean();
        }

        public void SaveAllPages(IEnumerable<ContentItem> pages)
        {
            throw new NotImplementedException();
        }

        public void SaveAllPosts(IEnumerable<ContentItem> posts)
        {
            throw new NotImplementedException();
        }

        public void SaveAllWidgets(IEnumerable<Widget> widgets)
        {
            throw new NotImplementedException();
        }

        public void SaveCategories(IEnumerable<Category> categories)
        {
            throw new NotImplementedException();
        }

        public void SaveSiteSettings(SiteSettings settings)
        {
            throw new NotImplementedException();
        }

        public void SaveFolderContents(String relativePath, IEnumerable<SourceFile> contents) => throw new NotImplementedException();
    }
}
