using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;

namespace PPTail.SiteGenerator;

public static class ContentRepositoryExtensions
{
    public static IEnumerable<SourceFile> GetFoldersContents(this IContentRepository contentRepo, IEnumerable<string> relativePaths, bool recursive)
    {
        if (contentRepo is null)
            throw new ArgumentNullException(nameof(contentRepo));

        var results = new List<SourceFile>();
        foreach (string relativePath in relativePaths ?? [])
            results.AddRange(contentRepo.GetFolderContents(relativePath, recursive));
        return results;
    }
}
