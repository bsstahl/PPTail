using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using System.Xml.Linq;
using TestHelperExtensions;
using PPTail.Enumerations;

namespace PPTail.Data.FileSystem.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        const String _sourceDataPathSettingName = "sourceDataPath";
        const String _connectionStringFilepathKey = "FilePath";

        const String _widgetZoneNodeFormat = "<widget id=\"{0}\" title=\"{1}\" showTitle=\"{2}\">{3}</widget>";
        const String _categoryNodeFormat = "<category id=\"{0}\" description=\"{2}\" parent=\"\">{1}</category>";

        public static IContentRepository Create(this IContentRepository ignore)
        {
            IFile mockFileSystem = Mock.Of<IFile>();
            return ignore.Create(mockFileSystem, "c:\\");
        }

        public static IContentRepository Create(this IContentRepository ignore, IFile fileSystem, String sourcePath)
        {
            return ignore.Create(fileSystem, Mock.Of<IDirectory>(), sourcePath);
        }

        public static IContentRepository Create(this IContentRepository? ignore, IFile fileSystem, IDirectory directoryProvider, String sourcePath)
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);
            container.AddSingleton<IDirectory>(directoryProvider);

            String sourceConnection = $"Provider=Test;{_connectionStringFilepathKey}={sourcePath}";
            return ignore.Create(container.BuildServiceProvider(), sourceConnection);
        }

        public static IContentRepository Create(this IContentRepository? _, IServiceProvider serviceProvider, string connectionString)
        {
            return new Repository(serviceProvider, connectionString);
        }

        public static XElement ConditionalAddNode(this XElement node, bool addNode, String name, String value)
        {
            if (addNode)
                node.Add(new XElement(XName.Get(name), value));
            return node;
        }

        public static IEnumerable<Widget> Create(this IEnumerable<Widget> ignore)
        {
            return ignore.Create(25.GetRandom(10));
        }

        public static IEnumerable<Widget> Create(this IEnumerable<Widget> ignore, Int32 count)
        {
            var result = new List<Widget>();

            var widgetTypes = Enum.GetValues(typeof(Enumerations.WidgetType));
            Int32 loopCount = Convert.ToInt32(System.Math.Ceiling(Convert.ToDouble(count / 3)));
            for (Int32 i = 0; i < loopCount; i++)
            {
                foreach (WidgetType widgetType in widgetTypes)
                    result.Add(widgetType.CreateWidget());
            }

            return result.Take(count);
        }

        public static Widget CreateWidget(this Enumerations.WidgetType widgetType)
        {
            return new Widget()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty.GetRandom(),
                ShowTitle = true.GetRandom(),
                WidgetType = widgetType,
                Dictionary = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("content", string.Empty.GetRandom())
                    }
            };
        }

        public static void ConfigureWidgets(this Mock<IFile> fileSystem, IEnumerable<Widget> widgets, String rootPath)
        {
            fileSystem.ConfigureWidgets(widgets, rootPath, false);
        }

        public static void ConfigureWidgets(this Mock<IFile> fileSystem, IEnumerable<Widget> widgets, String rootPath, bool addInvalidTypes)
        {
            const String widgetPath = "App_Data\\datastore\\widgets";
            const String zoneFilename = "be_WIDGET_ZONE.xml";
            const String widgetFileFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableStringDictionary><SerializableStringDictionary><DictionaryEntry Key=\"{0}\" Value=\"{1}\" /></SerializableStringDictionary></SerializableStringDictionary>";

            var files = new List<string>();
            foreach (var widget in widgets)
            {
                String fileName = $"{widget.Id}.xml";
                String thisFilePath = System.IO.Path.Combine(rootPath, widgetPath, fileName);

                if (widget.WidgetType == Enumerations.WidgetType.TextBox)
                {
                    var dictionaryItem = widget.Dictionary.First();
                    files.Add(fileName);
                    fileSystem.Setup(f => f.Exists(thisFilePath)).Returns(true);
                    fileSystem.Setup(f => f.ReadAllText(thisFilePath))
                        .Returns(string.Format(widgetFileFormat, dictionaryItem.Item1, dictionaryItem.Item2));
                }
                else if (widget.WidgetType == Enumerations.WidgetType.Tag_cloud)
                {
                    fileSystem.Setup(f => f.Exists(thisFilePath)).Returns(false);
                    fileSystem.Setup(f => f.ReadAllText(thisFilePath))
                        .Throws(new System.IO.FileNotFoundException("This widget type has no separate files"));
                }
            }

            files.Add(zoneFilename);

            var widgetNode = widgets.Serialize();
            if (addInvalidTypes)
            {
                // Alphanumeric node value
                String nodeText = string.Format(_widgetZoneNodeFormat, Guid.NewGuid().ToString(), string.Empty.GetRandom(), true.GetRandom(), string.Empty.GetRandom());
                widgetNode.Add(XElement.Parse(nodeText));

                // Numeric node value
                nodeText = string.Format(_widgetZoneNodeFormat, Guid.NewGuid().ToString(), string.Empty.GetRandom(), true.GetRandom(), 50.GetRandom(20));
                widgetNode.Add(XElement.Parse(nodeText));
            }

            String zoneFilePath = System.IO.Path.Combine(rootPath, widgetPath, zoneFilename);
            fileSystem.Setup(f => f.ReadAllText(zoneFilePath))
                .Returns(widgetNode.ToString());
        }

        public static void ConfigureCategories(this Mock<IFile> fileSystem, IEnumerable<Category> categories, String rootPath)
        {
            const String dataPath = "App_Data\\categories.xml";
            var categoryFileContents = categories.Serialize();
            var categoryFilePath = System.IO.Path.Combine(rootPath, dataPath);
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);
        }

        public static void ConfigureCategories(this Mock<IFile> fileSystem, XElement categoryFileContents, String categoryFilePath)
        {
            fileSystem.Setup(f => f.Exists(categoryFilePath)).Returns(true);
            fileSystem.Setup(f => f.ReadAllText(categoryFilePath))
                .Returns(categoryFileContents.ToString());
        }

        public static XElement Serialize(this IEnumerable<Widget> widgets)
        {
            var widgetNode = XElement.Parse("<?xml version=\"1.0\" encoding=\"utf-8\"?><widgets></widgets>");
            foreach (var widget in widgets)
            {
                String nodeText = string.Format(_widgetZoneNodeFormat, widget.Id.ToString(), widget.Title, widget.ShowTitle.ToString(), widget.WidgetType.Serialize());
                widgetNode.Add(XElement.Parse(nodeText));
            }

            return widgetNode;
        }

        public static String Serialize(this WidgetType widgetType)
        {
            return widgetType.ToString().Replace("_", " ");
        }

        public static XElement Serialize(this IEnumerable<Category> categories)
        {
            var rootNode = XElement.Parse("<?xml version=\"1.0\" encoding=\"utf-8\"?><categories></categories>");
            foreach (var category in categories)
            {
                String nodeText = string.Format(_categoryNodeFormat, category.Id.ToString(), category.Name, category.Description);
                rootNode.Add(XElement.Parse(nodeText));
            }

            return rootNode;
        }

        public static IEnumerable<SourceFile> Create(this IEnumerable<SourceFile>? _, String relativePath, Int32 count)
        {
            var result = new List<SourceFile>();

            for (Int32 i = 0; i < count; i++)
                result.Add((null as SourceFile).Create(relativePath));

            return result;
        }

        private static SourceFile Create(this SourceFile ignore, String relativePath)
        {
            return new SourceFile()
            {
                Contents = string.Empty.GetRandom().Select(c => Convert.ToByte(c)).ToArray(),
                FileName = $"{string.Empty.GetRandom()}.{string.Empty.GetRandom(3)}",
                RelativePath = relativePath
            };
        }

        public static Category Create(this Category ignore)
        {
            var id = Guid.NewGuid();
            String name = $"nameof_{id.ToString()}";
            return ignore.Create(id, name);
        }

        public static Category Create(this Category ignore, Guid id, String name)
        {
            String description = $"descriptionof_{id.ToString()}";
            return ignore.Create(id, name, description);
        }

        public static Category Create(this Category ignore, Guid id, String name, String description)
        {
            return new Category()
            {
                Id = id,
                Name = name,
                Description = description
            };
        }

        public static IEnumerable<Category> Create(this IEnumerable<Category> ignore)
        {
            return ignore.Create(10.GetRandom(5));
        }

        public static IEnumerable<Category> Create(this IEnumerable<Category> ignore, Int32 count)
        {
            var result = new List<Category>();
            for (Int32 i = 0; i < count; i++)
                result.Add((null as Category).Create());
            return result;
        }

        public static IEnumerable<Guid> GetRandomCategoryIds(this IEnumerable<Category> categories)
        {
            // Returns 1 or 2 category IDs from the collection of categories
            var result = new List<Guid>();
            Category cat1 = categories.GetRandom();
            Category cat2 = null;

            result.Add(cat1.Id);
            if (true.GetRandom())
            {
                do
                {
                    cat2 = categories.GetRandom();
                } while (cat1.Id == cat2.Id);

                result.Add(cat2.Id);
            }

            return result;
        }
    }
}