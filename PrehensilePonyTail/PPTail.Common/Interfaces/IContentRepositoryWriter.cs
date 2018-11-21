using System;
using System.Collections.Generic;
using PPTail.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IContentRepositoryWriter
    {
        void SaveSiteSettings(SiteSettings settings);
        void SaveAllPages(IEnumerable<ContentItem> pages);
        void SaveAllPosts(IEnumerable<ContentItem> posts);
        void SaveAllWidgets(IEnumerable<Widget> widgets);
        void SaveFolderContents(string relativePath, IEnumerable<SourceFile> contents);
        void SaveCategories(IEnumerable<Category> categories);
    }
}
