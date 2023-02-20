using PPTail.Entities;
using PPTail.Interfaces;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Markdig;
using Markdig.SyntaxHighlighting;

namespace PPTail.Data.MediaBlog
{
    public class YamlRepository : IContentRepository
    {
        const String _connectionStringFilepathKey = "FilePath";

        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 5;

        private readonly String _rootPath;
        private readonly IServiceProvider _serviceProvider;

        private IEnumerable<Category> _categories = null;
        private SiteSettings _siteSettings = null;

        private MarkdownPipeline _markdownPipeline = null;

        public MarkdownPipeline MarkdownPipeline
        {
            get
            {
                if (_markdownPipeline is null)
                {
                    _markdownPipeline = new MarkdownPipelineBuilder()
                        .UseSyntaxHighlighting()
                        .Build();
                }

                return _markdownPipeline;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be fixed in Globalization project")]
        public YamlRepository(IServiceProvider serviceProvider, string connectionString)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<IFile>();
            _serviceProvider.ValidateService<IDirectory>();

            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _rootPath = connectionString.GetConnectionStringValue(_connectionStringFilepathKey);

            if (String.IsNullOrWhiteSpace(_rootPath))
                throw new ArgumentException($"{_connectionStringFilepathKey} not found in Connection String", nameof(connectionString));
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            var relativePagePath = System.IO.Path.Combine(_rootPath, "Pages");
            var pagePath = System.IO.Path.GetFullPath(relativePagePath);
            var files = directory.EnumerateFiles(pagePath);

            var categories = this.GetCategories();

            foreach (var file in files.Where(f => f.ToUpperInvariant().EndsWith(".MD", StringComparison.InvariantCulture)))
            {
                var contents = fileSystem.ReadAllText(file);
                results.Add((null as ContentItem).FromYaml(contents, categories, this.MarkdownPipeline));
            }
            return results;
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            var postPath = System.IO.Path.Combine(_rootPath, "Posts");

            if (directory.Exists(postPath))
            {
                var files = directory.EnumerateFiles(postPath);
                foreach (var file in files.Where(f => f.ToUpperInvariant().EndsWith(".MD", StringComparison.InvariantCulture)))
                {
                    var fileContents = fileSystem.ReadAllText(file);
                    var (mediaPost, id) = YamlMediaPost.CreateMediaPost(fileContents, this.MarkdownPipeline);
                    var contentItem = mediaPost.AsContentItem(id);
                    if (contentItem != null)
                    {
                        results.Add(contentItem);
                    }
                }
            }

            return results;
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            string widgetPath = System.IO.Path.Combine(_rootPath, "Widgets");

            List<Entities.Widget> results = new List<Entities.Widget>();
            var files = directory.EnumerateFiles(widgetPath);
            foreach (var file in files)
            {
                var widget = fileSystem
                    .ReadAllText(file)
                    .ParseWidgetYaml(this.MarkdownPipeline);

                if (widget.IsNotNull() && widget.ShowInSidebar)
                    results.Add(widget);
            }

            return results;
        }

        public IEnumerable<Category> GetCategories()
        {
            if (_categories is null)
            {
                var categoriesFilePath = System.IO.Path.Combine(_rootPath, "Data\\Categories.md");
                var fileSystem = _serviceProvider.GetService<IFile>();
                var fileContent = fileSystem.ReadAllText(categoriesFilePath);

                var deserializer = new DeserializerBuilder().
                    WithNamingConvention(LowerCaseNamingConvention.Instance)
                    .Build();

                var (frontMatter, content) = fileContent.SplitYamlFile();
                _categories = deserializer.Deserialize<YamlCategoryCollection>(frontMatter).AsEntity();
            }

            return _categories;
        }

        public IEnumerable<SourceFile> GetFolderContents(String relativePath)
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var folderPath = System.IO.Path.Combine(_rootPath, relativePath);
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

        public SiteSettings GetSiteSettings()
        {
            if (_siteSettings == null)
            {
                var fileSystem = _serviceProvider.GetService<IFile>();
                String settingsPath = System.IO.Path.Combine(_rootPath, "Data\\SiteSettings.md");
                _siteSettings = fileSystem.ReadAllText(settingsPath).ParseYamlSettings();
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
    }
}
