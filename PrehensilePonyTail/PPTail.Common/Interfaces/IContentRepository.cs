using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IContentRepository
    {
        SiteSettings GetSiteSettings();
        IEnumerable<ContentItem> GetAllPages();
        IEnumerable<ContentItem> GetAllPosts();
        IEnumerable<Widget> GetAllWidgets();
        IEnumerable<SourceFile> GetFolderContents(string relativePath);
    }
}
