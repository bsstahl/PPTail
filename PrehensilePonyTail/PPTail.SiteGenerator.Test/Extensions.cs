using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;
using Moq;
using PPTail.Entities;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Enumerations;

namespace PPTail.SiteGenerator.Test
{
    public static class Extensions
    {

        public static Builder Create(this Builder ignore)
        {
            var contentRepo = Mock.Of<IContentRepository>();
            return ignore.Create(contentRepo, string.Empty.GetRandom());
        }

        public static Builder Create(this Builder ignore, IContentRepository contentRepo)
        {
            return ignore.Create(contentRepo, string.Empty.GetRandom());
        }

        public static Builder Create(this Builder ignore, IContentRepository contentRepo, string pageFilenameExtension)
        {
            return ignore.Create(contentRepo, Mock.Of<IArchiveProvider>(), pageFilenameExtension);
        }

        public static Builder Create(this Builder ignore, IContentRepository contentRepo, IArchiveProvider archiveProvider, string pageFilenameExtension)
        {
            var contactProvider = Mock.Of<IContactProvider>();
            return ignore.Create(contentRepo, archiveProvider, contactProvider, pageFilenameExtension);
        }

        public static Builder Create(this Builder ignore, IContentRepository contentRepo, IArchiveProvider archiveProvider, IContactProvider contactProvider, string pageFilenameExtension)
        {
            IServiceCollection container = new ServiceCollection();
            var pageGen = Mock.Of<IPageGenerator>();

            var settings = new Settings();
            settings.outputFileExtension = pageFilenameExtension;

            var navProvider = Mock.Of<INavigationProvider>();

            container.AddSingleton<IContentRepository>(contentRepo);
            container.AddSingleton<IPageGenerator>(pageGen);
            container.AddSingleton<Settings>(settings);
            container.AddSingleton<INavigationProvider>(navProvider);
            container.AddSingleton<IArchiveProvider>(archiveProvider);
            container.AddSingleton<IContactProvider>(contactProvider);

            return ignore.Create(container);
        }

        public static Builder Create(this Builder ignore, IServiceCollection container)
        {
            return new Builder(container.BuildServiceProvider());
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

        public static SiteSettings Create(this SiteSettings ignore)
        {
            return ignore.Create("My Test Blog", "A blog of epic scalability", 10.GetRandom(2));
        }

        public static SiteSettings Create(this SiteSettings ignore, string title, string description, int postsPerPage)
        {
            return new SiteSettings()
            {
                Title = title,
                Description = description,
                PostsPerPage = postsPerPage
            };
        }


        public static IEnumerable<SourceFile> Create(this IEnumerable<SourceFile> ignore)
        {
            return ignore.Create(10.GetRandom(3));
        }

        public static IEnumerable<SourceFile> Create(this IEnumerable<SourceFile> ignore, int count)
        {
            var result = new List<SourceFile>();

            for (int i = 0; i < count; i++)
                result.Add((null as SourceFile).Create());

            return result;
        }

        public static SourceFile Create(this SourceFile ignore)
        {
            return ignore.Create(
                string.Empty.GetRandom().Select(s => Convert.ToByte(s)).ToArray(),
                string.Empty.GetRandom(),
                string.Empty.GetRandom());
        }

        public static SourceFile Create(this SourceFile ignore, byte[] content, string relativePath, string fileName)
        {
            return new SourceFile()
            {
                Contents = content,
                RelativePath = relativePath,
                FileName = fileName
            };
        }

    }
}
