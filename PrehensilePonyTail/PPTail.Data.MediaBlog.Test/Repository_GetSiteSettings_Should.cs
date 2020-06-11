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
using PPTail.Builders;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_GetSiteSettings_Should
    {
        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 5;

        [Fact]
        public void ThrowSettingNotFoundExceptionIfSettingsContentIsInvalid()
        {
            String invalidJson = "<xml/>";

            var fileService = new MockFileServiceBuilder()
                .AddSiteSettingsFile(invalidJson)
                .Build();

            var target = new ContentRepositoryBuilder()
                .UseGenericSettings()
                .UseGenericDirectory()
                .AddFileService(fileService.Object)
                .Build();

            Assert.Throws<SettingNotFoundException>(() => target.GetSiteSettings());
        }

        [Fact]
        public void ThrowWithProperSettingNameIfSettingsCannotBeLoaded()
        {
            String expected = typeof(SiteSettings).Name;
            String invalidJson = "<xml/>";

            var fileService = new MockFileServiceBuilder()
                .AddSiteSettingsFile(invalidJson)
                .Build();

            var target = new ContentRepositoryBuilder()
                .UseGenericSettings()
                .UseGenericDirectory()
                .AddFileService(fileService.Object)
                .Build();

            try
            {
                var actual = target.GetSiteSettings();
            }
            catch (SettingNotFoundException ex)
            {
                Assert.Equal(expected, ex.SettingName);
            }
        }

        [Fact]
        public void ReadsTheProperFileFromTheFileSystem()
        {
            String rootPath = string.Empty;
            String expectedPath = System.IO.Path.Combine(rootPath, "SiteSettings.json");

            String json = new SiteSettingsFileBuilder()
                .UseRandomValues()
                .Build();

            var fileSystem = new MockFileServiceBuilder()
                .AddSiteSettingsFile(json)
                .Build();

            var target = new ContentRepositoryBuilder()
                .UseGenericDirectory()
                .UseGenericSettings()
                .AddFileService(fileSystem.Object)
                .Build();

            var actual = target.GetSiteSettings();

            fileSystem.Verify(f => f.ReadAllText(expectedPath));

        }

        [Fact]
        public void ReturnTheProperValueForTitle()
        {
            String expected = string.Empty.GetRandom();
            Func<SiteSettings, string> fieldValueDelegate = s => s.Title;
            var siteSettings = new SiteSettingsBuilder()
                .Title(expected)
                .Build();
            ExecutePropertyTest(siteSettings, expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueForDescription()
        {
            String expected = string.Empty.GetRandom();
            Func<SiteSettings, string> fieldValueDelegate = s => s.Description;
            var siteSettings = new SiteSettingsBuilder()
                .Description(expected)
                .Build();
            ExecutePropertyTest(siteSettings, expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueForPostsPerPage()
        {
            Int32 expectedValue = 25.GetRandom(5);
            String expected = expectedValue.ToString();
            Func<SiteSettings, string> fieldValueDelegate = s => s.PostsPerPage.ToString();
            var siteSettings = new SiteSettingsBuilder()
                .PostsPerPage(expectedValue)
                .Build();
            ExecutePropertyTest(siteSettings, expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheDefaultValueIfPostsPerPageIsNotSupplied()
        {
            Int32 expectedValue = _defaultPostsPerPage;
            String expected = expectedValue.ToString();
            Func<SiteSettings, string> fieldValueDelegate = s => s.PostsPerPage.ToString();
            var siteSettings = new SiteSettingsBuilder()
                .PostsPerPage(0)
                .Build();
            ExecutePropertyTest(siteSettings, expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueForPostsPerFeed()
        {
            Int32 expectedValue = 25.GetRandom(5);
            String expected = expectedValue.ToString();
            Func<SiteSettings, string> fieldValueDelegate = s => s.PostsPerFeed.ToString();
            var siteSettings = new SiteSettingsBuilder()
                .PostsPerFeed(expectedValue)
                .Build();
            ExecutePropertyTest(siteSettings, expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheDefaultValueIfPostsPerFeedIsNotSupplied()
        {
            Int32 expectedValue = _defaultPostsPerFeed;
            String expected = expectedValue.ToString();
            Func<SiteSettings, string> fieldValueDelegate = s => s.PostsPerFeed.ToString();
            var siteSettings = new SiteSettingsBuilder()
                .PostsPerFeed(0)
                .Build();
            ExecutePropertyTest(siteSettings, expected, fieldValueDelegate);
        }

        [Fact]
        public void ReturnTheProperValueForTheme()
        {
            String expected = string.Empty.GetRandom();
            Func<SiteSettings, string> fieldValueDelegate = s => s.Theme;
            var siteSettings = new SiteSettingsBuilder()
                .Theme(expected)
                .Build();
            ExecutePropertyTest(siteSettings, expected, fieldValueDelegate);
        }

        private static void ExecutePropertyTest(SiteSettings siteSettings, String expected, Func<SiteSettings, String> fieldValueDelegate)
        {
            var fileSystem = new MockFileServiceBuilder()
                .AddSiteSettings(siteSettings)
                .Build();

            var target = new ContentRepositoryBuilder()
                .UseGenericDirectory()
                .UseGenericSettings()
                .AddFileService(fileSystem.Object)
                .Build();

            var actual = target.GetSiteSettings();
            Assert.Equal(expected, fieldValueDelegate(actual));
        }

    }
}
