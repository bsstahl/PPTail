using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PPTail.Extensions
{
    public static class ContentRepositoryExtensions
    {
        public static ContentItem GetPostBySlug(this IContentRepository repo, string slug, bool caseSensitive = false)
        {
            Func<ContentItem, String> field = i => i.Slug;
            Func<IContentRepository, IEnumerable<ContentItem>> method = r => r.GetAllPosts();
            return repo.GetItemByStringField(method, field, slug, caseSensitive);
        }

        public static ContentItem GetPageBySlug(this IContentRepository repo, string slug, bool caseSensitive = false)
        {
            Func<ContentItem, String> field = i => i.Slug;
            Func<IContentRepository, IEnumerable<ContentItem>> method = r => r.GetAllPages();
            return repo.GetItemByStringField(method, field, slug, caseSensitive);
        }

        internal static ContentItem GetItemByStringField(this IContentRepository repo, Func<IContentRepository, IEnumerable<ContentItem>> method, Func<ContentItem, String> fieldDelegate, string fieldValue, bool caseSensitive)
        {
            var culture = CultureInfo.CurrentCulture;
            Func<ContentItem, String> field = caseSensitive ? fieldDelegate : i => fieldDelegate.Invoke(i).ToLower(culture);
            string value = caseSensitive ? fieldValue : fieldValue.ToLower(culture);
            return method.Invoke(repo).SingleOrDefault(i => field.Invoke(i) == value);
        }

    }
}
