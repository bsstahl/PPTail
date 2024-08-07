﻿using System;
using System.Collections.Generic;
using System.Linq;
using PPTail.Entities;
using PPTail.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Extensions;

namespace PPTail.Data.WordpressFiles
{
    public class Repository : Interfaces.IContentRepository
    {
        const String _connectionStringFilepathKey = "FilePath";
        readonly String _dataFilePath;

        public Repository(String dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public Repository(IServiceProvider serviceProvider, String connectionString)
        {
            _dataFilePath = connectionString.GetConnectionStringValue(_connectionStringFilepathKey);
        }

        #region LazyLoad/Cache properties

        IEnumerable<Entities.ContentItem> _pages = null;
        private IEnumerable<Entities.ContentItem> Pages
        {
            get
            {
                if (_pages == null)
                    LoadPages();
                return _pages;
            }
        }

        IEnumerable<Entities.ContentItem> _posts = null;
        private IEnumerable<Entities.ContentItem> Posts
        {
            get
            {
                if (_posts == null)
                    LoadPosts();
                return _posts;
            }
        }

        IEnumerable<KeyValuePair<int, string>> _users;
        private IEnumerable<KeyValuePair<int, string>> Users
        {
            get
            {
                if (_users == null)
                    LoadUsers();
                return _users;
            }
        }

        IEnumerable<KeyValuePair<int, string>> _tags;
        private IEnumerable<KeyValuePair<int, string>> Tags
        {
            get
            {
                if (_tags == null)
                    LoadTags();
                return _tags;
            }
        }

        IEnumerable<KeyValuePair<int, Entities.Category>> _categories;
        private IEnumerable<KeyValuePair<int, Entities.Category>> Categories
        {
            get
            {
                if (_categories == null)
                    LoadCategories();
                return _categories;
            }
        }

        IEnumerable<Entities.Widget> _widgets;
        private IEnumerable<Entities.Widget> Widgets
        {
            get
            {
                if (_widgets == null)
                    LoadWidgets();
                return _widgets;
            }
        }

        IEnumerable<Entities.SourceFile> _folderContents = null;
        private IEnumerable<Entities.SourceFile> FolderContents
        {
            get
            {
                if (_folderContents == null)
                    LoadFolderContents();
                return _folderContents;
            }
        }

        Entities.SiteSettings _siteSettings = null;
        private Entities.SiteSettings SiteSettings
        {
            get
            {
                if (_siteSettings == null)
                    LoadSiteSettings();
                return _siteSettings;
            }
        }

        #endregion

        #region IContentRepository Implementation

        public IEnumerable<Entities.ContentItem> GetAllPages()
        {
            return this.Pages;
        }

        public IEnumerable<Entities.ContentItem> GetAllPosts()
        {
            return this.Posts;
        }

        public IEnumerable<Entities.Widget> GetAllWidgets()
        {
            return this.Widgets;
        }

        public IEnumerable<Entities.Category> GetCategories()
        {
            return this.Categories.Select(c => c.Value);
        }

        public IEnumerable<SourceFile> GetFolderContents(String relativePath)
        {
            return GetFolderContents(relativePath, false);
        }

        public IEnumerable<Entities.SourceFile> GetFolderContents(String relativePath, bool recursive)
        {
            return recursive 
                ? throw new NotImplementedException() 
                : this.FolderContents;
        }

        public Entities.SiteSettings GetSiteSettings()
        {
            return this.SiteSettings;
        }

        #endregion

        #region Data load methods

        private void LoadPages()
        {
            var result = new List<ContentItem>();

            var fullPath = System.IO.Path.Combine(_dataFilePath, "pages.json");
            if (System.IO.File.Exists(fullPath))
            {

                var pagesJson = System.IO.File.ReadAllText(fullPath);
                var pages = Newtonsoft.Json.JsonConvert.DeserializeObject<Page[]>(pagesJson);

                foreach (var page in pages)
                {
                    String authorName = this.Users.Single(a => a.Key == page.author).Value;
                    result.Add(new ContentItem()
                    {
                        Author = authorName,
                        ByLine = $"by {authorName}",
                        CategoryIds = new List<System.Guid>(),
                        Content = page.content.rendered,
                        Description = page.excerpt.rendered,
                        Id = System.Guid.NewGuid(),
                        IsPublished = (page.status.ToLower() == "publish"),
                        PublicationDate = page.date,
                        LastModificationDate = page.modified,
                        MenuOrder = page.menu_order,
                        ShowInList = (page.menu_order > 0),
                        Tags = new List<string>(),
                        Title = page.title.rendered,
                        Slug = page.slug
                    });
                }
            }

            _pages = result;
        }

        private void LoadPosts()
        {
            var result = new List<ContentItem>();

            var fullPath = System.IO.Path.Combine(_dataFilePath, "posts.json");

            if (System.IO.File.Exists(fullPath))
            {
                var postsJson = System.IO.File.ReadAllText(fullPath);
                var posts = Newtonsoft.Json.JsonConvert.DeserializeObject<Post[]>(postsJson);

                foreach (var post in posts)
                {
                    String authorName = this.Users.Single(a => a.Key == post.author).Value;
                    result.Add(new ContentItem()
                    {
                        Author = authorName,
                        ByLine = $"by {authorName}",
                        CategoryIds = this.Categories.Where(c => post.categories.Contains(c.Key)).Select(c => c.Value.Id),
                        Content = post.content.rendered,
                        Description = post.excerpt.rendered,
                        Id = System.Guid.NewGuid(),
                        IsPublished = (post.status.ToLower() == "publish"),
                        PublicationDate = post.date,
                        LastModificationDate = post.modified,
                        MenuOrder = 0,
                        ShowInList = false,
                        Tags = this.Tags.Where(t => post.tags.Contains(t.Key)).Select(t => t.Value),
                        Title = post.title.rendered,
                        Slug = post.slug
                    });
                }
            }

            _posts = result;
        }

        private void LoadCategories()
        {
            var result = new List<KeyValuePair<int, Entities.Category>>();

            var fullPath = System.IO.Path.Combine(_dataFilePath, "categories.json");

            if (System.IO.File.Exists(fullPath))
            {
                var categoriesJson = System.IO.File.ReadAllText(fullPath);
                var categories = Newtonsoft.Json.JsonConvert.DeserializeObject<Category[]>(categoriesJson);

                foreach (var category in categories)
                {
                    result.Add(new KeyValuePair<int, Entities.Category>(
                        category.id,
                        new Entities.Category()
                        {
                            Description = category.description,
                            Id = System.Guid.NewGuid(),
                            Name = category.name
                        }));
                }
            }

            _categories = result;
        }

        private void LoadWidgets()
        {
            // TODO: Implement if ever needed. The current data from WP that I have doesn't have any widgets.
            _widgets = new List<Widget>() {
                new Widget()
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Sample Widget",
                    ShowTitle = true,
                    WidgetType = Enumerations.WidgetType.TextBox,
                    Dictionary = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("Content", "Place content here")
                    }
                }};
        }

