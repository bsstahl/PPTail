using PPTail.Entities;
using PPTail.Interfaces;
using System.Collections.Generic;

namespace PPTail.SiteGenerator;

public static class ContentRepositoryExtensions
{
    public static IEnumerable<SourceFile> GetFoldersContents(this IContentRepository contentRepo, IEnumerable<string> relativePaths, bool recursive)
    {
        var results = new List<SourceFile>();
        foreach (var relativePath in relativePaths)
            results.AddRange(contentRepo.GetFolderContents(relativePath, recursive));
        return results;
    }
}
