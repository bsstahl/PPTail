using Microsoft.Extensions.DependencyInjection;
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
    public class LinkProvider_GetUrl_Should
    {
        [Fact]
        public void ReturnJustTheFileNameAndExtensionIfLinkIsFromRootToRoot()
        {
            string fileName = string.Empty.GetRandom();
            string fileExtension = string.Empty.GetRandom(3);
            string relativePath = string.Empty;
            string expected = $"{fileName}.{fileExtension}";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(".", relativePath, fileName, fileExtension);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllSlashesShouldBeForwardSlashes()
        {
            string fileName = string.Empty.GetRandom();
            string fileExtension = string.Empty.GetRandom(3);
            string relativePath = string.Empty.GetRandom();
            string pathToRoot = ".";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(pathToRoot, relativePath, fileName, fileExtension);

            Assert.False(actual.Contains("\\"));
        }

        [Fact]
        public void UseTheDefaultFileExtensionIfNoneIsProvided()
        {
            string fileName = string.Empty.GetRandom();
            string relativePath = string.Empty.GetRandom();
            string pathToRoot = ".";

            ISettings settings = (null as ISettings).Create();
            string fileExtension = settings.OutputFileExtension;

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as ILinkProvider).Create(container);
            var actual = target.GetUrl(pathToRoot, relativePath, fileName);

            string expected = $".{fileExtension}";
            Assert.True(actual.EndsWith(expected));
        }

        [Fact]
        public void UseTheSpecifiedFileExtensionIfOneIsProvided()
        {
            string fileName = string.Empty.GetRandom();
            string fileExtension = string.Empty.GetRandom();
            string relativePath = string.Empty.GetRandom();
            string pathToRoot = ".";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(pathToRoot, relativePath, fileName, fileExtension);

            string expected = $".{fileExtension}";
            Assert.True(actual.EndsWith(expected));
        }

        [Fact]
        public void UseTheSpecifiedFileNameIfOneIsProvided()
        {
            string fileName = string.Empty.GetRandom();
            string fileExtension = string.Empty;
            string relativePath = string.Empty.GetRandom();
            string pathToRoot = ".";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(pathToRoot, relativePath, fileName, fileExtension);

            string expected = $"{fileName}.";
            Assert.True(actual.EndsWith(expected));
        }

        [Fact]
        public void StartWithTheFolderNameIfLinkIsFromRoot()
        {
            string fileName = string.Empty.GetRandom();
            string fileExtension = string.Empty.GetRandom();
            string relativePath = string.Empty.GetRandom();
            string pathToRoot = ".";

            var target = (null as ILinkProvider).Create();
            var actual = target.GetUrl(pathToRoot, relativePath, fileName, fileExtension);

            Assert.True(actual.StartsWith(relativePath));
        }
    }
}
