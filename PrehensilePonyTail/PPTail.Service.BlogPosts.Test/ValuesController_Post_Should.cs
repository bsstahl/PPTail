using System;
using Xunit;
using PPTail.Interfaces;
using PPTail.Entities;
using Moq;

namespace PPTail.Service.BlogPosts.Test
{
    public class ValuesController_Post_Should
    {
        [Fact]
        public void NotThrowANullReferenceException()
        {
            IContentEncoder contentEncoder = null;
            IContentItemPageGenerator pageGen = null;
            IRedirectProvider redirectProvider = null;
            ContentPageSource pageSource = null;

            var controller = new PPTail.Service.BlogPosts.ValuesController();

            try
            {
                var actual = controller.Post(contentEncoder, pageGen, redirectProvider, pageSource);
            }
            catch (NullReferenceException)
            {
                Assert.True(false, "NullReferenceException should not be thrown");
            }
            catch (Exception) { }

            Assert.True(true);
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfPageSourceIsNotSupplied()
        {
            IContentEncoder contentEncoder = Mock.Of<IContentEncoder>();
            IContentItemPageGenerator pageGen = Mock.Of<IContentItemPageGenerator>();
            IRedirectProvider redirectProvider = Mock.Of<IRedirectProvider>();
            ContentPageSource pageSource = null;

            var controller = new PPTail.Service.BlogPosts.ValuesController();
            Assert.Throws<ArgumentNullException>(() => controller.Post(contentEncoder, pageGen, redirectProvider, pageSource));
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfContentItemIsNotSuppliedOnThePageSource()
        {
            IContentEncoder contentEncoder = Mock.Of<IContentEncoder>();
            IContentItemPageGenerator pageGen = Mock.Of<IContentItemPageGenerator>();
            IRedirectProvider redirectProvider = Mock.Of<IRedirectProvider>();

            ContentPageSource pageSource = Mock.Of<ContentPageSource>();
            pageSource.ContentItem = null;
            pageSource.Settings = Mock.Of<ISettings>();

            var controller = new PPTail.Service.BlogPosts.ValuesController();
            Assert.Throws<ArgumentNullException>(() => controller.Post(contentEncoder, pageGen, redirectProvider, pageSource));
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfSettingsAreNotSuppliedOnThePageSource()
        {
            IContentEncoder contentEncoder = Mock.Of<IContentEncoder>();
            IContentItemPageGenerator pageGen = Mock.Of<IContentItemPageGenerator>();
            IRedirectProvider redirectProvider = Mock.Of<IRedirectProvider>();

            ContentPageSource pageSource = Mock.Of<ContentPageSource>();
            pageSource.ContentItem = Mock.Of<ContentItem>();
            pageSource.Settings = null;

            var controller = new PPTail.Service.BlogPosts.ValuesController();
            Assert.Throws<ArgumentNullException>(() => controller.Post(contentEncoder, pageGen, redirectProvider, pageSource));
        }

        [Fact]
        public void EncodeTheTitleAsThePageSlugIfNoSlugIsProvided()
        {
            IContentItemPageGenerator pageGen = Mock.Of<IContentItemPageGenerator>();
            IRedirectProvider redirectProvider = Mock.Of<IRedirectProvider>();

            ContentPageSource pageSource = Mock.Of<ContentPageSource>();
            pageSource.ContentItem = Mock.Of<ContentItem>();
            pageSource.ContentItem.Slug = string.Empty;
            pageSource.ContentItem.Title = Guid.NewGuid().ToString();

            pageSource.Settings = Mock.Of<ISettings>();

            var mockContentEncoder = new Mock<IContentEncoder>();
            mockContentEncoder.Setup(e => e.UrlEncode(pageSource.ContentItem.Title)).Verifiable();
            var contentEncoder = mockContentEncoder.Object;

            var controller = new PPTail.Service.BlogPosts.ValuesController();
            var actual = controller.Post(contentEncoder, pageGen, redirectProvider, pageSource);

            mockContentEncoder.VerifyAll();
        }

    }
}
