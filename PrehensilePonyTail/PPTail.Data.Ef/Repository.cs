using System;
using System.Collections.Generic;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Data.Ef
{
    public class Repository : Interfaces.IContentRepository
    {
        ContentContext _context;

        public Repository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetService<ContentContext>();
        }

        public void AddPage(Entities.ContentItem item) => throw new NotImplementedException();
        public void AddPages(IEnumerable<Entities.ContentItem> items) => throw new NotImplementedException();

        public IEnumerable<Entities.ContentItem> GetAllPages()
        {
            return new ContentItemCollection(_context.Pages);
        }

        public IEnumerable<Entities.ContentItem> GetAllPosts()
        {
            return new ContentItemCollection(_context.Posts);
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetCategories()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SourceFile> GetFolderContents(String relativePath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SourceFile> GetFolderContents(String relativePath, bool recursive)
        {
            throw new NotImplementedException();
        }

        public SiteSettings GetSiteSettings()
        {
            throw new NotImplementedException();
        }
    }
}
