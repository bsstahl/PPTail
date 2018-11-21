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
        Context _context = null;

        public RepositoryWriter(string filePath)
        {
            _filePath = filePath;
        }

        private Context Context
        {
            get
            {
                if (_context == null)
                    _context = Context.Load(_filePath);
                return _context;
            }
        }

        public void SaveAllPages(IEnumerable<ContentItem> pages)
        {
            var context = this.Context;
            context.Pages = pages;
            context.Save(_filePath);
        }

        public void SaveAllPosts(IEnumerable<ContentItem> posts)
        {
            var context = this.Context;
            context.Posts = posts;
            context.Save(_filePath);
        }

        public void SaveAllWidgets(IEnumerable<Widget> widgets)
        {
            var context = this.Context;
            context.Widgets = widgets;
            context.Save(_filePath);
        }

        public void SaveCategories(IEnumerable<Category> categories)
        {
            var context = this.Context;
            context.Categories = categories;
            context.Save(_filePath);
        }

        public void SaveFolderContents(string relativePath, IEnumerable<SourceFile> contents)
        {
            throw new NotImplementedException();
        }

        public void SaveSiteSettings(SiteSettings settings)
        {
            var context = this.Context;
            context.SiteSettings = settings;
            context.Save(_filePath);
        }
    }
}
