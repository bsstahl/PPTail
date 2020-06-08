using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Templates.Yaml.Test
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDirectoryService(this IServiceCollection container)
        {
            string folderPath = string.Empty.GetRandom();
            var fileList = Array.Empty<String>();
            return container.AddDirectoryService(folderPath, fileList);
        }

        public static IServiceCollection AddDirectoryService(this IServiceCollection container, string folderPath, IEnumerable<String> fileList)
        {
            var mockDirectory = new Mock<IDirectory>();
            mockDirectory.Setup(d => d.EnumerateFiles(folderPath)).Returns(fileList);
            return container.AddDirectoryService(mockDirectory);
        }

        public static IServiceCollection AddDirectoryService(this IServiceCollection container, Mock<IDirectory> mockDirectory)
        {
            return container.AddSingleton<IDirectory>(mockDirectory.Object);
        }
    }
}
