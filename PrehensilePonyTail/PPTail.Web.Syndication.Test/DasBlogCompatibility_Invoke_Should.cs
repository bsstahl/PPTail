using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Web.Syndication.Test
{
    public class DasBlogCompatibility_Invoke_Should
    {
        const string _dasBlogSyndicationFile = "syndication.axd";
        const string _currentSyndicationFile = "syndication.xml";

        [Fact]
        public void CallInvokeOnTheNextDelegateIfNotACallToTheSyndicationFile()
        {
            bool executed = false;

            string path = $"/{string.Empty.GetRandom()}.html";
            System.Diagnostics.Debug.Assert(!path.EndsWith(_dasBlogSyndicationFile));

            var context = (null as HttpContext).CreateMockContext(path);

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                executed = true;
                return Task.CompletedTask;
            });

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            Assert.True(executed);
        }

        [Fact]
        public void PassTheHttpContextToTheInvokeCallOnTheNextDelegate()
        {
            HttpContext actual = null;
            string path = $"/{string.Empty.GetRandom()}.html";
            var context = (null as HttpContext).CreateMockContext(path);

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                actual = c;
                return Task.CompletedTask;
            });

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            Assert.Equal(context.Object, actual);
        }

        [Fact]
        public void RedirectIfCallIsToDasBlogSyndicationFile()
        {
            string path = $"/{_dasBlogSyndicationFile}";

            var request = (null as HttpRequest).CreateMockRequest(path, null);
            var response = (null as HttpResponse).CreateMockResponse();
            var context = (null as HttpContext).CreateMockContext(request, response);

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                return Task.CompletedTask;
            });

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            response.Verify(r => r.Redirect(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void RedirectToNewSyndicationFileIfCallIsToDasBlogSyndicationFile()
        {
            string path = $"/{_dasBlogSyndicationFile}";
            string expectedPath = $"/{_currentSyndicationFile}";

            var request = (null as HttpRequest).CreateMockRequest(path, null);
            var response = (null as HttpResponse).CreateMockResponse();
            var context = (null as HttpContext).CreateMockContext(request, response);

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                return Task.CompletedTask;
            });

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            response.Verify(r => r.Redirect(expectedPath, It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void RedirectToSameFolderIfCallIsToDasBlogSyndicationFile()
        {
            string folderName = string.Empty.GetRandom();
            string expectedPath = $"/{folderName}/{_currentSyndicationFile}";
            string path = $"/{folderName}/{_dasBlogSyndicationFile}";

            var request = (null as HttpRequest).CreateMockRequest(path, null);
            var response = (null as HttpResponse).CreateMockResponse();
            var context = (null as HttpContext).CreateMockContext(request, response);

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                return Task.CompletedTask;
            });

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            response.Verify(r => r.Redirect(expectedPath, It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void IssuesAPermanantRedirectIfCallIsToDasBlogSyndicationFile()
        {
            string folderName = string.Empty.GetRandom();
            string expectedPath = $"/{folderName}/{_currentSyndicationFile}";
            string path = $"/{folderName}/{_dasBlogSyndicationFile}";

            var request = (null as HttpRequest).CreateMockRequest(path, null);
            var response = (null as HttpResponse).CreateMockResponse();
            var context = (null as HttpContext).CreateMockContext(request, response);

            var target = new DasBlogCompatibility((HttpContext c) =>
            {
                return Task.CompletedTask;
            });

            var t = target.Invoke(context.Object);
            Task.WaitAll(t);

            response.Verify(r => r.Redirect(It.IsAny<string>(), true), Times.Once);
        }
    }
}
