using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IPageGenerator
    {
        string GenerateContentPage(Entities.ContentItem pageData);
        string GeneratePostPage(Entities.ContentItem article);
    }
}
