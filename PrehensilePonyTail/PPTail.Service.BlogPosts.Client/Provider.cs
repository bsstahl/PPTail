using PPTail.Interfaces;
using System;
using PPTail.Entities;
using System.Collections.Generic;

namespace PPTail.Service.BlogPosts.Client
{
    public class Provider : IPostPageGenerator
    {
        public IEnumerable<SiteFile> GetPostPages(ContentPageSource pageSource)
        {
            throw new NotImplementedException();
        }
    }
}
