using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;

namespace PPTail.Data.MediaBlog
{
    public class Repository : PPTail.Interfaces.IContentRepository
    {
        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 5;

        const String _connectionStringFilepathKey = "FilePath";

        private readonly String _rootPath;
        private readonly IServiceProvider _serviceProvider;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "To be fixed in Globalization project")]
        public Repository(IServiceProvider serviceProvider, string connectionString)
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
            var pagePath = System.IO.Path.Combine(_rootPath, "pages");
            var files = directory.EnumerateFiles(pagePath);

            foreach (var file in files.Where(f => f.ToUpperInvariant().EndsWith(".JSON", StringComparison.InvariantCulture)))
            {
                var contentJson = fileSystem.ReadAllText(file);
                var id = Guid.Parse(System.IO.Path.GetFileNameWithoutExtension(file));
                results.Add((null as ContentItem).FromJson(contentJson, id));
            }
            return results;
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            var postPath = System.IO.Path.Combine(_rootPath, "posts");

            if (directory.Exists(postPath))
            {
                var files = directory.EnumerateFiles(postPath);
                foreach (var file in files.Where(f => f.ToUpperInvariant().EndsWith(".JSON", StringComparison.InvariantCulture)))
                {
                    var json = fileSystem.ReadAllText(file);
                    var id = Guid.Parse(System.IO.Path.GetFileNameWithoutExtension(file));
                    var mediaPost = MediaPost.Create(json);
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

            var results = new List<Widget>();
            var zoneFilePath = System.IO.Path.Combine(_rootPath, "Widgets.json");

            var zoneData = fileSystem.ReadAllText(zoneFilePath);
            var widgetZones = Newtonsoft.Json.JsonConvert.DeserializeObject<WidgetZone[]>(zoneData);

            foreach (var zone in widgetZones)
            {
                var thisDictionary = new List<Tuple<String, String>>
                {
                    new Tuple<String, String>("Content", zone.Content)
                };

                var thisWidgetType = (Enumerations.WidgetType)Enum.Parse(typeof(Enumerations.WidgetType), zone.WidgetType);

                if (zone.Active)
                {
                    var thisWidget = new Widget()
                    {
                        Id = Guid.Parse(zone.Id),
                        Title = zone.Title,
                        ShowTitle = zone.ShowTitle,
                        WidgetType = thisWidgetType,
                        Dictionary = thisDictionary
                    };

                    if (thisWidget.WidgetType != Enumerations.WidgetType.Unknown)
                    {
                        results.Add(thisWidget);
                    }
                }
            }

            return results;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categoriesFilePath = System.IO.Path.Combine(_rootPath, "Categories.json");
            var fileSystem = _serviceProvider.GetService<IFile>();
            var json = fileSystem.ReadAllText(categoriesFilePath);
            return JsonConvert.DeserializeObject<IEnumerable<Category>>(json);
        }

        public IEnumerable<SourceFile> GetFolderContents(String relativePath)
        {
            return GetFolderContents(relativePath, false);
        }

        public IEnumerable<SourceFile> GetFolderContents(String relativePath, bool recursive)
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var folderPath = System.IO.Path.Combine(_rootPath, relativePath);
            var results = new List<SourceFile>();

            if (directory.Exists(folderPath))
            {
                var sourceFiles = directory.EnumerateFiles(folderPath, recursive);
                foreach (var sourceFile in sourceFiles)
                {
                    Byte[] contents = null;
                    try
                    {
                        contents = fileSystem.ReadAllBytes(sourceFile);
                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        // TODO: Log this occurrance
                    }

                    if (contents != null)
                    {
                        results.Add(new SourceFile()
                        {
                            Contents = contents,
                            FileName = System.IO.Path.GetFileName(sourceFile),
                            RelativePath = relativePath
                        });
                    }
                }
            }

            return results;
        }

        public SiteSettings GetSiteSettings()
        {
            var settingsFilePath = System.IO.Path.Combine(_rootPath, "SiteSettings.json");
            var fileProvider = _serviceProvider.GetService<IFile>();
            var json = fileProvider.ReadAllText(settingsFilePath);

            SiteSettings result;
            try
            {
                result = JsonConvert.DeserializeObject<SiteSettings>(json);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                // Site settings file not parseable 
                // as a SiteSettings object
                result = null;
            }

            if (result == null)
            {
                throw new Exceptions.SettingNotFoundException(nameof(SiteSettings));
            }

            if (result.PostsPerPage == 0)
            {
                result.PostsPerPage = _defaultPostsPerPage;
            }

            if (result.PostsPerFeed == 0)
            {
                result.PostsPerFeed = _defaultPostsPerFeed;
            }

            return result;
        }
    }
}
