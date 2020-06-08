using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.FileSystem.Test
{
    public class ReadRepository_GetAllTemplates_Should
    {
        [Fact]
        public void EnumerateTheProperDirectory()
        {
            string templatePath = string.Empty.GetRandom();

            var mockDirectory = new Mock<IDirectory>();
            mockDirectory
                .Setup(d => d.EnumerateFiles(templatePath))
                .Returns(Array.Empty<String>())
                .Verifiable();

            var serviceProvider = new ServiceCollection()
                .AddDirectoryService(mockDirectory)
                .BuildServiceProvider();

            var target = new ReadRepository(serviceProvider, templatePath);
            var actual = target.GetAllTemplates();

            mockDirectory.VerifyAll();
        }


    }
}
