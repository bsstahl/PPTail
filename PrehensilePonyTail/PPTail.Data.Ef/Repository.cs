using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Extensions;

namespace PPTail.Data.Ef
{
    public class Repository: Interfaces.IContentRepository
    {
        ContentContext _context;

        public Repository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetService<ContentContext>();
        }

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
