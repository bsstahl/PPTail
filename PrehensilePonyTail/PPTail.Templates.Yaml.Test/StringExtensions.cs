using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.Yaml.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class StringExtensions
    {
        const string _connectionFormat = "Provider=PPTail.Templates.Yaml.ReadRepository;FilePath={0}";

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
            var target = new Yaml.ReadRepository(serviceProvider, connection);
            var actual = target.GetAllTemplates();

            mockFileService.Verify();
        }

        internal static void ExecuteTemplateContentTest(this String templateFilename, Enumerations.TemplateType templateType)
        {
            string expected = string.Empty.GetRandom();
            string yamlFile = $"---\r\n\r\n---\r\n{expected}";
            templateFilename.ExecuteTemplateContentTest(templateType, expected, yamlFile);
        }

        internal static void ExecuteTemplateContentTest(this String templateFilename, Enumerations.TemplateType templateType, String expected, String yamlFile)
        {
            string templatePath = string.Empty.GetRandom();
            string expectedPath = System.IO.Path.Combine(templatePath, templateFilename);

            var mockFileService = new Mock<IFile>();
            mockFileService
                .Setup(f => f.ReadAllText(It.IsAny<String>()))
                .Returns(string.Empty.GetRandom());
            mockFileService
                .Setup(f => f.ReadAllText(expectedPath))
                .Returns(yamlFile);

            var serviceProvider = new ServiceCollection()
                .AddFileService(mockFileService)
                .BuildServiceProvider();

            string connection = String.Format(_connectionFormat, templatePath);
            var target = new Yaml.ReadRepository(serviceProvider, connection);
            var actual = target.GetAllTemplates();

            Assert.Equal(expected, actual.Single(t => t.TemplateType == templateType).Content);
        }

    }
}
