using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;
using Moq;
using PPTail.Entities;
using TestHelperExtensions;

namespace PPTail.SiteGenerator.Test
{
    public static class Extensions
    {
        public static Builder Create(this Builder ignore)
        {
            IContentRepository contentRepo = Mock.Of<IContentRepository>();
            IPageGenerator pageGen = Mock.Of<IPageGenerator>();
            return ignore.Create(contentRepo, pageGen);
        }

        public static Builder Create(this Builder ignore, IContentRepository contentRepo, IPageGenerator pageGen)
        {
            return ignore.Create(contentRepo, pageGen, "html");
        }

        public static Builder Create(this Builder ignore, IContentRepository contentRepo, IPageGenerator pageGen, string pageFilenameExtension)
        {
            return new Builder(contentRepo, pageGen, pageFilenameExtension);
        }

        public static ContentItem Create(this ContentItem ignore)
        {
            return new ContentItem()
            {
                Author = string.Empty.GetRandom(),
                CategoryIds = new List<Guid>() { Guid.NewGuid() },
                Content = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                IsPublished = true,
                LastModificationDate = DateTime.UtcNow.AddDays(-10.GetRandom()),
                PublicationDate = DateTime.UtcNow.AddDays(-20.GetRandom(10)),
                Slug = string.Empty.GetRandom(),
                Tags = new List<string>() { string.Empty.GetRandom() },
                Title = string.Empty.GetRandom()
            };
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem> ignore, int count)
        {
            var contentItems = new List<ContentItem>();
            for (int i = 0; i < count; i++)
                contentItems.Add((null as ContentItem).Create());
            return contentItems;
        }

    }
}
