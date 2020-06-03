using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using TestHelperExtensions;
using Microsoft.Extensions.Primitives;
using PPTail.Interfaces;
using PPTail.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Web.Permalinks.Test
{
    public class DasBlogCompatibility_Invoke_Should
    {
        const String _dasBlogPostsFile = "post.aspx";

        [Fact]
        public void CallInvokeOnTheNextDelegateIfNotACallToThePostFile()
        {
            bool executed = false;
            String path = $"/{string.Empty.GetRandom()}.html";
            var context = (null as HttpContext).CreateMockContext(path);
            var serviceProvider = (null as IServiceProvider).Create();

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                executed = true;
                return Task.CompletedTask;
            }, serviceProvider);

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            Assert.True(executed);
        }

        [Fact]
        public void PassTheHttpContextToTheInvokeCallOnTheNextDelegate()
        {
            HttpContext actual = null;
            String path = $"/{string.Empty.GetRandom()}.html";
            var context = (null as HttpContext).CreateMockContext(path);
            var serviceProvider = (null as IServiceProvider).Create();

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                actual = c;
                return Task.CompletedTask;
            }, serviceProvider);

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            Assert.Equal(context.Object, actual);
        }

        [Fact]
        public void ReturnA404ResponseIfTheIdIsNotAValidGuid()
        {
            HttpContext actual = null;

            String path = $"/{_dasBlogPostsFile}";
            var query = (null as IQueryCollection).CreateMockQueryCollection();
            query.AddQueryParameter("id", "1234");

            var request = (null as HttpRequest).CreateMockRequest(path, query.Object);
            var response = (null as HttpResponse).CreateMockResponse();

            var context = (null as HttpContext).CreateMockContext(request, response);
            var serviceProvider = (null as IServiceProvider).Create();

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                actual = c;
                return Task.CompletedTask;
            }, serviceProvider);

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            Assert.Equal(404, context.Object.Response.StatusCode);
        }

        [Fact]
        public void PassTheCorrectPostIdToThePostLocator()
        {
            String path = $"/{_dasBlogPostsFile}";
            Guid id = Guid.NewGuid();

            var query = (null as IQueryCollection).CreateMockQueryCollection();
            query.AddQueryParameter("id", id.ToString());

            var request = (null as HttpRequest).CreateMockRequest(path, query.Object);
            var response = (null as HttpResponse).CreateMockResponse();

            var context = (null as HttpContext).CreateMockContext(request, response);
            var postLocator = new Mock<IPostLocator>();

            var serviceCollection = (null as IServiceCollection).Create();
            serviceCollection.ReplaceDependency<IPostLocator>(postLocator.Object);

            var target = new DasBlogCompatibility((HttpContext c) => { throw new InvalidOperationException("The next delegate should not be called in this test"); }, 
                serviceCollection.BuildServiceProvider());

            var t = target.Invoke(context.Object);
            // No need to wait for the task since it was never invoked --

            postLocator.Verify(l => l.GetUrlByPostId(id), Times.Once);
        }

        [Fact]
        public void ReturnA404ResponseIfThePostIdIsNotFound()
        {
            String path = $"/{_dasBlogPostsFile}";
            Guid id = Guid.NewGuid();

            var query = (null as IQueryCollection).CreateMockQueryCollection();
            query.AddQueryParameter("id", id.ToString());

            var request = (null as HttpRequest).CreateMockRequest(path, query.Object);
            var response = (null as HttpResponse).CreateMockResponse();

            var headers = Mock.Of<IHeaderDictionary>();
            response.SetupGet(r => r.Headers).Returns(headers);

            var context = (null as HttpContext).CreateMockContext(request, response);
            var postLocator = new Mock<IPostLocator>();
            postLocator.Setup(l => l.GetUrlByPostId(It.IsAny<Guid>()))
                .Throws(new ContentItemNotFoundException(id));

            var serviceCollection = (null as IServiceCollection).Create();
            serviceCollection.ReplaceDependency<IPostLocator>(postLocator.Object);

            var target = new DasBlogCompatibility((HttpContext c) => { throw new InvalidOperationException("The next delegate should not be called in this test"); },
                serviceCollection.BuildServiceProvider());

            var t = target.Invoke(context.Object);
            // No need to wait for the task since it was never invoked

            Assert.Equal(404, context.Object.Response.StatusCode);
        }

        [Fact]
        public void RedirectToThePostUrlBasedOnThePostId()
        {
            String path = $"/{_dasBlogPostsFile}";
            Guid id = Guid.NewGuid();
            String postUrl = $"{string.Empty.GetRandom()}.html";

            var query = (null as IQueryCollection).CreateMockQueryCollection();
            query.AddQueryParameter("id", id.ToString());

            var request = (null as HttpRequest).CreateMockRequest(path, query.Object);
            var response = (null as HttpResponse).CreateMockResponse();

            var context = (null as HttpContext).CreateMockContext(request, response);

            var postLocator = new Mock<IPostLocator>();
            postLocator.Setup(l => l.GetUrlByPostId(It.IsAny<Guid>()))
                .Returns(postUrl);

            var serviceCollection = (null as IServiceCollection).Create();
            serviceCollection.ReplaceDependency<IPostLocator>(postLocator.Object);

            var target = new DasBlogCompatibility((HttpContext c) => { throw new InvalidOperationException("The next delegate should not be called in this test"); },
                serviceCollection.BuildServiceProvider());

            var t = target.Invoke(context.Object);
            // No need to wait for the task since it was never invoked --

            response.Verify(r => r.Redirect(postUrl, It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void IssuesAPermanantRedirectToThePostUrl()
        {
            String path = $"/{_dasBlogPostsFile}";
            Guid id = Guid.NewGuid();
            String postUrl = $"{string.Empty.GetRandom()}.html";

            var query = (null as IQueryCollection).CreateMockQueryCollection();
            query.AddQueryParameter("id", id.ToString());

            var request = (null as HttpRequest).CreateMockRequest(path, query.Object);
            var response = (null as HttpResponse).CreateMockResponse();

            var headers = Mock.Of<IHeaderDictionary>();
            response.SetupGet(r => r.Headers).Returns(headers);

            var context = (null as HttpContext).CreateMockContext(request, response);

            var postLocator = new Mock<IPostLocator>();
            postLocator.Setup(l => l.GetUrlByPostId(It.IsAny<Guid>()))
                .Returns(postUrl);

            var serviceCollection = (null as IServiceCollection).Create();
            serviceCollection.ReplaceDependency<IPostLocator>(postLocator.Object);

            var target = new DasBlogCompatibility((HttpContext c) => { throw new InvalidOperationException("The next delegate should not be called in this test"); },
                serviceCollection.BuildServiceProvider());

            var t = target.Invoke(context.Object);
            // No need to wait for the task since it was never invoked --

            response.Verify(r => r.Redirect(It.IsAny<string>(), true), Times.Once);
        }
    }
}
