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
        const string _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";
        const string _createDasBlogPostsCompatibilityFileSettingName = "createDasBlogPostsCompatibilityFile";

        [Fact]
        public void UseTheProperContentRepositoryInstance()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IContentRepository>();

            var contentRepo1 = new Mock<IContentRepository>();
            container.AddSingleton<IContentRepository>(contentRepo1.Object);

            var contentRepo2 = new FakeContentRepository();
            container.AddSingleton<IContentRepository>(contentRepo2);

            var settings = (null as Settings).Create();
            settings.SourceConnection = contentRepo1.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actual = target.Build();
            contentRepo1.Verify(c => c.GetAllPages(), Times.AtLeastOnce());
        }


        [Fact]
        public void RequestAllPagesFromTheRepository()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();
            contentRepo.Verify(c => c.GetAllPages(), Times.AtLeastOnce());
        }

        [Fact]
        public void RequestAllPostsFromTheRepository()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();
            contentRepo.Verify(c => c.GetAllPosts(), Times.AtLeastOnce());
        }

        [Fact]
        public void ReturnOneItemInFolderPagesIfOnePageIsRetrieved()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });
            var target = (null as Builder).Create(container);
            var actual = target.Build();
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.ToLowerInvariant().Contains("pages/")));
        }

        [Fact]
        public void ReturnOneItemInFolderPostsIfOnePostIsRetrieved()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });
            var target = (null as Builder).Create(container);
            var actual = target.Build();
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.ToLowerInvariant().Contains("posts\\")));
        }

        [Fact]
        public void SetTheFilenameOfTheContentPageToTheSlugPlusTheExtension()
        {
            var container = (null as IServiceCollection).Create();

            var contentItem = (null as ContentItem).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            string extension = string.Empty.GetRandom(3);
            var settings = (null as Settings).Create(extension);
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actualPages = target.Build();
            var actualPage = actualPages.Single(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage);

            var expected = $"pages/{contentItem.Slug}.{extension}".ToLowerInvariant();
            Assert.Equal(expected, actualPage.RelativeFilePath.ToLowerInvariant());
        }

        [Fact]
        public void SetTheFilenameOfThePostPageToTheSlugPlusTheExtension()
        {
            var container = (null as IServiceCollection).Create();

            var contentItem = (null as ContentItem).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            string extension = string.Empty.GetRandom(4);
            var settings = (null as ISettings).Create(extension);
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actualPages = target.Build();
            var actualPage = actualPages.Single(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage);

            string expectedFileName = $"{contentItem.Slug}.{extension}";
            var expectedFilePath = System.IO.Path.Combine("posts", expectedFileName);
            Assert.Equal(expectedFilePath, actualPage.RelativeFilePath.ToLowerInvariant());
        }

        [Fact]
        public void DontCreateAnyPageFilesIfAllPagesAreUnpublished()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = false;
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage));
        }

        [Fact]
        public void OnlyCreateAsManyPageFilesAsThereArePublishedPages()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var expected = contentItems.Count(ci => ci.IsPublished);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage));
        }

        [Fact]
        public void DoNotCreateOutputForAnUnpublishedPage()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true;
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var unpublishedItem = contentItems.GetRandom();
            unpublishedItem.IsPublished = false;

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(ci => ci.RelativeFilePath.Contains(unpublishedItem.Slug)));
        }


        [Fact]
        public void DontCreateAnyPostFilesIfAllPostsAreUnpublished()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = false;
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage));
        }

        [Fact]
        public void OnlyCreateAsManyFilesAsThereArePublishedPosts()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var expected = contentItems.Count(ci => ci.IsPublished);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage));
        }

        [Fact]
        public void DoNotCreateOutputForAnUnpublishedPost()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true;
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var unpublishedItem = contentItems.GetRandom();
            unpublishedItem.IsPublished = false;

            var target = (null as Builder).Create(container);
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
            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPages()).Returns(contentItems);

            var container = (null as IServiceCollection).Create(contentRepo.Object);

            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var pageGen = new Mock<IContentItemPageGenerator>();
            container.ReplaceDependency<IContentItemPageGenerator>(pageGen.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.Generate(It.IsAny<string>(), It.IsAny<string>(), item, It.IsAny<Enumerations.TemplateType>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            }
        }

        [Fact]
        public void CallThePageGeneratorExactlyOnceWithEachPublishedPost()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPosts()).Returns(contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var pageGen = new Mock<IContentItemPageGenerator>();
            container.ReplaceDependency<IContentItemPageGenerator>(pageGen.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.Generate(It.IsAny<string>(), It.IsAny<string>(), item, It.IsAny<Enumerations.TemplateType>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            }
        }

        [Fact]
        public void GenerateASlugForAPostWithoutAPreviouslyGeneratedSlug()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(1);
            var post = contentItems.Single();
            post.Slug = string.Empty;
            post.IsPublished = true;
            contentRepo.Setup(c => c.GetAllPosts()).Returns(contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var postPage = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.PostPage).Single();
            Assert.Contains(post.Title, postPage.RelativeFilePath);
        }

        [Fact]
        public void GenerateASlugForAPageWithoutAPreviouslyGeneratedSlug()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(1);
            var page = contentItems.Single();
            page.Slug = string.Empty;
            page.IsPublished = true;
            contentRepo.Setup(c => c.GetAllPages()).Returns(contentItems);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var contentPage = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.ContentPage).Single();
            Assert.Contains(page.Title, contentPage.RelativeFilePath);
        }

        [Fact]
        public void CallGenerateStylesheetEactlyOnce()
        {
            var container = (null as IServiceCollection).Create();

            var pageGen = new Mock<IPageGenerator>();
            container.ReplaceDependency<IPageGenerator>(pageGen.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            pageGen.Verify(c => c.GenerateStylesheet(), Times.Once);
        }

        [Fact]
        public void CallGenerateBootstrapFileEactlyOnce()
        {
            var container = (null as IServiceCollection).Create();

            var pageGen = new Mock<IPageGenerator>();
            container.ReplaceDependency<IPageGenerator>(pageGen.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            pageGen.Verify(c => c.GenerateBootstrapPage(), Times.Once);
        }

        [Fact]
        public void CallGenerateHomepageEactlyOnce()
        {
            var container = (null as IServiceCollection).Create();

            var pageGen = new Mock<IHomePageGenerator>();
            container.ReplaceDependency<IHomePageGenerator>(pageGen.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            pageGen.Verify(c => c.GenerateHomepage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>()), Times.Once);
        }

        [Fact]
        public void CreateOneRawSiteFileForEachSourceFile()
        {
            var contentRepo = new Mock<IContentRepository>();
            var container = (null as IServiceCollection).Create(contentRepo.Object);

            var settings = new Settings() { SourceConnection = contentRepo.Object.GetSourceConnection() };
            string additionalPathSettingsValue = $"{string.Empty.GetRandom()},{string.Empty.GetRandom()},{string.Empty.GetRandom()}";
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalPathSettingsValue);
            container.ReplaceDependency<ISettings>(settings);

            var additionalPaths = additionalPathSettingsValue.Split(',');
            int expected = 0;
            foreach (var additionalPath in additionalPaths)
            {
                int count = 10.GetRandom(3);
                var additionalFiles = (null as IEnumerable<SourceFile>).Create(count);
                expected += count;
                contentRepo.Setup(r => r.GetFolderContents(additionalPath)).Returns(additionalFiles);
            }

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count(a => a.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

        [Fact]
        public void ProperlyBase64EncodeAllFileContents()
        {
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = new Settings();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            string additionalPathSettingsValue = $"{string.Empty.GetRandom()},{string.Empty.GetRandom()},{string.Empty.GetRandom()}";
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalPathSettingsValue);
            container.ReplaceDependency<ISettings>(settings);

            var additionalPaths = additionalPathSettingsValue.Split(',');
            var sourceFiles = new List<SourceFile>();
            foreach (var additionalPath in additionalPaths)
            {
                var additionalFiles = (null as IEnumerable<SourceFile>).Create();
                sourceFiles.AddRange(additionalFiles);
                contentRepo.Setup(r => r.GetFolderContents(additionalPath)).Returns(additionalFiles);
            }

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
            var container = (null as IServiceCollection).Create();

            var contentRepo = new Mock<IContentRepository>();
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = new Settings();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            string additionalPathSettingsValue = $"{string.Empty.GetRandom()},{string.Empty.GetRandom()},{string.Empty.GetRandom()}";
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalPathSettingsValue);
            container.ReplaceDependency<ISettings>(settings);

            var additionalPaths = additionalPathSettingsValue.Split(',');
            int expected = 0;
            foreach (var additionalPath in additionalPaths)
            {
                int count = 10.GetRandom(3);
                var additionalFiles = (null as IEnumerable<SourceFile>).Create(count);
                expected += count;
                contentRepo.Setup(r => r.GetFolderContents(additionalPath)).Returns(additionalFiles);
            }

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count(a => a.IsBase64Encoded));
        }

        [Fact]
        public void ReturnNoRawFilesIfTheSettingDoesNotExist()
        {
            var container = (null as IServiceCollection).Create();

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(f => f.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

        [Fact]
        public void ReturnNoRawFilesIfTheSettingIsEmpty()
        {
            var container = (null as IServiceCollection).Create();

            var settings = new Settings();
            settings.SourceConnection = container.BuildServiceProvider().GetService<IContentRepository>().GetSourceConnection();
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, string.Empty);
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(f => f.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

        [Fact]
        public void ReturnNoRawFilesIfTheSettingIsNull()
        {
            var container = (null as IServiceCollection).Create();

            var settings = new Settings();
            settings.SourceConnection = container.BuildServiceProvider().GetService<IContentRepository>().GetSourceConnection();
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, null);
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(f => f.SourceTemplateType == Enumerations.TemplateType.Raw));
        }

        [Fact]
        public void ReturnOneSearchPageForEachTag()
        {
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var tags = posts.SelectMany(p => p.Tags).Distinct();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var searchProvider = new Mock<ISearchProvider>();
            container.ReplaceDependency<ISearchProvider>(searchProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(tag, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void ReturnOneSearchPageForEachCategory()
        {
            var container = (null as IServiceCollection).Create();

            var categories = (null as IEnumerable<Category>).Create(5);
            container.ReplaceDependency<IEnumerable<Category>>(categories);

            int postCount = 25.GetRandom(10);
            var posts = (null as IEnumerable<ContentItem>).Create(categories, postCount);
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var searchProvider = new Mock<ISearchProvider>();
            container.ReplaceDependency<ISearchProvider>(searchProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var usedCategoryIds = posts.SelectMany(p => p.CategoryIds).Distinct();
            var usedCategories = categories.Where(c => usedCategoryIds.Contains(c.Id));
            foreach (var category in usedCategories)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(category.Name, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void ReturnOneSearchPageEvenIfNameIsUsedAsATagAndACategory()
        {
            var container = (null as IServiceCollection).Create();

            string overlappingName = string.Empty.GetRandom();
            string otherTag = string.Empty.GetRandom();
            var targetCategory = (null as Category).Create(Guid.NewGuid(), overlappingName, "target_category");
            var otherCategory = (null as Category).Create();
            var allCategories = new List<Category>() { targetCategory, otherCategory };
            container.ReplaceDependency<IEnumerable<Category>>(allCategories);

            var post1 = (null as ContentItem).Create(targetCategory.Id, new List<string>() { otherTag });
            var post2 = (null as ContentItem).Create(otherCategory.Id, new List<string>() { overlappingName });
            var posts = new List<ContentItem>() { post1, post2 };
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var searchProvider = new Mock<ISearchProvider>();
            container.ReplaceDependency<ISearchProvider>(searchProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            searchProvider.Setup(s => s.GenerateSearchResultsPage(otherTag, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            searchProvider.Setup(s => s.GenerateSearchResultsPage(otherCategory.Name, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            searchProvider.Verify(s => s.GenerateSearchResultsPage(overlappingName, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SupplyTheFullListOfPostsToEachSearchPage()
        {
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var searchProvider = new Mock<ISearchProvider>();
            container.ReplaceDependency<ISearchProvider>(searchProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var tags = posts.SelectMany(p => p.Tags).Distinct();
            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(It.IsAny<string>(), posts, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void SupplyTheNavigationContentToEachSearchPage()
        {
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var searchProvider = new Mock<ISearchProvider>();
            container.ReplaceDependency<ISearchProvider>(searchProvider.Object);

            var navContent = string.Empty.GetRandom();
            var navigationProvider = new Mock<INavigationProvider>();
            navigationProvider.Setup(n => n.CreateNavigation(It.IsAny<IEnumerable<ContentItem>>(), "../", It.IsAny<string>()))
                .Returns(navContent);
            container.ReplaceDependency<INavigationProvider>(navigationProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var tags = posts.SelectMany(p => p.Tags).Distinct();
            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), navContent, It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void SupplyTheSidebarContentToEachSearchPage()
        {
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var searchProvider = new Mock<ISearchProvider>();
            container.ReplaceDependency<ISearchProvider>(searchProvider.Object);

            var sidebarContent = string.Empty.GetRandom();
            var pageGen = new Mock<IPageGenerator>();
            pageGen.Setup(n => n.GenerateSidebarContent(It.IsAny<IEnumerable<ContentItem>>(), 
                It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<IEnumerable<Widget>>(), 
                It.IsAny<string>()))
                .Returns(sidebarContent);
            container.ReplaceDependency<IPageGenerator>(pageGen.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var tags = posts.SelectMany(p => p.Tags).Distinct();
            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), sidebarContent, It.IsAny<string>()));
        }

        [Fact]
        public void SupplyTheCorrectPathToRootToEachSearchPage()
        {
            string expectedPathToRoot = "../";

            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var searchProvider = new Mock<ISearchProvider>();
            container.ReplaceDependency<ISearchProvider>(searchProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var tags = posts.SelectMany(p => p.Tags).Distinct();
            foreach (var tag in tags)
                searchProvider.Verify(s => s.GenerateSearchResultsPage(It.IsAny<string>(), It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<string>(), It.IsAny<string>(), expectedPathToRoot));
        }

        [Fact]
        public void ReturnTheCorrectContentOfTheSearchPage()
        {
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var searchProvider = new Mock<ISearchProvider>();
            container.ReplaceDependency<ISearchProvider>(searchProvider.Object);

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var tags = posts.SelectMany(p => p.Tags).Distinct();
            foreach (var tag in tags)
            {
                searchProvider.Setup(s => s.GenerateSearchResultsPage(tag, It.IsAny<IEnumerable<ContentItem>>(), It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(tag.ToBase64());
            }

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var actualPages = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.SearchPage);
            foreach (var tag in tags)
            {
                var tagPage = actualPages.Single(p => p.RelativeFilePath.Contains(tag));
                Assert.Equal(tag, tagPage.Content.FromBase64());
            }
        }

        [Fact]
        public void NotProcessAnEmptyTag()
        {
            var container = (null as IServiceCollection).Create();

            string testTag = string.Empty.GetRandom();
            var post1 = (null as ContentItem).Create();
            post1.Tags = new List<string>() { testTag };
            var post2 = (null as ContentItem).Create();
            post2.Tags = new List<string>() { string.Empty };
            var posts = new List<ContentItem>() { post1, post2 };
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            string filenameExtension = string.Empty.GetRandom();
            var settings = (null as ISettings).Create(filenameExtension);
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.SearchPage));
        }

        [Fact]
        public void ReturnContentMarkedAsUnEncoded()
        {
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            string filenameExtension = string.Empty.GetRandom();
            var settings = (null as ISettings).Create(filenameExtension);
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var actualPages = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.SearchPage);
            Assert.DoesNotContain(actualPages, p => p.IsBase64Encoded);
        }

        [Fact]
        public void ReturnOneRedirectItemForEachPost()
        {
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            string filenameExtension = string.Empty.GetRandom();
            var settings = (null as ISettings).Create(filenameExtension);
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var actualPages = actual.Where(p => p.SourceTemplateType == Enumerations.TemplateType.Redirect);
            Assert.Equal(posts.Count(), actualPages.Count());
        }

        [Fact]
        public void PassTheUrlToTheCorrectPageForEachPostToTheRedirectProvider()
        {
            string filenameExtension = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create(filenameExtension);
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var redirectProvider = new Mock<IRedirectProvider>();
            foreach (var post in posts)
            {
                string fileName = $"{post.Slug}.{filenameExtension}";
                redirectProvider
                    .Setup(r => r.GenerateRedirect(
                    It.Is<string>(s => s.EndsWith(fileName))
                    )).Verifiable();
            }
            container.ReplaceDependency<IRedirectProvider>(redirectProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            redirectProvider.VerifyAll();
        }

        [Fact]
        public void PassTheUrlToTheCorrectPathForEachPostToTheRedirectProvider()
        {
            string folderName = "Posts";
            string filenameExtension = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create(filenameExtension);
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var redirectProvider = new Mock<IRedirectProvider>();
            foreach (var post in posts)
            {
                string fileName = $"{post.Slug}.{filenameExtension}";
                string fullPath = $"/{folderName}/{fileName}";
                redirectProvider
                    .Setup(r => r.GenerateRedirect(
                    It.Is<string>(s => s.EndsWith(fullPath))
                    )).Verifiable();
            }
            container.ReplaceDependency<IRedirectProvider>(redirectProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            redirectProvider.VerifyAll();
        }

        [Fact]
        public void CreateTheSyndicationContentExactlyOnce()
        {
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var syndicationProvider = new Mock<ISyndicationProvider>();
            container.ReplaceDependency<ISyndicationProvider>(syndicationProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            syndicationProvider.Verify(s => s.GenerateFeed(It.IsAny<IEnumerable<ContentItem>>()), Times.Once);
        }

        [Fact]
        public void ReturnTheSyndicationResourcePage()
        {
            string syndicationFileName = "syndication.xml";
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var syndicationProvider = new Mock<ISyndicationProvider>();
            syndicationProvider.Setup(s => s.GenerateFeed(It.IsAny<IEnumerable<ContentItem>>())).Returns(string.Empty.GetRandom());
            container.ReplaceDependency<ISyndicationProvider>(syndicationProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.Syndication && p.RelativeFilePath.EndsWith(syndicationFileName)));
        }

        [Fact]
        public void ReturnTheOutputOfTheSyndicationProviderAsTheContentOfTheSyndicationFile()
        {
            string syndicationFileExtension = "xml";
            string syndicationContent = string.Empty.GetRandom();

            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var syndicationProvider = new Mock<ISyndicationProvider>();
            syndicationProvider.Setup(s => s.GenerateFeed(It.IsAny<IEnumerable<ContentItem>>())).Returns(syndicationContent);
            container.ReplaceDependency<ISyndicationProvider>(syndicationProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            var actualFile = actual.Single(p => p.SourceTemplateType == Enumerations.TemplateType.Syndication && p.RelativeFilePath.EndsWith(syndicationFileExtension));
            Assert.Equal(syndicationContent, actualFile.Content);
        }

        [Fact]
        public void NotGenerateADasBlogCompatibilitySyndicationFileIfNoExtendedSettingExists()
        {
            string syndicationFileName = "syndication.axd";
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var syndicationProvider = new Mock<ISyndicationProvider>();
            syndicationProvider.Setup(s => s.GenerateFeed(It.IsAny<IEnumerable<ContentItem>>())).Returns(string.Empty.GetRandom());
            container.ReplaceDependency<ISyndicationProvider>(syndicationProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.Syndication && p.RelativeFilePath.EndsWith(syndicationFileName)));
        }

        [Fact]
        public void NotGenerateADasBlogCompatibilitySyndicationFileIfTheExtendedSettingSaysNotTo()
        {
            string syndicationFileName = "syndication.axd";
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            settings.AddExtendedSetting(_createDasBlogSyndicationCompatibilityFileSettingName, false.ToString());
            container.ReplaceDependency<ISettings>(settings);

            var syndicationProvider = new Mock<ISyndicationProvider>();
            syndicationProvider.Setup(s => s.GenerateFeed(It.IsAny<IEnumerable<ContentItem>>())).Returns(string.Empty.GetRandom());
            container.ReplaceDependency<ISyndicationProvider>(syndicationProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.Syndication && p.RelativeFilePath.EndsWith(syndicationFileName)));
        }

        [Fact]
        public void GenerateADasBlogCompatibilitySyndicationFileIfTheExtendedSettingExists()
        {
            string syndicationFileName = "syndication.axd";
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            settings.AddExtendedSetting(_createDasBlogSyndicationCompatibilityFileSettingName, true.ToString());
            container.ReplaceDependency<ISettings>(settings);

            var syndicationProvider = new Mock<ISyndicationProvider>();
            syndicationProvider.Setup(s => s.GenerateFeed(It.IsAny<IEnumerable<ContentItem>>())).Returns(string.Empty.GetRandom());
            container.ReplaceDependency<ISyndicationProvider>(syndicationProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.Syndication && p.RelativeFilePath.EndsWith(syndicationFileName)));
        }

        [Fact]
        public void NotGenerateADasBlogPostsCompatibilityFileIfNoExtendedSettingExists()
        {
            string postsFileName = "posts.xml";
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            container.ReplaceDependency<ISettings>(settings);

            var syndicationProvider = new Mock<ISyndicationProvider>();
            syndicationProvider.Setup(s => s.GenerateFeed(It.IsAny<IEnumerable<ContentItem>>())).Returns(string.Empty.GetRandom());
            container.ReplaceDependency<ISyndicationProvider>(syndicationProvider.Object);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.Raw && p.RelativeFilePath.EndsWith(postsFileName)));
        }

        [Fact]
        public void NotGenerateADasBlogPostsCompatibilityFileIfTheExtendedSettingSaysNotTo()
        {
            string postsFileName = "posts.xml";
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            settings.AddExtendedSetting(_createDasBlogPostsCompatibilityFileSettingName, false.ToString());
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.Raw && p.RelativeFilePath.EndsWith(postsFileName)));
        }

        [Fact]
        public void GenerateADasBlogPostsCompatibilityFileIfTheExtendedSettingExists()
        {
            string postsFileName = "posts.xml";
            var container = (null as IServiceCollection).Create();

            var posts = (null as IEnumerable<ContentItem>).Create();
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            container.ReplaceDependency<IContentRepository>(contentRepo.Object);

            var settings = (null as ISettings).Create();
            settings.SourceConnection = contentRepo.Object.GetSourceConnection();
            settings.AddExtendedSetting(_createDasBlogPostsCompatibilityFileSettingName, true.ToString());
            container.ReplaceDependency<ISettings>(settings);

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(p => p.SourceTemplateType == Enumerations.TemplateType.Raw && p.RelativeFilePath.EndsWith(postsFileName)));
        }

    }
}
