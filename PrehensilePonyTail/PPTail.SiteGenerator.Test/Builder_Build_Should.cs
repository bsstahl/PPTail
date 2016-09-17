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
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains("\\Pages\\")));
        }

        [Fact]
        public void ReturnOneItemInFolderPostsIfOnePostIsRetrieved()
        {
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });
            var target = (null as Builder).Create(contentRepo.Object);
            var actual = target.Build();
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains("\\Posts\\")));
        }

        [Fact]
        public void SetTheFilenameOfTheContentPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            var expected = $"\\Pages\\{contentItem.Slug}.{extension}";
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, extension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains(expected)));
        }

        [Fact]
        public void SetTheFilenameOfThePostPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            var expected = $"\\Posts\\{contentItem.Slug}.{extension}";
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, extension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains(expected)));
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
        public void CallThePageGeneratorExactlyOnceWithEachPublishedPage()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPages()).Returns(contentItems);

            var pageGen = new Mock<IPageGenerator>();
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var settings = new Settings();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var target = (null as Builder).Create(container);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.GenerateContentPage(It.IsAny<SiteSettings>(), item), Times.Once);
            }
        }

        [Fact]
        public void CallThePageGeneratorExactlyOnceWithEachPublishedPost()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPosts()).Returns(contentItems);

            var pageGen = new Mock<IPageGenerator>();
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var settings = new Settings();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);

            var siteSettings = (null as SiteSettings).Create();

            var target = (null as Builder).Create(container);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.GeneratePostPage(It.IsAny<SiteSettings>(), item), Times.Once);
            }
        }

        [Fact]
        public void CallGenerateStylesheetEactlyOnce()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = new Mock<IPageGenerator>();
            var settings = new Settings();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var target = (null as Builder).Create(container);
            var actual = target.Build();

            pageGen.Verify(c => c.GenerateStylesheet(It.IsAny<SiteSettings>()), Times.Once);
        }

        [Fact]
        public void CallGenerateHomepageEactlyOnce()
        {
            var container = new ServiceCollection();

            var contentRepo = new Mock<IContentRepository>();
            var pageGen = new Mock<IPageGenerator>();
            var settings = new Settings();

            container.AddSingleton<IContentRepository>(contentRepo.Object);
            container.AddSingleton<IPageGenerator>(pageGen.Object);
            container.AddSingleton<Settings>(settings);

            var siteSettings = (null as SiteSettings).Create();
            var target = (null as Builder).Create(container);
            var actual = target.Build();

            pageGen.Verify(c => c.GenerateHomepage(It.IsAny<SiteSettings>(), It.IsAny<IEnumerable<ContentItem>>()), Times.Once);
        }


    }
}
