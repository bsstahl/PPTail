﻿using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;

namespace PPTail.Data.Forestry
{
    public class RepositoryWriter : IContentRepositoryWriter
    {
        const string PAGES_RELATIVE_PATH = "Pages";
        const string POSTS_RELATIVE_PATH = "Posts";

        private readonly IServiceProvider _serviceProvider;

        private readonly string _rootFilePath;
        private readonly string _contentRepositoryName;

        public RepositoryWriter(IServiceProvider serviceProvider, string rootFilePath, string contentRepositoryName)
        {
            _serviceProvider = serviceProvider;

            _rootFilePath = rootFilePath;
            _contentRepositoryName = contentRepositoryName;
        }

        private IEnumerable<Category> _categories;
        public IEnumerable<Category> Categories 
        { 
            get
            {
                if (_categories is null)
                {
                    var readRepo = _serviceProvider.GetNamedService<IContentRepository>(_contentRepositoryName);
                    _categories = readRepo.GetCategories();
                }
                return _categories;
            }
        }

        public void SaveAllPages(IEnumerable<Entities.ContentItem> pages)
        {
            string path = System.IO.Path.Combine(_rootFilePath, PAGES_RELATIVE_PATH);
            SaveContentItems(pages, path);
        }

        public void SaveAllPosts(IEnumerable<Entities.ContentItem> posts)
        {
            string path = System.IO.Path.Combine(_rootFilePath, POSTS_RELATIVE_PATH);
            SaveContentItems(posts, path);
        }


        public void SaveAllWidgets(IEnumerable<Entities.Widget> widgets) => throw new NotImplementedException();
        public void SaveCategories(IEnumerable<Category> categories) => throw new NotImplementedException();
        public void SaveFolderContents(String relativePath, IEnumerable<SourceFile> contents) => throw new NotImplementedException();
        public void SaveSiteSettings(Entities.SiteSettings settings) => throw new NotImplementedException();



        private void SaveContentItems(IEnumerable<Entities.ContentItem> contentItems, String relativePath)
        {
            string fullPath = System.IO.Path.GetFullPath(relativePath);
            foreach (var item in contentItems)
            {
                item.Slug = String.IsNullOrWhiteSpace(item.Slug) ? item.Title.CreateSlug() : item.Slug;

                var fileContents = new ContentItemFileBuilder(item, this.Categories)
                    .Build();

                var filePath = System.IO.Path.Combine(fullPath, $"{item.Slug}.md");
                System.IO.File.WriteAllText(filePath, fileContents);
            }
        }

    }
}
