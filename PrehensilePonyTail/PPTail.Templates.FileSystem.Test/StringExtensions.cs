using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.FileSystem.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class StringExtensions
    {
        const string _connectionFormat = "Provider=PPTail.Templates.FileSystem.ReadRepository;FilePath={0}";


        internal static void ExecuteTemplateRetrievalTest(this String templateFilename)
        {
            string templatePath = string.Empty.GetRandom();
            string expected = System.IO.Path.Combine(templatePath, templateFilename);

            var mockFileService = new Mock<IFile>();

            mockFileService
                .Setup(f => f.ReadAllText(expected))
                .Returns(string.Empty.GetRandom())
                .Verifiable();

            var serviceProvider = new ServiceCollection()
                .AddFileService(mockFileService)
                .BuildServiceProvider();

            string connection = String.Format(_connectionFormat, templatePath);
            var target = new FileSystem.ReadRepository(serviceProvider, connection);
            var actual = target.GetAllTemplates();

            mockFileService.Verify();
        }

        internal static void ExecuteTemplateContentTest(this String templateFilename, Enumerations.TemplateType templateType)
        {
            string expected = string.Empty.GetRandom();

            string templatePath = string.Empty.GetRandom();
            string expectedPath = System.IO.Path.Combine(templatePath, templateFilename);

            var mockFileService = new Mock<IFile>();
            mockFileService
                .Setup(f => f.ReadAllText(It.IsAny<String>()))
                .Returns(string.Empty.GetRandom());
            mockFileService
                .Setup(f => f.ReadAllText(expectedPath))
                .Returns(expected);

            var serviceProvider = new ServiceCollection()
                .AddFileService(mockFileService)
                .BuildServiceProvider();

            string connection = String.Format(_connectionFormat, templatePath);
            var target = new FileSystem.ReadRepository(serviceProvider, connection);
            var actual = target.GetAllTemplates();

            Assert.Equal(expected, actual.Single(t => t.TemplateType == templateType).Content);
        }

    }
}
