using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.Links.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class LinkProvider_GetUrl_Should
    {
        [Fact]
        public void ReturnJustTheFileNameAndExtensionIfLinkIsFromRootToRoot()
        {
            String fileName = string.Empty.GetRandom();
            String fileExtension = string.Empty.GetRandom(3);
            String relativePath = string.Empty;
            String expected = $"{fileName}.{fileExtension}";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(".", relativePath, fileName, fileExtension);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllSlashesShouldBeForwardSlashes()
        {
            String fileName = string.Empty.GetRandom();
            String fileExtension = string.Empty.GetRandom(3);
            String relativePath = string.Empty.GetRandom();
            String pathToRoot = ".";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(pathToRoot, relativePath, fileName, fileExtension);

            Assert.DoesNotContain("\\", actual);
        }

        [Fact]
        public void UseTheDefaultFileExtensionIfNoneIsProvided()
        {
            String fileName = string.Empty.GetRandom();
            String relativePath = string.Empty.GetRandom();
            String pathToRoot = ".";
            String fileExtension = string.Empty.GetRandom();

            SiteSettings siteSettings = new SiteSettings()
            {
                OutputFileExtension = fileExtension
            };

            var container = new ServiceCollection();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo
                .Setup(r => r.GetSiteSettings())
                .Returns(siteSettings);
            container.AddSingleton<IContentRepository>(contentRepo.Object);

            var target = (null as ILinkProvider).Create(container);
            var actual = target.GetUrl(pathToRoot, relativePath, fileName);

            String expected = $".{fileExtension}";
            Assert.EndsWith(expected, actual);
        }

        [Fact]
        public void UseTheSpecifiedFileExtensionIfOneIsProvided()
        {
            String fileName = string.Empty.GetRandom();
            String fileExtension = string.Empty.GetRandom();
            String relativePath = string.Empty.GetRandom();
            String pathToRoot = ".";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(pathToRoot, relativePath, fileName, fileExtension);

            String expected = $".{fileExtension}";
            Assert.EndsWith(expected, actual);
        }

        [Fact]
        public void UseTheSpecifiedFileNameIfOneIsProvided()
        {
            String fileName = string.Empty.GetRandom();
            String fileExtension = string.Empty;
            String relativePath = string.Empty.GetRandom();
            String pathToRoot = ".";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(pathToRoot, relativePath, fileName, fileExtension);

            String expected = $"{fileName}.";
            Assert.EndsWith(expected, actual);
        }

        [Fact]
        public void StartWithTheFolderNameIfLinkIsFromRoot()
        {
            String fileName = string.Empty.GetRandom();
            String fileExtension = string.Empty.GetRandom();
            String relativePath = string.Empty.GetRandom();
            String pathToRoot = ".";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(pathToRoot, relativePath, fileName, fileExtension);

            Assert.StartsWith(relativePath, actual);
        }
    }
}
