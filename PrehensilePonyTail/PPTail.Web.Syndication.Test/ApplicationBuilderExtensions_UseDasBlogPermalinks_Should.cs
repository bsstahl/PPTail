using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Web.Syndication.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ApplicationBuilderExtensions_UseDasBlogPermalinks_Should
    {
        [Fact]
        public void CallUseOnTheApplicationBuilder()
        {
            var target = new Mock<IApplicationBuilder>();
            target.Object.UseDasBlogSyndication();
            target.Verify(t => t.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()), Times.Once);
        }

        [Fact]
        public void ReturnTheResultOfTheUseMethod()
        {
            var target = new Mock<IApplicationBuilder>();
            var expected = Mock.Of<IApplicationBuilder>();
            target.Setup(t => t.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(expected);
            var actual = target.Object.UseDasBlogSyndication();
            Assert.Equal(expected, actual);
        }
    }
}
