using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IArchiveProvider
    {
        string GenerateArchive(Settings settings, Entities.SiteSettings siteSettings, IEnumerable<Entities.ContentItem> posts, IEnumerable<Entities.ContentItem> pages, string pathToRoot);
    }
}
