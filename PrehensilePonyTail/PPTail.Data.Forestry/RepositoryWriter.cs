using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.Forestry
{
    public class RepositoryWriter : IContentRepositoryWriter
    {
        public void SaveAllPages(IEnumerable<ContentItem> pages)
        {
            throw new NotImplementedException();
        }

        public void SaveAllPosts(IEnumerable<ContentItem> posts)
        {
            throw new NotImplementedException();
        }


        public void SaveAllWidgets(IEnumerable<Widget> widgets) => throw new NotImplementedException();
        public void SaveCategories(IEnumerable<Category> categories) => throw new NotImplementedException();
        public void SaveFolderContents(String relativePath, IEnumerable<SourceFile> contents) => throw new NotImplementedException();
        public void SaveSiteSettings(SiteSettings settings) => throw new NotImplementedException();
    }
}
