using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PPTail.Entities;
using PPTail.Extensions;
using PPTail.Interfaces;

namespace PPTail.Data.PhotoBlog
{
    public class Repository : PPTail.Interfaces.IContentRepository
    {
        private const String _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";
        private const String _createDasBlogPostsCompatibilityFileSettingName = "createDasBlogPostsCompatibilityFile";
        private const String _connectionStringFilepathKey = "FilePath";

        private readonly String _rootPath;
        private readonly IServiceProvider _serviceProvider;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<IFile>();

            var settings = _serviceProvider.GetService<ISettings>();
            _rootPath = settings.SourceConnection.GetConnectionStringValue(_connectionStringFilepathKey);

            // HACK: Updating settings shouldn't be done here, it probably should be done from the command line
            _ = settings.ExtendedSettings.Set(_createDasBlogSyndicationCompatibilityFileSettingName, false.ToString());
            _ = settings.ExtendedSettings.Set(_createDasBlogPostsCompatibilityFileSettingName, false.ToString());
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            var pagePath = System.IO.Path.Combine(_rootPath, "pages");
            var files = directory.EnumerateFiles(pagePath);
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".json")))
            {
                var contentJson = fileSystem.ReadAllText(file);
                var contentItem = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.ContentItem>(contentJson);
                if (contentItem != null)
                {
                    contentItem.Id = Guid.Parse(System.IO.Path.GetFileNameWithoutExtension(file));
                    results.Add(contentItem);
                }
            }
            return results;
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            var postPath = System.IO.Path.Combine(_rootPath, "posts");
            var files = directory.EnumerateFiles(postPath);
            foreach (var file in files.Where(f => f.ToLowerInvariant().EndsWith(".json")))
            {
                var json = fileSystem.ReadAllText(file);
                var imagePost = Newtonsoft.Json.JsonConvert.DeserializeObject<FlickrImagePost>(json);
                var id = Guid.Parse(System.IO.Path.GetFileNameWithoutExtension(file));
                var contentItem = imagePost.AsContentItem(id);
                if (contentItem != null)
                {
                    results.Add(contentItem);
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
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Category>>(json);
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
                    Byte[] contents = null;
                    try
                    {
                        contents = fileSystem.ReadAllBytes(sourceFile);
                    }
                    catch (System.UnauthorizedAccessException)
                    { }

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
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SiteSettings>(json);
        }
    }
}
