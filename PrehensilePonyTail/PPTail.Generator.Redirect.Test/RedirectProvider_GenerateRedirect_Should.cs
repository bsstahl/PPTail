using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.Redirect.Test
{
    public class RedirectProvider_GenerateRedirect_Should
    {
        [Fact]
        public void IncludeTheUrlInTheOutput()
        {
            string url = string.Empty.GetRandom();
            var target = new RedirectProvider();
            var actual = target.GenerateRedirect(url);
            Assert.Contains(url, actual);
        }

        [Fact]
        public void IncludeARedirectInTheOutput()
        {
            string url = string.Empty.GetRandom();
            string command = "window.location.assign(";
            var target = new RedirectProvider();
            var actual = target.GenerateRedirect(url);
            Assert.Contains(command, actual);
        }
    }
}
