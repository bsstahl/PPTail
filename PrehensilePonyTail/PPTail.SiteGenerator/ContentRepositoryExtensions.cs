using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.SiteGenerator
{
    public static class ContentRepositoryExtensions
    {
        public static IEnumerable<SourceFile> GetFoldersContents(this IContentRepository contentRepo, IEnumerable<string> relativePaths)
        {
            var results = new List<SourceFile>();
            foreach (var relativePath in relativePaths)
                results.AddRange(contentRepo.GetFolderContents(relativePath));
            return results;
        }
    }
}
