using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IContentRepository
    {
        IEnumerable<ContentItem> GetAllPages();
        IEnumerable<ContentItem> GetAllPosts();
    }
}
