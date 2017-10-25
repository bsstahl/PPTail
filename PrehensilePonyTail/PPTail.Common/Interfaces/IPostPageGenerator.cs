using System;
using System.Collections.Generic;
using System.Text;
using PPTail.Entities;

namespace PPTail.Interfaces
{
    public interface IPostPageGenerator
    {
        IEnumerable<SiteFile> GetPostPages(ContentPageSource pageSource);
    }
}
