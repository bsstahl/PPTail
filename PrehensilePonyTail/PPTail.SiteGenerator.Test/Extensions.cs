using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;
using Moq;

namespace PPTail.SiteGenerator.Test
{
    public static class Extensions
    {
        public static Builder Create(this Builder ignore)
        {
            IContentRepository contentRepo = Mock.Of<IContentRepository>();
            IPageGenerator pageGen = Mock.Of<IPageGenerator>();
            return ignore.Create(contentRepo, pageGen);
        }

        public static Builder Create(this Builder ignore, IContentRepository contentRepo, IPageGenerator pageGen)
        {
            return new Builder(contentRepo, pageGen);
        }
    }
}
