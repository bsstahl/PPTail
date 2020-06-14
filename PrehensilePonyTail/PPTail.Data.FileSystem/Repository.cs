using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using PPTail.Interfaces;
using PPTail.Extensions;

namespace PPTail.Data.FileSystem
{
    public class Repository : Interfaces.IContentRepository
    {
        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 5;

        const String _connectionStringFilepathKey = "FilePath";

        const String _widgetRelativePath = "datastore\\widgets";
        const String _categoriesRelativePath = "categories.xml";
        const String _settingsFilename = "settings.xml";

        private readonly IServiceProvider _serviceProvider;
        private readonly String _rootDataPath;
        private readonly String _rootSitePath;

        SiteSettings _siteSettings = null;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be fixed in Globalization project")]
        public Repository(IServiceProvider serviceProvider, String connectionString)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<IFile>();

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _rootSitePath = connectionString.GetConnectionStringValue(_connectionStringFilepathKey);
            if (String.IsNullOrWhiteSpace(_rootSitePath))
                throw new ArgumentException($"{_connectionStringFilepathKey} not found in Connection String", nameof(connectionString));

            _rootDataPath = System.IO.Path.Combine(_rootSitePath, "App_Data");
        }

        public SiteSettings GetSiteSettings()
        {
            if (_siteSettings is null)
            {
                var fileSystem = _serviceProvider.GetService<IFile>();
                String settingsPath = System.IO.Path.Combine(_rootDataPath, _settingsFilename);
                _siteSettings = fileSystem.ReadAllText(settingsPath).ParseSettings();
                if (_siteSettings is null)
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
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            String pagePath = System.IO.Path.Combine(_rootDataPath, "pages");
            var files = directory.EnumerateFiles(pagePath);
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".xml")))
            {
                var contentItem = fileSystem.ReadAllText(file).ParseContentItem(file, "page");
                if (contentItem != null)
                    results.Add(contentItem);
            }
            return results;
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            String pagePath = System.IO.Path.Combine(_rootDataPath, "posts");
            var files = directory.EnumerateFiles(pagePath);
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".xml")))
            {
                var contentItem = fileSystem.ReadAllText(file).ParseContentItem(file, "post");
                if (contentItem != null)
                    results.Add(contentItem);
            }
            return results;
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();

            var results = new List<Widget>();
            String widgetPath = System.IO.Path.Combine(_rootDataPath, _widgetRelativePath);
            String zoneFilePath = System.IO.Path.Combine(widgetPath, "be_WIDGET_ZONE.xml");

            var zoneData = fileSystem.ReadAllText(zoneFilePath);
            var zones = XElement.Parse(zoneData);

            foreach (var widget in zones.Descendants().Where(d => d.Name.LocalName == "widget"))
            {
                var thisDictionary = new List<Tuple<string, string>>();
                Enumerations.WidgetType thisWidgetType = widget.Deserialize();

                var thisWidget = new Widget()
                {
                    Id = Guid.Parse(widget.Attributes().Single(a => a.Name == "id").Value),
                    Title = widget.Attributes().Single(a => a.Name == "title").Value,
                    ShowTitle = Boolean.Parse(widget.Attributes().Single(a => a.Name == "showTitle").Value),
                    WidgetType = thisWidgetType,
                    Dictionary = thisDictionary
                };

                String fileName = $"{thisWidget.Id.ToString()}.xml";
                String filePath = System.IO.Path.Combine(widgetPath, fileName);
                String widgetFile = string.Empty;
                if (fileSystem.Exists(filePath))
                    widgetFile = fileSystem.ReadAllText(filePath);

                if (!string.IsNullOrEmpty(widgetFile))
                {
                    var w = XElement.Parse(widgetFile);
                    var entry = w.Descendants().Single(n => n.Name.LocalName == "DictionaryEntry");
                    thisDictionary.Add(new Tuple<string, string>(entry.Attribute("Key").Value, entry.Attribute("Value").Value));
                }

                if (thisWidget.WidgetType != Enumerations.WidgetType.Unknown)
                    results.Add(thisWidget);
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
            const String categoryNodeName = "category";
            var fileSystem = _serviceProvider.GetService<IFile>();

            var results = new List<Category>();
            var path = System.IO.Path.Combine(_rootDataPath, _categoriesRelativePath);

            var fileContents = fileSystem.ReadAllText(path);
            var categoriesNode = XElement.Parse(fileContents);
            foreach (var categoryNode in categoriesNode.Descendants().Where(d => d.Name == categoryNodeName))
            {
                var idNode = categoryNode.Attributes().SingleOrDefault(d => d.Name == "id");
                var nameValue = categoryNode.Value;

                if (idNode == null || string.IsNullOrWhiteSpace(nameValue))
                {
                    // TODO: Log that this category was skipped
                }
                else
                {
                    var descriptionNode = categoryNode.Attributes().SingleOrDefault(d => d.Name == "description");
                    var description = (descriptionNode != null) ? descriptionNode.Value : string.Empty;

                    results.Add(new Category()
                    {
                        Id = Guid.Parse(idNode.Value),
                        Name = nameValue,
                        Description = description
                    });
                }
            }

            return results;
        }
    }
}
