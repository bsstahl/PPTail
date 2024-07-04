using System;
using System.Collections.Generic;
using System.Linq;
using PPTail.Entities;
using PPTail.Extensions;

namespace PPTail.Content.UnpublishedPosts;

public static class ContentItemExtensions
{
    public static string ToMarkdown(this IEnumerable<ContentItem> items, string outputFileExtension)
        => string.Join("\r\n", items.Select(i => i.ToMarkdown(outputFileExtension)));

    public static string ToMarkdown(this ContentItem item, string outputFileExtension)
        => $"* [{item?.Title}]({{PathToRoot}}\\Posts\\{item?.Slug}.{outputFileExtension})".ToUrl();
}
