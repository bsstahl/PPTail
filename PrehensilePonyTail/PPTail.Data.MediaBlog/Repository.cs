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
        const int _defaultPostsPerPage = 3;
        const int _defaultPostsPerFeed = 5;

        const string _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";
        const string _createDasBlogPostsCompatibilityFileSettingName = "createDasBlogPostsCompatibilityFile";

        const string _connectionStringFilepathKey = "FilePath";

        private readonly string _rootPath;
        private readonly IServiceProvider _serviceProvider;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<IFile>();
            _serviceProvider.ValidateService<IDirectory>();

            var settings = _serviceProvider.GetService<ISettings>();

            if (string.IsNullOrWhiteSpace(settings.SourceConnection))
                throw new Exceptions.SettingNotFoundException(nameof(settings.SourceConnection));

            _rootPath = settings.SourceConnection.GetConnectionStringValue(_connectionStringFilepathKey);

            // HACK: Updating settings shouldn't be done here, it probably should be done from the command line
            settings.ExtendedSettings.Set(_createDasBlogSyndicationCompatibilityFileSettingName, false.ToString());
            settings.ExtendedSettings.Set(_createDasBlogPostsCompatibilityFileSettingName, false.ToString());
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            string pagePath = System.IO.Path.Combine(_rootPath, "pages");
            var files = directory.EnumerateFiles(pagePath);

            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".json")))
            {
                var contentJson = fileSystem.ReadAllText(file);
                Guid id = Guid.Parse(System.IO.Path.GetFileNameWithoutExtension(file));
                results.Add((null as ContentItem).FromJson(contentJson, id));
            }
            return results;
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            string postPath = System.IO.Path.Combine(_rootPath, "posts");

            if (directory.Exists(postPath))
            {
                var files = directory.EnumerateFiles(postPath);
                foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".json")))
                {
                    var json = fileSystem.ReadAllText(file);
                    Guid id = Guid.Parse(System.IO.Path.GetFileNameWithoutExtension(file));
                    var mediaPost = MediaPost.Create(json);
                    var contentItem = mediaPost.AsContentItem(id);
                    if (contentItem != null)
                        results.Add(contentItem);
                }
            }

            return results;
        }

        public IEnumerable<Widget> GetAllWidgets()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();

            var results = new List<Widget>();
            string zoneFilePath = System.IO.Path.Combine(_rootPath, "Widgets.json");

            var zoneData = fileSystem.ReadAllText(zoneFilePath);
            var widgetZones = Newtonsoft.Json.JsonConvert.DeserializeObject<WidgetZone[]>(zoneData);

            foreach (var zone in widgetZones)
            {
                var thisDictionary = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("Content", zone.Content)
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
                        results.Add(thisWidget);
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

        public IEnumerable<SourceFile> GetFolderContents(string relativePath)
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
                    {
                        // TODO: Log this occurrance
                    }

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
                throw new Exceptions.SettingNotFoundException(nameof(SiteSettings));

            if (result.PostsPerPage == 0)
                result.PostsPerPage = _defaultPostsPerPage;

            if (result.PostsPerFeed == 0)
                result.PostsPerFeed = _defaultPostsPerFeed;

            return result;
        }
    }
}
