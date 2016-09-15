using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace PPTail.Data.FileSystem.Test
{
    public static class Extensions
    {
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


    }
}
