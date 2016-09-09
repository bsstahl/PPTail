using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Generator.T4Html.Test
{
    public static class Extensions
    {
        const string _contentPageTemplatePath = @"..\ContentPage.template.html";
        const string _postPageTemplatePath = @"..\PostPage.template.html";

        public static IPageGenerator Create(this IPageGenerator ignore)
        {
            var cpt = System.IO.File.ReadAllText(_contentPageTemplatePath);
            var ppt = System.IO.File.ReadAllText(_postPageTemplatePath);
            return ignore.Create(cpt, ppt);
        }

        public static IPageGenerator Create(this IPageGenerator ignore, string contentPageTemplate, string postPageTemplate)
        {
            return new PPTail.Generator.T4Html.PageGenerator(contentPageTemplate, postPageTemplate);
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
    }
}
