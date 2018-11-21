using System;
using System.Collections.Generic;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Data.NativeJson
{
    public class Repository : IContentRepository
    {
        readonly string _filePath;
        public Repository(string filePath)
        {
            _filePath = filePath;
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
