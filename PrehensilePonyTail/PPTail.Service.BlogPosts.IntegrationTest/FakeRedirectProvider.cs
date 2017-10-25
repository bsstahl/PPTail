using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Service.BlogPosts.IntegrationTest
{
    public class FakeRedirectProvider : IRedirectProvider
    {
        public string GenerateRedirect(string redirectToUrl)
        {
            return redirectToUrl;
        }
    }
}
