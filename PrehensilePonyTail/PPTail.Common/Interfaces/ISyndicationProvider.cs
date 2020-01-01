using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface ISyndicationProvider
    {
        String GenerateFeed(IEnumerable<Entities.ContentItem> posts);
    }
}
