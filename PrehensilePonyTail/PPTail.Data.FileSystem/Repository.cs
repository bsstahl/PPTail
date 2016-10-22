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
        const int _defaultPostsPerPage = 3;
        const int _defaultPostsPerFeed = 5;

        const string _sourceDataPathSettingName = "sourceDataPath";
        const string _widgetRelativePath = "datastore\\widgets";
        const string _categoriesRelativePath = "categories.xml";

        private readonly IServiceProvider _serviceProvider;
        private readonly string _rootDataPath;
        private readonly string _rootSitePath;

        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _serviceProvider.ValidateService<ISettings>();
            _serviceProvider.ValidateService<IFile>();

            var settings = _serviceProvider.GetService<ISettings>();
            var fileSystem = _serviceProvider.GetService<IFile>();

            settings.Validate(_sourceDataPathSettingName);

            _rootSitePath = settings.ExtendedSettings.Get(_sourceDataPathSettingName);
            _rootDataPath = System.IO.Path.Combine(_rootSitePath, "App_Data");
        }

        public SiteSettings GetSiteSettings()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            string settingsPath = System.IO.Path.Combine(_rootDataPath, "settings.xml");
            var result = fileSystem.ReadAllText(settingsPath).ParseSettings();
            if (result == null)
                throw new Exceptions.SettingNotFoundException(typeof(SiteSettings).Name);

            if (string.IsNullOrWhiteSpace(result.Title))
                throw new Exceptions.SettingNotFoundException(nameof(result.Title));

            if (result.PostsPerPage == 0)
                result.PostsPerPage = _defaultPostsPerPage;

            if (result.PostsPerFeed == 0)
                result.PostsPerFeed = _defaultPostsPerFeed;

            return result;
        }

        public IEnumerable<ContentItem> GetAllPages()
        {
            var fileSystem = _serviceProvider.GetService<IFile>();
            var directory = _serviceProvider.GetService<IDirectory>();

            var results = new List<ContentItem>();
            string pagePath = System.IO.Path.Combine(_rootDataPath, "pages");
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
            string pagePath = System.IO.Path.Combine(_rootDataPath, "posts");
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
            string widgetPath = System.IO.Path.Combine(_rootDataPath, _widgetRelativePath);
            string zoneFilePath = System.IO.Path.Combine(widgetPath, "be_WIDGET_ZONE.xml");

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

                string fileName = $"{thisWidget.Id.ToString()}.xml";
                string filePath = System.IO.Path.Combine(widgetPath, fileName);
                string widgetFile = string.Empty;
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

        public IEnumerable<SourceFile> GetFolderContents(string relativePath)
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
            const string categoryNodeName = "category";
            var fileSystem = _serviceProvider.GetService<IFile>();

            // TODO: Add defensive code to handle error conditions
            var results = new List<Category>();
            var path = System.IO.Path.Combine(_rootDataPath, _categoriesRelativePath);

            var fileContents = fileSystem.ReadAllText(path);
            var categoriesNode = XElement.Parse(fileContents);
            foreach (var categoryNode in categoriesNode.Descendants().Where(d => d.Name == categoryNodeName))
                results.Add(new Category()
                {
                    Id = Guid.Parse(categoryNode.Attributes().Single(d => d.Name == "id").Value),
                    Name = categoryNode.Value,
                    Description = categoryNode.Attributes().Single(d => d.Name == "description").Value
                });

            return results;
        }
    }
}
