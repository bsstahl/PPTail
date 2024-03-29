﻿using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Extensions
{
    public static class ContentItemExtensions
    {
        public static IEnumerable<string> GetAllTags(this IEnumerable<ContentItem> contentItems)
        {
            return contentItems
                .Where(p => p.IsPublished && p.Tags.IsNotNull())
                .SelectMany(p => p.Tags)
                .Where(t => !string.IsNullOrWhiteSpace(t));
        }
    }
}
