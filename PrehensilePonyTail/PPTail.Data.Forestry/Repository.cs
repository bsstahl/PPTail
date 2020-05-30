using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using PPTail.Interfaces;
using PPTail.Extensions;

namespace PPTail.Data.Forestry
{
    public class Repository : Interfaces.IContentRepository
    {
        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 5;

        const String _connectionStringFilepathKey = "FilePath";

        const string _dataRelativePath = "Data";
        const string _pagesRelativePath = "Pages";
        const string _postsRelativePath = "Posts";
        const string _widgetsRelativePath = "Widgets";

        const String _settingsFilename = "SiteSettings.md";
        const String _categoriesFilename = "Categories.md";

        private readonly IServiceProvider _serviceProvider;
        private readonly string _rootDataPath;
        private readonly string _rootPagesPath;
        private readonly string _rootPostsPath;
        private readonly String _rootSitePath;
        private readonly string _rootWidgetsPath;

        private SiteSettings _siteSettings = null;
        private IEnumerable<Category> _categories = null;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<IFile>();

            var settings = _serviceProvider.GetService<ISettings>();
            settings.Validate(s => s.SourceConnection, nameof(settings.SourceConnection));

            _rootSitePath = settings.SourceConnection.GetConnectionStringValue(_connectionStringFilepathKey);
            _rootDataPath = System.IO.Path.Combine(_rootSitePath, _dataRelativePath);
            _rootPagesPath = System.IO.Path.Combine(_rootSitePath, _pagesRelativePath);
            _rootPostsPath = System.IO.Path.Combine(_rootSitePath, _postsRelativePath);
            _rootWidgetsPath = System.IO.Path.Combine(_rootSitePath, _widgetsRelativePath);
        }

        public SiteSettings GetSiteSettings()
        {
            if (_siteSettings == null)
            {
                var fileSystem = _serviceProvider.GetService<IFile>();
                String settingsPath = System.IO.Path.Combine(_rootDataPath, _settingsFilename);
                _siteSettings = fileSystem.ReadAllText(settingsPath).ParseSettings();
                if (_siteSettings == null)
                    throw new Exceptions.SettingNotFoundException(typeof(SiteSettings).Name);

                if (string.IsNullOrWhiteSpace(_siteSettings.Title))
                    throw new Exceptions.SettingNotFoundException(nameof(_siteSettings.Title));

                if (_siteSettings.PostsPerPage == 0)
                    _siteSettings.PostsPerPage = _defaultPostsPerPage;

                if (_siteSettings.PostsPerFeed == 0)
                    _siteSettings.PostsPerFeed = _defaultPostsPerFeed;
            }

            return _siteSettings;
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            return this.GetContentItems(_rootPagesPath);
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            return this.GetContentItems(_rootPostsPath);
        }

        private IEnumerable<ContentItem> GetContentItems(String path)
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            var files = directory.EnumerateFiles(path);
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".md")))
            {
                var contentText = fileSystem.ReadAllText(file);
                ContentItem contentItem = null;
                try
                {
                    contentItem = contentText.ParseContentItem(this.GetCategories());
                }
                catch (Exception ex)
                {
                    string message = $"Unable to parse a ContentItem from '{file}'\r\n\r\n{contentText}";
                    throw new InvalidOperationException(message, ex);
                }

                if (contentItem.IsNotNull())
                    results.Add(contentItem);
            }

            return results;
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            List<Widget> results = new List<Widget>();
            var files = directory.EnumerateFiles(_rootWidgetsPath);
            foreach (var file in files)
            {
                var widget = fileSystem
                    .ReadAllText(file)
                    .ParseWidget();

                if (widget.IsNotNull())
                    results.Add(widget);
            }

            return results;
        }

        public IEnumerable<SourceFile> GetFolderContents(String relativePath)
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var folderPath = System.IO.Path.Combine(_rootSitePath, relativePath);
            var results = new List<SourceFile>();

            if (directory.Exists(folderPath))
            {
                var sourceFiles = directory.EnumerateFiles(folderPath);
                foreach (var sourceFile in sourceFiles)
                {
                    byte[] contents = null;
                    try
                    {
                        contents = fileSystem.ReadAllBytes(sourceFile);
                    }
                    catch (System.UnauthorizedAccessException)
                    { }

                    if (contents != null)
                        results.Add(new SourceFile()
                        {
                            Contents = contents,
                            FileName = System.IO.Path.GetFileName(sourceFile),
                            RelativePath = relativePath
                        });
                }
            }

            return results;
        }

        public IEnumerable<Category> GetCategories()
        {
            if (_categories is null)
            {
                var fileSystem = _serviceProvider.GetService<IFile>();

                string path = System.IO.Path.Combine(_rootDataPath, _categoriesFilename);
                _categories = fileSystem
                    .ReadAllText(path)
                    .ParseCategories()
                    .Where(c => c.Id != Guid.Empty && !String.IsNullOrWhiteSpace(c.Name));
            }

            return _categories;
        }
    }
}
