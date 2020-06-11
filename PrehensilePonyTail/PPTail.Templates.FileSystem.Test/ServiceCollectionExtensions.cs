using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Templates.FileSystem.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileService(this IServiceCollection container)
        {
            String fileContent = String.Empty.GetRandom();
            return container.AddFileService(fileContent);
        }

        public static IServiceCollection AddFileService(this IServiceCollection container, string fileContent)
        {
            var mockFileService = new Mock<IFile>();
            mockFileService.Setup(f => f.ReadAllText(It.IsAny<String>())).Returns(fileContent);
            return container.AddFileService(mockFileService);
        }

        public static IServiceCollection AddFileService(this IServiceCollection container, Mock<IFile> mockFileService)
        {
            return container.AddSingleton<IFile>(mockFileService.Object);
        }
    }
}
