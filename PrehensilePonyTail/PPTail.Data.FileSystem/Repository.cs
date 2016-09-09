using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;

namespace PPTail.Data.FileSystem
{
    public class Repository: Interfaces.IContentRepository
    {
        public Repository() { }

        public IEnumerable<ContentItem> GetAllPages()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentItem> GetAllPosts()
        {
            throw new NotImplementedException();
        }
    }
}
