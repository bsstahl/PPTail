using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.IO;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.Yaml.Test
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

            var target = new Yaml.ReadRepository(serviceProvider, templatePath);
            var actual = target.GetAllTemplates();

            mockDirectory.VerifyAll();
        }

    }
}
