using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.SiteGenerator
{
    public class Builder
    {
        public Builder(IContentRepository contentRepo, IPageGenerator pageGen)
        {
        }

        public IEnumerable<SiteFile> Build()
        {
            var result = new List<SiteFile>();

            //TODO: Implement

            return result;
        }
    }
}
