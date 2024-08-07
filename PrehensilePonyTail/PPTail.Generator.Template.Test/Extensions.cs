﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestHelperExtensions;
using PPTail.Interfaces;
using PPTail.Entities;

namespace PPTail.Generator.Template.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IServiceCollection Create(this IServiceCollection? ignore)
        {
            var contentRepo = new Mock<IContentRepository>();
            return ignore.Create(contentRepo);
        }

        public static IServiceCollection Create(this IServiceCollection? ignore, Mock<IContentRepository> mockContentRepo)
        {
            var siteSettings = (null as SiteSettings).Create();
            return ignore.Create(mockContentRepo, siteSettings);
        }

        public static IServiceCollection Create(this IServiceCollection? _, Mock<IContentRepository> mockContentRepo, SiteSettings siteSettings)
        {
            var container = new ServiceCollection();
            container.AddSingleton<IEnumerable<Entities.Template>>((null as IEnumerable<Entities.Template>).Create());
            container.AddSingleton<IEnumerable<Category>>(new List<Category>());

            mockContentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);
            container.AddSingleton<IContentRepository>(mockContentRepo.Object);

            container.AddSingleton<ILinkProvider>(Mock.Of<ILinkProvider>());
            container.AddSingleton<IContentEncoder>(Mock.Of<IContentEncoder>());

            return container;
        }

        //public static IContentRepository Create(this IContentRepository ignore)
        //{
        //    return ignore.Create(new SiteSettings());
        //}

        public static IContentRepository Create(this IContentRepository? _, SiteSettings siteSettings)
        {
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings);
            return contentRepo.Object;
        }

        public static ITemplateProcessor Create(this ITemplateProcessor? ignore, IServiceCollection container)
        {
            return ignore.Create(container.BuildServiceProvider());
        }

        public static ITemplateProcessor Create(this ITemplateProcessor? _, IServiceProvider serviceProvider)
        {
            return new TemplateProcessor(serviceProvider);
        }

        public static IEnumerable<Entities.Template> Create(this IEnumerable<Entities.Template>? _)
        {
            var templates = new List<Entities.Template>();
            foreach (Enumerations.TemplateType templateType in Enum.GetValues(typeof(Enumerations.TemplateType)))
                templates.Add((null as Entities.Template).Create(templateType));
            return templates;
        }

        public static Entities.Template Create(this Entities.Template ignore)
        {
            throw new NotImplementedException();
        }

        public static Entities.Template Create(this Entities.Template? _, Enumerations.TemplateType templateType)
        {
            return new Entities.Template()
            {
                Content = string.Empty.GetRandom(),
                TemplateType = templateType
            };
        }

        public static ContentItem Create(this ContentItem? ignore)
        {
            return ignore.Create(string.Empty.GetRandom(), new List<Guid>() { Guid.NewGuid() }, string.Empty.GetRandom(), string.Empty.GetRandom(), true.GetRandom(), DateTime.UtcNow.AddMinutes(10.GetRandom()), DateTime.UtcNow.AddHours(10.GetRandom(1)), string.Empty.GetRandom(), new List<string>() { string.Empty.GetRandom() }, string.Empty.GetRandom(), string.Empty.GetRandom());
        }

        public static ContentItem Create(this ContentItem? _, String author, IEnumerable<Guid> categoryIds, String content, String description, bool isPublished, DateTime lastModDate, DateTime pubDate, String slug, IEnumerable<string> tags, String title, String teaser)
        {
            return new ContentItem()
            {
                Author = author,
                CategoryIds = categoryIds,
                Content = content,
                Description = description,
                Id = Guid.NewGuid(),
                IsPublished = isPublished,
                LastModificationDate = lastModDate,
                PublicationDate = pubDate,
                Slug = slug,
                Tags = tags,
                Title = title,
                ByLine = $"by {author}",
                Teaser = teaser
            };
        }


        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem>? ignore)
        {
            return ignore.Create(50.GetRandom(25));
        }

        public static IEnumerable<ContentItem> Create(this IEnumerable<ContentItem>? _, Int32 count)
        {
            var result = new List<ContentItem>();
            for (Int32 i = 0; i < count; i++)
                result.Add((null as ContentItem).Create());
            return result;
        }

        public static SiteSettings Create(this SiteSettings? _)
        {
            return new SiteSettings()
            {
                Title = string.Empty.GetRandom(),
                Description = string.Empty.GetRandom(),
                PostsPerPage = 25.GetRandom(5),
                PostsPerFeed = 25.GetRandom(5)
            };
        }

        public static IEnumerable<Category> CreateCategories(this IEnumerable<Guid> categoryIds)
        {
            var result = new List<Category>();
            foreach (var categoryId in categoryIds)
            {
                String name = categoryId.ToString().Substring(0, 8);
                result.Add(new Category() { Id = categoryId, Name = name, Description = $"descriptionOf_{name}" });
            }
            return result;
        }

        public static String GetCategoryName(this Guid categoryId, IEnumerable<Category> categories)
        {
            return categories.Single(c => c.Id == categoryId).Name;
        }

        public static IServiceCollection RemoveDependency<T>(this IServiceCollection container) where T : class
        {
            var item = container.Where(sd => sd.ServiceType == typeof(T)).Single();
            container.Remove(item);
            return container;
        }

        public static IServiceCollection ReplaceDependency<T>(this IServiceCollection container, T serviceInstance) where T : class
        {
            container.RemoveDependency<T>();
            container.AddSingleton<T>(serviceInstance);
            return container;
        }
    }
}
