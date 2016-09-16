using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using System.Xml.Linq;

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

    }
}
