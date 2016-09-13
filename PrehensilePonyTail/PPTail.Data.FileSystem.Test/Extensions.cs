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
        public static IContentRepository Create(this IContentRepository ignore)
        {
            IFileSystem mockFileSystem = Mock.Of<IFileSystem>();
            IServiceCollection container = new ServiceCollection();
            container.AddSingleton<IFileSystem>(mockFileSystem);
            return ignore.Create(container);
        }

        public static IContentRepository Create(this IContentRepository ignore, IServiceCollection container)
        {
            return ignore.Create(container, "c:\\");
        }

        public static IContentRepository Create(this IContentRepository ignore, IServiceCollection container, string rootPath)
        {
            return new PPTail.Data.FileSystem.Repository(container.BuildServiceProvider(), rootPath);
        }
    }
}
