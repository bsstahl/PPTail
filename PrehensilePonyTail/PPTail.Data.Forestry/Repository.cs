using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using PPTail.Interfaces;
using PPTail.Extensions;
using Markdig;
using Markdig.SyntaxHighlighting;

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

        private Entities.SiteSettings _siteSettings = null;
        private IEnumerable<Category> _categories = null;

        private MarkdownPipeline _markdownPipeline = null;

        public MarkdownPipeline MarkdownPipeline 
        { 
            get
            {
                if (_markdownPipeline is null)
                {
                    _markdownPipeline = new MarkdownPipelineBuilder()
                        // .UseAdvancedExtensions()
                        .UseSyntaxHighlighting()
                        .Build();
                }
                return _markdownPipeline;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be fixed in Globalization project")]
        public Repository(IServiceProvider serviceProvider, string sourceConnection)
        {
            _serviceProvider = serviceProvider;
            _serviceProvider.ValidateService<IFile>();

            if (String.IsNullOrWhiteSpace(sourceConnection))
                throw new ArgumentNullException(nameof(sourceConnection));

            _rootSitePath = sourceConnection.GetConnectionStringValue(_connectionStringFilepathKey);
            if (String.IsNullOrWhiteSpace(_rootSitePath))
                throw new ArgumentException($"{_connectionStringFilepathKey} not found in Connection String", nameof(sourceConnection));

            _rootDataPath = System.IO.Path.Combine(_rootSitePath, _dataRelativePath);
            _rootPagesPath = System.IO.Path.Combine(_rootSitePath, _pagesRelativePath);
            _rootPostsPath = System.IO.Path.Combine(_rootSitePath, _postsRelativePath);
            _rootWidgetsPath = System.IO.Path.Combine(_rootSitePath, _widgetsRelativePath);
        }

        public Entities.SiteSettings GetSiteSettings()
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

        public IEnumerable<Entities.ContentItem> GetAllPages()
        {
            return this.GetContentItems(_rootPagesPath);
        }

        public IEnumerable<Entities.ContentItem> GetAllPosts()
        {
            return this.GetContentItems(_rootPostsPath);
        }

        private IEnumerable<Entities.ContentItem> GetContentItems(String path)
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<Entities.ContentItem>();
            var files = directory.EnumerateFiles(path);
            foreach (var file in files.Where(f => f.ToUpperInvariant().EndsWith(".MD", StringComparison.InvariantCulture)))
            {
                var contentText = fileSystem.ReadAllText(file);
                var categories = this.GetCategories();
                Entities.ContentItem contentItem = null;
                try
                {
                    contentItem = contentText.ParseContentItem(categories, this.MarkdownPipeline);
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

        public IEnumerable<Entities.Widget> GetAllWidgets()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            List<Entities.Widget> results = new List<Entities.Widget>();
            var files = directory.EnumerateFiles(_rootWidgetsPath);
            foreach (var file in files)
            {
                var widget = fileSystem
                    .ReadAllText(file)
                    .ParseWidget(this.MarkdownPipeline);

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
