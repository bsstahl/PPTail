using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;

namespace PPTail.Generator.T4Html
{
    public class PageGenerator: Interfaces.IPageGenerator
    {
        public PageGenerator() { }

        public string GenerateContentPage(ContentItem pageData)
        {
            return $"<title>{pageData.Title}</title>";
        }

        public string GeneratePostPage(ContentItem article)
        {
            throw new NotImplementedException();
        }
    }
}
