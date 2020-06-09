using Moq;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Templates.FileSystem.Test
{
    public static class MockFileServiceExtensions
    {
        public static void ConfigureTemplate(this Mock<IFile> mockFileService, string templatePath, string templateFilename)
        {
            var expectedPath = System.IO.Path.Combine(templatePath, templateFilename);
            mockFileService
                .Setup(f => f.ReadAllText(expectedPath))
                .Returns(string.Empty.GetRandom());
        }

        public static void VerifyOnce(this Mock<IFile> mockFileService, string templatePath, string templateFilename)
        {
            var expectedPath = System.IO.Path.Combine(templatePath, templateFilename);
            mockFileService.Verify(f => f.ReadAllText(expectedPath), Times.Once);
        }
    }
}
