using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.Redirect.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RedirectProvider_GenerateRedirect_Should
    {
        [Fact]
        public void IncludeTheUrlInTheOutput()
        {
            String url = string.Empty.GetRandom();
            String redirectTemplate = "window.location.assign(\"{Url}\");";
            var target = (null as IRedirectProvider).Create(redirectTemplate);
            var actual = target.GenerateRedirect(url);
            Assert.Contains(url, actual);
        }

        [Fact]
        public void IncludeTheRawTemplateInTheOutput()
        {
            String url = string.Empty.GetRandom();
            String command = "window.location";
            String redirectTemplate = "window.location.assign(\"{Url}\");";
            var target = (null as IRedirectProvider).Create(redirectTemplate);
            var actual = target.GenerateRedirect(url);
            Assert.Contains(command, actual);
        }

    }
}
