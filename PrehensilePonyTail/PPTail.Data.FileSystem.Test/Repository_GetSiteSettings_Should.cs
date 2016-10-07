using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using PPTail.Entities;
using Moq;
using PPTail.Interfaces;
using System.Xml.Linq;
using PPTail.Exceptions;

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_GetSiteSettings_Should
    {
        const int _defaultPostsPerPage = 3;
        const string _dataFolder = "App_Data";

        [Fact]
        public void ThrowSettingNotFoundExceptionIfSettingsCannotBeLoaded()
        {
            string rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            string xml = "<badXml></bad>";

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            Assert.Throws(typeof(Exceptions.SettingNotFoundException), () => target.GetSiteSettings());
        }

        [Fact]
        public void ReadsTheProperFileFromTheFileSystem()
        {
            string rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            string expectedPath = System.IO.Path.Combine(rootPath, _dataFolder, "settings.xml");
            string xml = (null as SiteSettings).BuildXml(string.Empty.GetRandom(), string.Empty.GetRandom(), 0);

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.Is<string>(p => p == expectedPath)))
                .Returns(xml).Verifiable();

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetSiteSettings();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void ReturnTheProperValueForTitle()
        {
            string expected = string.Empty.GetRandom();
            string xml = (null as SiteSettings).BuildXml(expected, string.Empty, 0);

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.Title);
        }

        [Fact]
        public void ThrowSettingNotFoundExceptionIfTitleIsNotSupplied()
        {
            var xml = XElement.Parse((null as SiteSettings).BuildXml(string.Empty.GetRandom(), string.Empty.GetRandom(), 10.GetRandom(2)));
            xml.RemoveDescendants("name");

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            Assert.Throws<SettingNotFoundException>(() => target.GetSiteSettings());
        }

        [Fact]
        public void ReturnTheProperValueForDescription()
        {
            string expected = string.Empty.GetRandom();
            string xml = (null as SiteSettings).BuildXml(string.Empty.GetRandom(), expected, 0);

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.Description);
        }

        [Fact]
        public void ReturnAnEmptyStringIfDescriptionIsNotSupplied()
        {
            var xml = XElement.Parse((null as SiteSettings).BuildXml(string.Empty.GetRandom(), string.Empty.GetRandom(), 10.GetRandom(2)));
            xml.RemoveDescendants("description");

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.True(string.IsNullOrWhiteSpace(actual.Description));
        }

        [Fact]
        public void ReturnTheProperValueForPostsPerPage()
        {
            int expected = 25.GetRandom(5);
            string xml = (null as SiteSettings).BuildXml(string.Empty.GetRandom(), string.Empty, expected);

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.PostsPerPage);
        }

        [Fact]
        public void ReturnTheDefaultValueIfPostsPerPageIsNotSupplied()
        {
            var xml = XElement.Parse((null as SiteSettings).BuildXml(string.Empty.GetRandom(), string.Empty.GetRandom(), 10.GetRandom(2)));
            xml.RemoveDescendants("postsperpage");

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(_defaultPostsPerPage, actual.PostsPerPage);
        }
    }
}
