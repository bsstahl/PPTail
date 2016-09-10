using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PPTail.Interfaces;
using PPTail.Entities;
using TestHelperExtensions;

namespace PPTail.SiteGenerator.Test
{
    public class Builder_Build_Should
    {
        [Fact]
        public void RequestAllPagesFromTheRepository()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            contentRepo.Verify(c => c.GetAllPages(), Times.AtLeastOnce());
        }

        [Fact]
        public void RequestAllPostsFromTheRepository()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            contentRepo.Verify(c => c.GetAllPosts(), Times.AtLeastOnce());
        }

        [Fact]
        public void ReturnOneItemInFolderPagesIfOnePageIsRetrieved()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });
            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();
            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains("\\Pages\\")));
        }

        [Fact]
        public void ReturnOneItemInFolderPostsIfOnePostIsRetrieved()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains("\\Posts\\")));
        }

        [Fact]
        public void SetTheFilenameOfTheContentPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            var expected = $"\\Pages\\{contentItem.Slug}.{extension}";
            contentRepo.Setup(c => c.GetAllPages()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, pageGen, extension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains(expected)));
        }

        [Fact]
        public void SetTheFilenameOfThePostPageToTheSlugPlusTheExtension()
        {
            string extension = string.Empty.GetRandom(4);
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();
            var contentItem = (null as ContentItem).Create();
            var expected = $"\\Posts\\{contentItem.Slug}.{extension}";
            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => new List<ContentItem>() { contentItem });

            var target = (null as Builder).Create(contentRepo.Object, pageGen, extension);
            var actual = target.Build();

            Assert.Equal(1, actual.Count(f => f.RelativeFilePath.Contains(expected)));
        }

        [Fact]
        public void DontCreateAnyFilesIfAllPagesAreUnpublished()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(0, actual.Count());
        }

        [Fact]
        public void OnlyCreateAsManyFilesAsThereArePublishedPages()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var expected = contentItems.Count(ci => ci.IsPublished);

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void DoNotCreateOutputForAnUnpublishedPage()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true;

            var unpublishedItem = contentItems.GetRandom();
            unpublishedItem.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPages()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(ci => ci.RelativeFilePath.Contains(unpublishedItem.Slug)));
        }


        [Fact]
        public void DontCreateAnyFilesIfAllPostsAreUnpublished()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(0, actual.Count());
        }

        [Fact]
        public void OnlyCreateAsManyFilesAsThereArePublishedPosts()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var expected = contentItems.Count(ci => ci.IsPublished);

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(expected, actual.Count());
        }

        [Fact]
        public void DoNotCreateOutputForAnUnpublishedPost()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            foreach (var item in contentItems)
                item.IsPublished = true;

            var unpublishedItem = contentItems.GetRandom();
            unpublishedItem.IsPublished = false;

            contentRepo.Setup(c => c.GetAllPosts()).Returns(() => contentItems);

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            Assert.Equal(0, actual.Count(ci => ci.RelativeFilePath.Contains(unpublishedItem.Slug)));
        }

        [Fact]
        public void CallThePageGeneratorExactlyOnceWithEachPublishedPage()
        {
            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPages()).Returns(contentItems);

            var pageGen = new Mock<IPageGenerator>();
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var target = (null as Builder).Create(contentRepo.Object, pageGen.Object);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.GenerateContentPage(item), Times.Once);
            }
        }

        [Fact]
        public void CallThePageGeneratorExactlyOnceWithEachPublishedPost()
        {
            var contentRepo = new Mock<IContentRepository>();
            var contentItems = (null as IEnumerable<ContentItem>).Create(50.GetRandom(25));
            contentRepo.Setup(c => c.GetAllPosts()).Returns(contentItems);

            var pageGen = new Mock<IPageGenerator>();
            foreach (var item in contentItems)
                item.IsPublished = true.GetRandom();

            var target = (null as Builder).Create(contentRepo.Object, pageGen.Object);
            var actual = target.Build();

            foreach (var item in contentItems)
            {
                if (item.IsPublished)
                    pageGen.Verify(c => c.GeneratePostPage(item), Times.Once);
            }
        }
    }
}
