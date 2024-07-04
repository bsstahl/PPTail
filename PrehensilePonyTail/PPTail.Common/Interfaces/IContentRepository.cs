using System;
using System.Collections.Generic;
using PPTail.Entities;

namespace PPTail.Interfaces
{
    public interface IContentRepository
    {
        SiteSettings GetSiteSettings();

        void AddPage(ContentItem item);
        void AddPages(IEnumerable<ContentItem> items);

        IEnumerable<ContentItem> GetAllPages();
        IEnumerable<ContentItem> GetAllPosts();
        
        IEnumerable<Widget> GetAllWidgets();

        IEnumerable<SourceFile> GetFolderContents(String relativePath);
        IEnumerable<SourceFile> GetFolderContents(String relativePath, bool recursive);

        IEnumerable<Category> GetCategories();
    }
}
