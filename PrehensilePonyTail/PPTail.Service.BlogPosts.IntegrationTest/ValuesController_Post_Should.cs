using System;
using System.Linq;
using Xunit;
using PPTail.Interfaces;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Service.BlogPosts.IntegrationTest
{
    public class ValuesController_Post_Should
    {
        [Fact]
        public void ReturnTwoResultItemsIfOnePostExists()
        {
            var serviceCollection = new ServiceCollection();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            IContentItemPageGenerator pageGen = new PPTail.Generator.ContentPage.PageGenerator(serviceProvider);
            IContentEncoder contentEncoder = new FakeContentEncoder();
            IRedirectProvider redirectProvider = new FakeRedirectProvider();

            ContentPageSource pageSource = new PPTail.Entities.ContentPageSource();
            pageSource.ContentItem = new ContentItem();
            pageSource.Settings = new Settings();

            var controller = new PPTail.Service.BlogPosts.ValuesController();
            var actual = controller.Post(contentEncoder, pageGen, redirectProvider, pageSource);

            Assert.Equal(2, actual.Count());
        }
    }
}
