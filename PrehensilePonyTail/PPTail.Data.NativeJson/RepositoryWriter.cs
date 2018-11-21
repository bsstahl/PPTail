using System;
using System.Collections.Generic;
using System.Text;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Data.NativeJson
{
    public class RepositoryWriter: IContentRepositoryWriter
    {
        readonly string _filePath;
        public RepositoryWriter(string filePath)
        {
            _filePath = filePath;
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

        public void SaveFolderContents(string relativePath, IEnumerable<SourceFile> contents)
        {
            throw new NotImplementedException();
        }

        public void SaveSiteSettings(SiteSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
