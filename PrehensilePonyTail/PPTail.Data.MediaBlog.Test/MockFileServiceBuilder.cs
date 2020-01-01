using Moq;
using Newtonsoft.Json;
using PPTail.Builders;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    public class MockFileServiceBuilder
    {
        const String _widgetFilePath = "Widgets.json";
        const String _categoryFilePath = "Categories.json";
        const String _settingsFilePath = "SiteSettings.json";

        readonly List<(String FilePath, String Content)> _posts = new List<(string, string)>();
        readonly List<WidgetZone> _widgets = new List<WidgetZone>();
        readonly List<Category> _categories = new List<Category>();
        readonly List<SecurableSourceFile> _sourceFiles = new List<SecurableSourceFile>();

        String _settingsFile = string.Empty;
        SiteSettings _siteSettings = null;

        internal Mock<IFile> Build()
        {
            var fileSystem = new Mock<IFile>();

            foreach (var (filePath, content) in _posts)
            {
                fileSystem.Setup(f => f.ReadAllText(filePath))
                    .Returns(content).Verifiable();
            }

            foreach (var sourceFile in _sourceFiles)
            {
                String filePath = Path.Combine(sourceFile.RelativePath, sourceFile.FileName);
                if (sourceFile.IsSecured)
                {
                    fileSystem.Setup(f => f.ReadAllBytes(filePath))
                        .Throws(new System.UnauthorizedAccessException());
                }
                else
                {
                    fileSystem.Setup(f => f.ReadAllBytes(filePath))
                        .Returns(sourceFile.Contents).Verifiable();
                }
            }

            var widgetContent = JsonConvert.SerializeObject(_widgets);
            fileSystem.Setup(f => f.ReadAllText(_widgetFilePath))
                .Returns(widgetContent).Verifiable();

            var categoryContent = JsonConvert.SerializeObject(_categories);
            fileSystem.Setup(f => f.ReadAllText(_categoryFilePath))
                .Returns(categoryContent).Verifiable();

            // If a SiteSettings object has been provided it
            // will override any settings file that has been supplied
            if (_siteSettings != null)
            {
                _settingsFile = JsonConvert.SerializeObject(_siteSettings);
            }

            if (!string.IsNullOrEmpty(_settingsFile))
            {
                fileSystem.Setup(f => f.ReadAllText(_settingsFilePath))
                    .Returns(_settingsFile)
                    .Verifiable();
            }

            return fileSystem;
        }

        internal MockFileServiceBuilder AddPost((string, string) file)
        {
            _posts.Add(file);
            return this;
        }

        internal MockFileServiceBuilder AddPosts(IEnumerable<MockMediaFile> files)
        {
            foreach (var file in files)
            {
                _posts.AddRange(files.Select(f => (f.GetFilename(), f.Contents)));
            }

            return this;
        }

        internal MockFileServiceBuilder AddRandomWidgets(Int32 count)
        {
            for (Int32 i = 0; i < count; i++)
            {
                this.AddRandomWidget();
            }

            return this;
        }

        internal MockFileServiceBuilder AddRandomWidget()
        {
            return this.AddWidget(
                new WidgetZoneBuilder()
                    .UseRandom()
                    .Build());
        }

        internal MockFileServiceBuilder AddWidget(WidgetZone widgetFile)
        {
            _widgets.Add(widgetFile);
            return this;
        }

        internal MockFileServiceBuilder AddWidgets(IEnumerable<WidgetZone> widgets)
        {
            _widgets.AddRange(widgets);
            return this;
        }

        internal MockFileServiceBuilder AddCategories(IEnumerable<Category> categories)
        {
            _categories.AddRange(categories);
            return this;
        }

        internal MockFileServiceBuilder AddRandomCategory()
        {
            _categories.Add(new CategoryBuilder()
                .UseRandom()
                .Build());
            return this;
        }

        internal MockFileServiceBuilder AddRandomCategories(Int32 count)
        {
            for (Int32 i = 0; i < count; i++)
            {
                this.AddRandomCategory();
            }

            return this;
        }

        internal MockFileServiceBuilder AddSourceFile(SourceFile file)
        {
            _sourceFiles.Add(new SecurableSourceFile(file));
            return this;
        }

        internal MockFileServiceBuilder AddSecuredSourceFile(SourceFile file, bool isSecured)
        {
            _sourceFiles.Add(new SecurableSourceFile(file, isSecured));
            return this;
        }

        internal MockFileServiceBuilder AddSourceFiles(IEnumerable<SourceFile> files)
        {
            foreach (var file in files)
            {
                this.AddSourceFile(file);
            }

            return this;
        }

        internal MockFileServiceBuilder AddSiteSettingsFile(String siteSettings)
        {
            _settingsFile = siteSettings;
            _siteSettings = null;
            return this;
        }

        internal MockFileServiceBuilder AddSiteSettings(SiteSettings siteSettings)
        {
            _siteSettings = siteSettings;
            return this;
        }
    }
}
