using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Web.Syndication.Test
{
    public static class Extensions
    {

        public static Mock<HttpContext> CreateMockContext(this HttpContext ignore, string path)
        {
            var query = (null as IQueryCollection).CreateMockQueryCollection();
            var request = (null as HttpRequest).CreateMockRequest(path, query.Object);
            var response = (null as HttpResponse).CreateMockResponse();
            return ignore.CreateMockContext(request, response);
        }

        public static Mock<HttpContext> CreateMockContext(this HttpContext ignore, Mock<HttpRequest> request, Mock<HttpResponse> response)
        {
            var context = new Mock<HttpContext>();
            context.SetupAllProperties();

            context.SetupGet(c => c.Response).Returns(response.Object);
            context.SetupGet(c => c.Request).Returns(request.Object);

            return context;
        }

        public static Mock<IQueryCollection> CreateMockQueryCollection(this IQueryCollection ignore)
        {
            return new Mock<IQueryCollection>();
        }

        public static Mock<HttpRequest> CreateMockRequest(this HttpRequest ignore, string path, IQueryCollection queryCollection)
        {
            var request = new Mock<HttpRequest>();
            request.SetupAllProperties();
            request.SetupGet(r => r.Query).Returns(queryCollection);
            request.SetupGet(r => r.Path).Returns(path);
            return request;
        }

        public static Mock<HttpResponse> CreateMockResponse(this HttpResponse ignore)
        {
            var response = new Mock<HttpResponse>();
            return response.SetupAllProperties();
        }

        public static void AddQueryParameter(this Mock<IQueryCollection> queryValueCollection, string key, string value)
        {
            var values = new StringValues(value);
            var pair = new KeyValuePair<string, StringValues>(key, values);
            queryValueCollection.SetupGet(q => q[key]).Returns(values);
        }

    }
}
