using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PPTail.Interfaces;
using PPTail.Entities;
using PPTail.Extensions;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

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
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.ToLowerInvariant().Contains("posts\\")));
        }

        [Fact]
        public void SetTheFilenameOfTheContentPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(3);
            var contentItem = (null as ContentItem).Create();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, extension);
            var actualPages = target.Build();
            var actualPage = actualPages.Single(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage);

            var expected = $"pages/{contentItem.Slug}.{extension}".ToLowerInvariant();
            Assert.Equal(expected, actualPage.RelativeFilePath.ToLowerInvariant());
        }

        [Fact]
        public void SetTheFilenameOfThePostPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var contentItem = (null as ContentItem).Create();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, extension);
            var actualPages = target.Build();
            var actualPage = actualPages.Single(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage);

            string expectedFileName = $"{contentItem.Slug}.{extension}";
            var expectedFilePath = System.IO.Path.Combine("posts", expectedFileName);
            Assert.Equal(expectedFilePath, actualPage.RelativeFilePath.ToLowerInvariant());
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
            container.AddSingleton<ISettings>(settings);
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
            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPosts()).Returns(contentItems);
            var archiveProvider = Mock.Of<IArchiveProvider>();

            var pageGen = new Mock<IPageGenerator>();
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var target = (null as Builder).Create(contentRepo.Object, pageGen.Object);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.GeneratePostPage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SiteSettings>(), item), Times.Once);
            }
        }

        [Fact]
        public void GenerateASlugForAPostWithoutAPreviouslyGeneratedSlug()
        {
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(1);
            var post = contentItems.Single();
            post.Slug = string.Empty;
            post.IsPublished = true;

            contentRepo.Setup(c => c.GetAllPosts()).Returns(contentItems);
            var archiveProvider = Mock.Of<IArchiveProvider>();

            var pageGen = new Mock<IPageGenerator>();

            var target = (null as Builder).Create(contentRepo.Object, pageGen.Object);
            var actual = target.Build();

            var postPage = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage).Single();
            Assert.Contains(post.Title, postPage.RelativeFilePath);
        }

        [Fact]
        public void GenerateASlugForAPageWithoutAPreviouslyGeneratedSlug()
        {
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(1);
            var page = contentItems.Single();
            page.Slug = string.Empty;
            page.IsPublished = true;

            contentRepo.Setup(c => c.GetAllPages()).Returns(contentItems);
            var archiveProvider = Mock.Of<IArchiveProvider>();

            var pageGen = new Mock<IPageGenerator>();

            var target = (null as Builder).Create(contentRepo.Object, pageGen.Object);
            var actual = target.Build();

            var contentPage = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage).Single();
            Assert.Contains(page.Title, contentPage.RelativeFilePath);
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
            container.AddSingleton<ISettings>(settings);
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
            container.AddSingleton<ISettings>(settings);
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
            container.AddSingleton<ISettings>(settings);
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
            container.AddSingleton<ISettings>(settings);
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
            container.AddSingleton<ISettings>(settings);
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
            container.AddSingleton<ISettings>(settings);
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
            container.AddSingleton<ISettings>(settings);
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
            container.AddSingleton<ISettings>(settings);
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
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<SiteSettings>(siteSettings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(f => f.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

        [Fact]
        public void ReturnOneSearchPageForEachTag()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var tags = posts.SelectMany(p => p.Tags).Distinct();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var searchProvider = new Mock<ISearchProvider>();

            var target = (null as Builder).Create(contentRepo.Object, searchProvider.Object);
            var actual = target.Build();

            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(tag, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void ReturnOneSearchPageForEachCategory()
        {
            var categories = (null as IEnumerable<Category>).Create(5);
            int postCount = 25.GetRandom(10);

            var posts = (null as IEnumerable<ContentItem>).Create(categories, postCount);

            var usedCategoryIds = posts.SelectMany(p => p.CategoryIds).Distinct();
            var usedCategories = categories.Where(c => usedCategoryIds.Contains(c.Id));

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var searchProvider = new Mock<ISearchProvider>();

            var target = (null as Builder).Create(contentRepo.Object, searchProvider.Object, categories);
            var actual = target.Build();

            foreach (var category in usedCategories)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(category.Name, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void ReturnOneSearchPageEvenIfNameIsUsedAsATagAndACategory()
        {
            string overlappingName = string.Empty.GetRandom();
            string otherTag = string.Empty.GetRandom();
            var targetCategory = (null as Category).Create(Guid.NewGuid(), overlappingName, "target_category");
            var otherCategory = (null as Category).Create();
            var allCategories = new List<Category>() { targetCategory, otherCategory };

            var post1 = (null as ContentItem).Create(targetCategory.Id, new List<string>() { otherTag });
            var post2 = (null as ContentItem).Create(otherCategory.Id, new List<string>() { overlappingName });

            var posts = new List<ContentItem>() { post1, post2 };
            var tags = posts.SelectMany(p => p.Tags).Distinct();
            var usedCategoryIds = posts.SelectMany(p => p.CategoryIds).Distinct();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var searchProvider = new Mock<ISearchProvider>();

            var target = (null as Builder).Create(contentRepo.Object, searchProvider.Object, allCategories);
            var actual = target.Build();

            searchProvider.Setup(s => s.GenerateSearchResultsPage(otherTag, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            searchProvider.Setup(s => s.GenerateSearchResultsPage(otherCategory.Name, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            searchProvider.Verify(s => s.GenerateSearchResultsPage(overlappingName, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SupplyTheFullListOfPostsToEachSearchPage()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var tags = posts.SelectMany(p => p.Tags).Distinct();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var searchProvider = new Mock<ISearchProvider>();

            var target = (null as Builder).Create(contentRepo.Object, searchProvider.Object);
            var actual = target.Build();

            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(It.IsAny<string>(), posts, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void SupplyTheNavigationContentToEachSearchPage()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var tags = posts.SelectMany(p => p.Tags).Distinct();
            var navContent = string.Empty.GetRandom();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var searchProvider = new Mock<ISearchProvider>();

            var navigationProvider = new Mock<INavigationProvider>();
            navigationProvider.Setup(n => n.CreateNavigation(It.IsAny<IEnumerable<ContentItem>>(), "../", It.IsAny<string>()))
                .Returns(navContent);

            var target = (null as Builder).Create(contentRepo.Object, searchProvider.Object, navigationProvider.Object);
            var actual = target.Build();

            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), navContent, It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void SupplyTheSidebarContentToEachSearchPage()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var tags = posts.SelectMany(p => p.Tags).Distinct();
            var sidebarContent = string.Empty.GetRandom();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var searchProvider = new Mock<ISearchProvider>();

            var pageGen = new Mock<IPageGenerator>();
            pageGen.Setup(n => n.GenerateSidebarContent(It.IsAny<ISettings>(), It.IsAny<SiteSettings>(),
                    It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<IEnumerable<ContentItem>>(),
                    It.IsAny<IEnumerable<Widget>>(), It.IsAny<string>()))
                .Returns(sidebarContent);

            var target = (null as Builder).Create(contentRepo.Object, searchProvider.Object, pageGen.Object);
            var actual = target.Build();

            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), sidebarContent, It.IsAny<string>()));
        }

        [Fact]
        public void SupplyTheCorrectPathToRootToEachSearchPage()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var tags = posts.SelectMany(p => p.Tags).Distinct();
            var pathToRoot = "../";

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var searchProvider = new Mock<ISearchProvider>();

            var target = (null as Builder).Create(contentRepo.Object, searchProvider.Object);
            var actual = target.Build();

            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), pathToRoot));
        }

        [Fact]
        public void ReturnTheCorrectContentOfTheSearchPage()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var tags = posts.SelectMany(p => p.Tags).Distinct();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var searchProvider = new Mock<ISearchProvider>();
            foreach (var tag in tags)
            {
                searchProvider.Setup(s => s.GenerateSearchResultsPage(tag, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(tag.ToBase64());
            }

            var target = (null as Builder).Create(contentRepo.Object, searchProvider.Object);
            var actual = target.Build();
            var actualPages = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.SearchPage);

            foreach (var tag in tags)
            {
                var tagPage = actualPages.Single(p => p.RelativeFilePath.Contains(tag));
                Assert.Equal(tag, tagPage.Content.FromBase64());
            }
        }

        [Fact]
        public void ReturnTheCorrectRelativeFilePathOfEachSearchPage()
        {
            string tagPart1 = string.Empty.GetRandom(4);
            string tagPart2 = string.Empty.GetRandom(4);
            string testTag = $".{tagPart1} {tagPart2}";

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            posts.Single().Tags = new List<string>() { testTag };

            var tags = posts.SelectMany(p => p.Tags).Distinct();
            string filenameExtension = string.Empty.GetRandom();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var target = (null as Builder).Create(contentRepo.Object, filenameExtension);
            var actual = target.Build();
            var actualPages = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.SearchPage);

            var expected = $"Search/{testTag.CreateSlug()}.{filenameExtension}";
            Assert.Equal(expected, actualPages.Single().RelativeFilePath);
        }

        [Fact]
        public void NotProcessAnEmptyTag()
        {
            string testTag = string.Empty.GetRandom();

            var post1 = (null as ContentItem).Create();
            post1.Tags = new List<string>() { testTag };

            var post2 = (null as ContentItem).Create();
            post2.Tags = new List<string>() { string.Empty };

            var posts = new List<ContentItem>() { post1, post2 };

            var tags = posts.SelectMany(p => p.Tags).Distinct();
            string filenameExtension = string.Empty.GetRandom();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var target = (null as Builder).Create(contentRepo.Object, filenameExtension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.SearchPage));
        }

        [Fact]
        public void ReturnContentMarkedAsUnEncoded()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();
            var tags = posts.SelectMany(p => p.Tags).Distinct();
            string filenameExtension = string.Empty.GetRandom();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var target = (null as Builder).Create(contentRepo.Object, filenameExtension);
            var actual = target.Build();
            var actualPages = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.SearchPage);

            Assert.False(actualPages.Any(p => p.IsBase64Encoded));
        }

        [Fact]
        public void ReturnOneRedirectItemForEachPost()
        {
            var posts = (null as IEnumerable<ContentItem>).Create();

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            string filenameExtension = string.Empty.GetRandom();

            var target = (null as Builder).Create(contentRepo.Object, filenameExtension);
            var actual = target.Build();
            var actualPages = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.Redirect);

            Assert.Equal(posts.Count(), actualPages.Count());
        }

        [Fact]
        public void PassTheCorrectRedirectToUrlForEachPostToTheRedirectProvider()
        {
            string folderName = "Posts";
            string filenameExtension = string.Empty.GetRandom();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var redirectProvider = new Mock<IRedirectProvider>();
            foreach (var post in posts)
            {
                string fileName = $"{post.Slug}.{filenameExtension}";
                string expectedUrl = System.IO.Path.Combine("..", folderName, fileName);
                redirectProvider.Setup(r => r.GenerateRedirect(It.Is<string>(s => s == expectedUrl))).Verifiable();
            }

            var target = (null as Builder).Create(contentRepo.Object, redirectProvider.Object, filenameExtension);
            var actual = target.Build();

            redirectProvider.VerifyAll();
        }

        [Fact]
        public void ReturnRedirectWithTheCorrectFilePath()
        {
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var post = posts.Single();

            string folderName = "Permalinks";
            string filenameExtension = string.Empty.GetRandom();
            string fileName = $"{post.Id.ToString()}.{filenameExtension}";
            string expectedPath = System.IO.Path.Combine(folderName, fileName);

            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);

            var target = (null as Builder).Create(contentRepo.Object, filenameExtension);
            var actual = target.Build();
            var actualPage = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.Redirect).Single();

            Assert.Contains(expectedPath, actualPage.RelativeFilePath);
        }
    }
}