        private void LoadFolderContents()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        private void LoadSiteSettings()
        {
            // TODO: Implement if ever needed. The current data from WP that I have doesn't have any visible way to get this information.
            _siteSettings = new Entities.SiteSettings()
            {
                Title = "Site Title",
                Description = "A Description of the Site",
                PostsPerFeed = 10,
                PostsPerPage = 10,
                Theme = "ThemeName"
            };
        }

        private void LoadTags()
        {
            var result = new List<KeyValuePair<int, string>>();

            var fullPath = System.IO.Path.Combine(_dataFilePath, "tags.json");
            var tagsJson = System.IO.File.ReadAllText(fullPath);
            var tags = Newtonsoft.Json.JsonConvert.DeserializeObject<Tag[]>(tagsJson);

            foreach (var tag in tags)
                result.Add(new KeyValuePair<int, string>(tag.id, tag.name));

            _tags = result;
        }

        private void LoadUsers()
        {
            // TODO: Implement for real
            _users = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(11, "User11"),
                new KeyValuePair<int, string>(21, "User21"),
                new KeyValuePair<int, string>(31, "User31")
            };
        }

        public void AddPage(ContentItem item) => throw new NotImplementedException();
        public void AddPages(IEnumerable<ContentItem> items) => throw new NotImplementedException();

        #endregion

    }
}
