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

namespace PPTail.Data.Forestry.Test
{
    public class Repository_GetSiteSettings_Should
    {
        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 5;
        const String _dataFolder = "Data";

        [Fact]
        public void ThrowSettingNotFoundExceptionIfSettingsCannotBeLoaded()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            String fileContents = "Bogus SiteSettings file";

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(fileContents);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            Assert.Throws<SettingNotFoundException>(() => target.GetSiteSettings());
        }

        [Fact]
        public void ThrowWithProperSettingNameIfSettingsCannotBeLoaded()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            String fileContents = "Bogus SiteSettings file";

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(fileContents);

            String expected = typeof(SiteSettings).Name;

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            try
            {
                var actual = target.GetSiteSettings();
                Assert.False(true, "SettingNotFoundException not thrown");
            }
            catch (SettingNotFoundException ex)
            {
                Assert.Equal(expected, ex.SettingName);
            }
        }

        [Fact]
        public void ReadsTheProperFileFromTheFileSystem()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            String expectedPath = System.IO.Path.Combine(rootPath, _dataFolder, "SiteSettings.md");

            String fileContents = new SettingsFileBuilder().UseRandomValues().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.Is<string>(p => p == expectedPath)))
                .Returns(fileContents).Verifiable();

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            var actual = target.GetSiteSettings();

            fileSystem.VerifyAll();
        }

        [Fact]
        public void ReturnTheProperValueForTitle()
        {
            String expected = string.Empty.GetRandom();
            String xml = new SettingsFileBuilder().UseRandomValues()
                .Title(expected).Build();

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
            var xml = new SettingsFileBuilder()
                .UseRandomValues()
                .RemoveTitle()
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            Assert.Throws<SettingNotFoundException>(() => target.GetSiteSettings());
        }

        [Fact]
        public void ThrowWithTheProperSettingNameIfTitleIsNotSupplied()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}\\";

            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemoveTitle().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            SiteSettings actual;
            String expected = nameof(actual.Title);
            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);
            try
            {
                actual = target.GetSiteSettings();
                Assert.False(true, "SettingNotFoundException not thrown");
            }
            catch (SettingNotFoundException ex)
            {
                Assert.Equal(expected, ex.SettingName);
            }
        }

        [Fact]
        public void ReturnTheProperValueForDescription()
        {
            String expected = string.Empty.GetRandom();
            String xml = new SettingsFileBuilder().UseRandomValues()
                .Description(expected).Build();

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
            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemoveDescription().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.True(string.IsNullOrWhiteSpace(actual.Description));
        }

        [Fact]
        public void ReturnTheProperValueForPostsPerPage()
        {
            Int32 expected = 25.GetRandom(5);
            String xml = new SettingsFileBuilder().UseRandomValues()
                .PostsPerPage(expected).Build();

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
            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemovePostsPerPage().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(_defaultPostsPerPage, actual.PostsPerPage);
        }

        [Fact]
        public void ReturnTheProperValueForPostsPerFeed()
        {
            Int32 expected = 25.GetRandom(5);
            String xml = new SettingsFileBuilder().UseRandomValues()
                .PostsPerFeed(expected).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.PostsPerFeed);
        }

        [Fact]
        public void ReturnTheDefaultValueIfPostsPerFeedIsNotSupplied()
        {
            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemovePostsPerFeed().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(_defaultPostsPerFeed, actual.PostsPerFeed);
        }

        [Fact]
        public void ReturnTheProperValueForTheme()
        {
            String expected = string.Empty.GetRandom(25);
            var xml = new SettingsFileBuilder().UseRandomValues()
                .Theme(expected).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml);

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(expected, actual.Theme);
        }

        [Fact]
        public void ReturnAnEmptyStringIfThemeNodeIsEmpty()
        {
            var xml = new SettingsFileBuilder().UseRandomValues()
                .Theme(string.Empty).Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(string.Empty, actual.Theme);
        }

        [Fact]
        public void ReturnAnEmptyStringIfThemeIsNotSupplied()
        {
            var xml = new SettingsFileBuilder().UseRandomValues()
                .RemoveTheme().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(xml.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(string.Empty, actual.Theme);
        }

        [Fact]
        public void ReadTheSettingsFileOnceEvenIfCalledMultipleTimes()
        {
            String rootPath = $"c:\\{string.Empty.GetRandom()}\\";
            String expectedPath = System.IO.Path.Combine(rootPath, _dataFolder, "SiteSettings.md");

            String fileContents = new SettingsFileBuilder().UseRandomValues().Build();

            var fileSystem = new Mock<IFile>();
            fileSystem
                .Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(fileContents);

            var target = (null as IContentRepository).Create(fileSystem.Object, rootPath);

            target.GetSiteSettings();
            target.GetSiteSettings();
            var actual = target.GetSiteSettings();

            fileSystem
                .Verify(f => f.ReadAllText(It.Is<string>(p => p == expectedPath)), Times.Once);
        }

        [Fact]
        public void ReturnTheCorrectNumberOfSiteVariables()
        {
            var siteVariables = (null as IEnumerable<SiteVariable>)
                .CreateRandom();

            var fileData = new SettingsFileBuilder()
                .UseRandomValues()
                .Variables(siteVariables)
                .RemoveTheme()
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(fileData.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            Assert.Equal(siteVariables.Count(), actual.Variables.Count());
        }

        [Fact]
        public void ReturnTheCorrectSiteVariableNames()
        {
            var siteVariables = (null as IEnumerable<SiteVariable>)
                .CreateRandom();

            var fileData = new SettingsFileBuilder()
                .UseRandomValues()
                .Variables(siteVariables)
                .RemoveTheme()
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(fileData.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            var expectedVariableNames = siteVariables.Select(v => v.Name).AsHash();
            var actualVariableNames = actual.Variables.Select(v => v.Name).AsHash();

            Assert.Equal(expectedVariableNames, actualVariableNames);
        }

        [Fact]
        public void ReturnTheCorrectSiteVariableValues()
        {
            var siteVariables = (null as IEnumerable<SiteVariable>)
                .CreateRandom();

            var fileData = new SettingsFileBuilder()
                .UseRandomValues()
                .Variables(siteVariables)
                .RemoveTheme()
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(fileData.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            var expectedVariableValues = siteVariables.Select(v => v.Value).AsHash();
            var actualVariableValues = actual.Variables.Select(v => v.Value).AsHash();

            Assert.Equal(expectedVariableValues, actualVariableValues);
        }

        [Fact]
        public void ReturnTheCorrectSiteVariable()
        {
            var siteVariables = (null as IEnumerable<SiteVariable>)
                .CreateRandom(1);

            var fileData = new SettingsFileBuilder()
                .UseRandomValues()
                .Variables(siteVariables)
                .RemoveTheme()
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(fileData.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            var expectedVariable = siteVariables.Single();
            var actualVariable = actual.Variables.Single();

            Assert.Equal(expectedVariable.Name, actualVariable.Name);
            Assert.Equal(expectedVariable.Value, actualVariable.Value);
        }

        [Fact]
        public void ReturnTheCorrectSiteVariableEvenIfItContainsAColon()
        {
            var siteVariableName = String.Empty.GetRandom();
            var siteVariableValue = $"{string.Empty.GetRandom()}:{string.Empty.GetRandom()}";
            var siteVariables = new List<SiteVariable>() { new SiteVariable() { Name = siteVariableName, Value = siteVariableValue } };

            var fileData = new SettingsFileBuilder()
                .UseRandomValues()
                .Variables(siteVariables)
                .RemoveTheme()
                .Build();

            var fileSystem = new Mock<IFile>();
            fileSystem.Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(fileData.ToString());

            var target = (null as IContentRepository).Create(fileSystem.Object, "c:\\");
            var actual = target.GetSiteSettings();

            var expectedVariable = siteVariables.Single();
            var actualVariable = actual.Variables.Single();

            Assert.Equal(expectedVariable.Name, actualVariable.Name);
            Assert.Equal(expectedVariable.Value, actualVariable.Value);
        }
    }
}
