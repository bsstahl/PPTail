using System;
using System.Collections.Generic;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Data.WordpressFiles
{
    public class Repository : Interfaces.IContentRepository
    {
        readonly string _dataFilePath;

        public Repository(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
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
