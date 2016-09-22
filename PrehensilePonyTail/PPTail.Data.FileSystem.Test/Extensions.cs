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
    public static class Extensions
    {
        const string _siteSettingsXmlTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?><settings><name>{0}</name><description>{1}</description><postsperpage>{2}</postsperpage></settings>";
        const string _sourceDataPathSettingName = "sourceDataPath";
        const string _widgetZoneNodeFormat = "<widget id=\"{0}\" title=\"{1}\" showTitle=\"{2}\">{3}</widget>";


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
                var widgetType = (Enumerations.WidgetType)3.GetRandom();
                result.Add(widgetType.CreateWidget());
            }

            return result;
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

        public static void ConfigureWidgets(this Mock<IFileSystem> fileSystem, IEnumerable<Widget> widgets, string rootPath)
        {
            fileSystem.ConfigureWidgets(widgets, rootPath, false);
        }

        public static void ConfigureWidgets(this Mock<IFileSystem> fileSystem, IEnumerable<Widget> widgets, string rootPath, bool addInvalidTypes)
        {
            const string widgetPath = ".\\datastore\\widgets";
            const string zoneFilename = "be_WIDGET_ZONE.xml";
            const string widgetFileFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableStringDictionary><SerializableStringDictionary><DictionaryEntry Key=\"{0}\" Value=\"{1}\" /></SerializableStringDictionary></SerializableStringDictionary>";

            var files = new List<string>();
            foreach (var widget in widgets)
            {
                string fileName = $"{widget.Id}.xml";
                string thisFilePath = System.IO.Path.Combine(rootPath, widgetPath, fileName);

                if (widget.WidgetType == Enumerations.WidgetType.TextBox)
                {
                    var dictionaryItem = widget.Dictionary.First();
                    files.Add(fileName);
                    fileSystem.Setup(f => f.ReadAllText(thisFilePath))
                        .Returns(string.Format(widgetFileFormat, dictionaryItem.Item1, dictionaryItem.Item2));
                }
                else if (widget.WidgetType == Enumerations.WidgetType.Tag_cloud)
                {
                    fileSystem.Setup(f => f.ReadAllText(thisFilePath))
                        .Throws(new System.IO.FileNotFoundException("This widget type has no separate files"));
                }
            }

            files.Add(zoneFilename);

            var widgetNode = widgets.Serialize();
            if (addInvalidTypes)
            {
                string nodeText = string.Format(_widgetZoneNodeFormat, Guid.NewGuid().ToString(), string.Empty.GetRandom(), true.GetRandom(), string.Empty.GetRandom());
                widgetNode.Add(XElement.Parse(nodeText));
            }

            string zoneFilePath = System.IO.Path.Combine(rootPath, widgetPath, zoneFilename);
            fileSystem.Setup(f => f.ReadAllText(zoneFilePath))
                .Returns(widgetNode.ToString());
        }

        public static XElement Serialize(this IEnumerable<Widget> widgets)
        {
            var widgetNode = XElement.Parse("<?xml version=\"1.0\" encoding=\"utf-8\"?><widgets></widgets>");
            foreach (var widget in widgets)
            {
                string nodeText = string.Format(_widgetZoneNodeFormat, widget.Id.ToString(), widget.Title, widget.ShowTitle.ToString(), widget.WidgetType.Serialize());
                widgetNode.Add(XElement.Parse(nodeText));
            }

            return widgetNode;
        }

        public static string Serialize(this WidgetType widgetType)
        {
            return widgetType.ToString().Replace("_", " ");
        }

        public static string Serialize(this Widget widget)
        {
            //             const string widgetFileFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableStringDictionary><SerializableStringDictionary><DictionaryEntry Key=\"{0}\" Value=\"{1}\" /></SerializableStringDictionary></SerializableStringDictionary>";
            throw new NotImplementedException();
        }
    }
}