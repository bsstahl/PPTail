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

namespace PPTail.Data.FileSystem.Test
{
    public static class Extensions
    {
        const string _siteSettingsXmlTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?><settings><name>{0}</name><description>{1}</description><postsperpage>{2}</postsperpage></settings>";
        const string _sourceDataPathSettingName = "sourceDataPath";

        public static IContentRepository Create(this IContentRepository ignore)
        {
            IFileSystem mockFileSystem = Mock.Of<IFileSystem>();
            return ignore.Create(mockFileSystem, "c:\\");
        }

        public static IContentRepository Create(this IContentRepository ignore, IFileSystem fileSystem, string sourcePath)
        {
            var container = new ServiceCollection();

            var settings = new Settings();
            container.AddSingleton<Settings>(settings);
            settings.ExtendedSettings.Set(_sourceDataPathSettingName, sourcePath);

            container.AddSingleton<IFileSystem>(fileSystem);

            return ignore.Create(container);
        }

        public static IContentRepository Create(this IContentRepository ignore, IServiceCollection container)
        {
            return new PPTail.Data.FileSystem.Repository(container);
        }

        public static string BuildXml(this SiteSettings ignore, string title, string description, int postsPerPage)
        {
            return string.Format(_siteSettingsXmlTemplate, title, description, postsPerPage.ToString());
        }

        public static XElement RemoveDescendants(this XElement element, string elementLocalName)
        {
            var target = element.Descendants().Where(n => n.Name.LocalName == elementLocalName);
            target.Remove();
            return element;
        }

        public static IEnumerable<Widget> Create(this IEnumerable<Widget> ignore)
        {
            return ignore.Create(10.GetRandom(3));
        }

        public static IEnumerable<Widget> Create(this IEnumerable<Widget> ignore, int count)
        {
            var result = new List<Widget>();

            for (int i = 0; i < count; i++)
            {
                result.Add(new Widget()
                {
                    Id = Guid.NewGuid(),
                    Title = string.Empty.GetRandom(),
                    ShowTitle = true.GetRandom(),
                    WidgetType = Enumerations.WidgetType.TextBox,
                    Dictionary = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("content", string.Empty.GetRandom())
                    }
                });
            }

            return result;
        }

        public static void ConfigureWidgets(this Mock<IFileSystem> fileSystem, IEnumerable<Widget> widgets, string rootPath)
        {
            const string widgetPath = ".\\datastore\\widgets";
            const string zoneFilename = "be_WIDGET_ZONE.xml";
            const string widgetFileFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableStringDictionary><SerializableStringDictionary><DictionaryEntry Key=\"{0}\" Value=\"{1}\" /></SerializableStringDictionary></SerializableStringDictionary>";

            var files = new List<string>();
            foreach (var widget in widgets)
            {
                string fileName = $"{widget.Id}.xml";
                string thisFilePath = System.IO.Path.Combine(rootPath, widgetPath, fileName);
                var dictionaryItem = widget.Dictionary.First();

                files.Add(fileName);
                fileSystem.Setup(f => f.ReadAllText(thisFilePath))
                    .Returns(string.Format(widgetFileFormat, dictionaryItem.Item1, dictionaryItem.Item2));
            }

            files.Add(zoneFilename);

            var widgetNode = widgets.Serialize();
            string zoneFilePath = System.IO.Path.Combine(rootPath, widgetPath, zoneFilename);
            fileSystem.Setup(f => f.ReadAllText(zoneFilePath))
                .Returns(widgetNode.ToString());

            // fileSystem.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
            //    .Returns(files);
        }

        public static string Serialize(this IEnumerable<Widget> widgets)
        {
            const string widgetZoneNodeFormat = "<widget id=\"{0}\" title=\"{1}\" showTitle=\"{2}\">{3}</widget>";

            var widgetNode = XElement.Parse("<?xml version=\"1.0\" encoding=\"utf-8\"?><widgets></widgets>");
            foreach (var widget in widgets)
            {
                string nodeText = string.Format(widgetZoneNodeFormat, widget.Id.ToString(), widget.Title, widget.ShowTitle.ToString(), widget.WidgetType.ToString());
                widgetNode.Add(XElement.Parse(nodeText));
            }

            return widgetNode.ToString();
        }

        public static string Serialize(this Widget widget)
        {
            //             const string widgetFileFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableStringDictionary><SerializableStringDictionary><DictionaryEntry Key=\"{0}\" Value=\"{1}\" /></SerializableStringDictionary></SerializableStringDictionary>";
            throw new NotImplementedException();
        }
    }
}