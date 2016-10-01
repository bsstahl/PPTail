using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PPTail.Interfaces;
using PPTail.Entities;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.SiteGenerator.Test
{
    public class Builder_Build_Should
    {
        const string _additionalFilePathsSettingName = "additionalFilePaths";

        [Fact]
        public void RequestAllPagesFromTheRepository()
        {
            var contentRepo = new Mock<IContentRepository>();
            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();
            contentRepo.Verify(c => c.GetAllPages(), Times.AtLeastOnce());
        }

        [Fact]
        public void RequestAllPostsFromTheRepository()
        {
            var contentRepo = new Mock<IContentRepository>();
            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();
            contentRepo.Verify(c => c.GetAllPosts(), Times.AtLeastOnce());
        }

        [Fact]
        public void ReturnOneItemInFolderPagesIfOnePageIsRetrieved()
        {
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });
            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.ToLowerInvariant().Contains("pages/")));
        }

        [Fact]
        public void ReturnOneItemInFolderPostsIfOnePostIsRetrieved()
        {
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });
            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.ToLowerInvariant().Contains("posts/")));
        }

        [Fact]
        public void SetTheFilenameOfTheContentPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            var expected = $"pages/{contentItem.Slug}.{extension}";
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, extension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.ToLowerInvariant().Contains(expected)));
        }

        [Fact]
        public void SetTheFilenameOfThePostPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            var expected = $"posts/{contentItem.Slug}.{extension}";
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, extension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.ToLowerInvariant().Contains(expected)));
        }

        [Fact]
        public void DontCreateAnyPageFilesIfAllPagesAreUnpublished()
        {
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage));
        }

        [Fact]
        public void OnlyCreateAsManyPageFilesAsThereArePublishedPages()
        {
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var expected = contentItems.Count(ci => ci.IsPublished);

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage));
        }

        [Fact]
        public void DoNotCreateOutputForAnUnpublishedPage()
        {
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true;

            var unpublishedItem = contentItems.GetRandom();
            unpublishedItem.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(ci => ci.RelativeFilePath.Contains(unpublishedItem.Slug)));
        }


        [Fact]
        public void DontCreateAnyPostFilesIfAllPostsAreUnpublished()
        {
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage));
        }

        [Fact]
        public void OnlyCreateAsManyFilesAsThereArePublishedPosts()
        {
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var expected = contentItems.Count(ci => ci.IsPublished);

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage));
        }

        [Fact]
        public void DoNotCreateOutputForAnUnpublishedPost()
        {
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true;

            var unpublishedItem = contentItems.GetRandom();
            unpublishedItem.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(ci => ci.RelativeFilePath.Contains(unpublishedItem.Slug)));
        }

        [Fact]
        public void CreateAnOutputForBootstrap()
        {
            var target = (null as Builder).Create();
            var actual = target.Build();
            Assert.Equal(1, actual.Count(ci => ci.SourceTemplateType == Enumerations.TemplateType.Bootstrap));
        }

        [Fact]
        public void CreateAnOutputForTheArchive()
        {
            var target = (null as Builder).Create();
            var actual = target.Build();
            Assert.Equal(1, actual.Count(ci => ci.SourceTemplateType == Enumerations.TemplateType.Archive));
        }

        [Fact]
        public void CreateAnOutputForTheContactPage()
        {
            var target = (null as Builder).Create();
            var actual = target.Build();
            Assert.Equal(1, actual.Count(ci => ci.SourceTemplateType == Enumerations.TemplateType.ContactPage));
        }

        [Fact]
        public void CallThePageGeneratorExactlyOnceWithEachPublishedPage()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPages()).Returns(contentItems);
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();

            var pageGen = new Mock<IPageGenerator>();
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var settings = new Settings();
            var navProvider = Mock.Of<INavigationProvider>();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var siteSettings = (null as SiteSettings).Create();
            var target = (null as Builder).Create(container);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.GenerateContentPage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SiteSettings>(), item), Times.Once);
            }
        }

        [Fact]
        public void CallThePageGeneratorExactlyOnceWithEachPublishedPost()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPosts()).Returns(contentItems);
            var archiveProvider = Mock.Of<IArchiveProvider>();

            var pageGen = new Mock<IPageGenerator>();
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var settings = new Settings();
            var navProvider = Mock.Of<INavigationProvider>();
            var contactProvider = Mock.Of<IContactProvider>();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var siteSettings = (null as SiteSettings).Create();

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.GeneratePostPage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SiteSettings>(), item), Times.Once);
            }
        }

        [Fact]
        public void CallGenerateStylesheetEactlyOnce()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = new Mock<IPageGenerator>();
            var settings = new Settings();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var siteSettings = (null as SiteSettings).Create();
            var target = (null as Builder).Create(container);
            var actual = target.Build();

            pageGen.Verify(c => c.GenerateStylesheet(It.IsAny<SiteSettings>()), Times.Once);
        }

        [Fact]
        public void CallGenerateBootstrapFileEactlyOnce()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = new Mock<IPageGenerator>();
            var settings = new Settings();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var siteSettings = (null as SiteSettings).Create();
            var target = (null as Builder).Create(container);
            var actual = target.Build();

            pageGen.Verify(c => c.GenerateBootstrapPage(), Times.Once);
        }

        [Fact]
        public void CallGenerateHomepageEactlyOnce()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = new Mock<IPageGenerator>();
            var settings = new Settings();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var siteSettings = (null as SiteSettings).Create();
            var target = (null as Builder).Create(container);
            var actual = target.Build();

            pageGen.Verify(c => c.GenerateHomepage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SiteSettings>(), It.IsAny<IEnumerable<ContentItem>>()), Times.Once);
        }

        [Fact]
        public void CreateOneRawSiteFileForEachSourceFile()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = Mock.Of<IPageGenerator>();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();
            var siteSettings = Mock.Of<SiteSettings>();

            var settings = new Settings();
            string additionalPathSettingsValue = $"{string.Empty.GetRandom()},{string.Empty.GetRandom()},{string.Empty.GetRandom()}";
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalPathSettingsValue);

            var additionalPaths = additionalPathSettingsValue.Split(',');
            int expected = 0;
            foreach (var additionalPath in additionalPaths)
            {
                int count = 10.GetRandom(3);
                var additionalFiles = (null as IEnumerable<SourceFile>).Create(count);
                expected += count;
                contentRepo.Setup(r => r.GetFolderContents(additionalPath)).Returns(additionalFiles);
            }

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<SiteSettings>(siteSettings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count(a => a.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

        [Fact]
        public void ProperlyBase64EncodeAllFileContents()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = Mock.Of<IPageGenerator>();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();
            var siteSettings = Mock.Of<SiteSettings>();

            var settings = new Settings();
            string additionalPathSettingsValue = $"{string.Empty.GetRandom()},{string.Empty.GetRandom()},{string.Empty.GetRandom()}";
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalPathSettingsValue);

            var additionalPaths = additionalPathSettingsValue.Split(',');
            var sourceFiles = new List<SourceFile>();
            foreach (var additionalPath in additionalPaths)
            {
                var additionalFiles = (null as IEnumerable<SourceFile>).Create();
                sourceFiles.AddRange(additionalFiles);
                contentRepo.Setup(r => r.GetFolderContents(additionalPath)).Returns(additionalFiles);
            }

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<SiteSettings>(siteSettings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            foreach (var sourceFile in sourceFiles)
            {
                var filePath = System.IO.Path.Combine(sourceFile.RelativePath, sourceFile.FileName);
                var expected = Convert.ToBase64String(sourceFile.Contents);
                Assert.Equal(expected, actual.Single(f => f.RelativeFilePath == filePath).Content);
            }
        }

        [Fact]
        public void ProperlyMarkAllRawFilesAsBase64Encoded()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = Mock.Of<IPageGenerator>();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();
            var siteSettings = Mock.Of<SiteSettings>();

            var settings = new Settings();
            string additionalPathSettingsValue = $"{string.Empty.GetRandom()},{string.Empty.GetRandom()},{string.Empty.GetRandom()}";
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalPathSettingsValue);

            var additionalPaths = additionalPathSettingsValue.Split(',');
            int expected = 0;
            foreach (var additionalPath in additionalPaths)
            {
                int count = 10.GetRandom(3);
                var additionalFiles = (null as IEnumerable<SourceFile>).Create(count);
                expected += count;
                contentRepo.Setup(r => r.GetFolderContents(additionalPath)).Returns(additionalFiles);
            }

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<SiteSettings>(siteSettings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count(a => a.IsBase64Encoded));
        }

        [Fact]
        public void ReturnNoRawFilesIfTheSettingDoesNotExist()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = Mock.Of<IPageGenerator>();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();
            var siteSettings = Mock.Of<SiteSettings>();

            var settings = new Settings();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<SiteSettings>(siteSettings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(f => f.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

        [Fact]
        public void ReturnNoRawFilesIfTheSettingIsEmpty()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = Mock.Of<IPageGenerator>();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();
            var siteSettings = Mock.Of<SiteSettings>();

            var settings = new Settings();
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, string.Empty);

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<SiteSettings>(siteSettings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(f => f.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

        [Fact]
        public void ReturnNoRawFilesIfTheSettingIsNull()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = Mock.Of<IPageGenerator>();
            var navProvider = Mock.Of<INavigationProvider>();
            var archiveProvider = Mock.Of<IArchiveProvider>();
            var contactProvider = Mock.Of<IContactProvider>();
            var siteSettings = Mock.Of<SiteSettings>();

            var settings = new Settings();
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, null);

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<SiteSettings>(siteSettings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(f => f.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

    }
}
