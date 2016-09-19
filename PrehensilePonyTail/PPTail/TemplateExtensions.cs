using PPTail.Entities;
using PPTail.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public static class TemplateExtensions
    {
        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore, string styleTemplatePath, string bootstrapTemplatePath, string homePageTemplatePath, string contentPageTemplatePath, string postPageTemplatePath, string itemTemplatePath)
        {
            string contentPageTemplate = System.IO.File.ReadAllText(contentPageTemplatePath);
            string styleTemplate = System.IO.File.ReadAllText(styleTemplatePath);
            string bootstrapTemplate = System.IO.File.ReadAllText(bootstrapTemplatePath);
            string homePageTemplate = System.IO.File.ReadAllText(homePageTemplatePath);
            string postPageTemplate = System.IO.File.ReadAllText(postPageTemplatePath);
            string itemTemplate = System.IO.File.ReadAllText(itemTemplatePath);

            return new List<Template>()
            {
                new Template() { Content = contentPageTemplate, TemplateType = TemplateType.ContentPage },
                new Template() { Content = postPageTemplate, TemplateType = TemplateType.PostPage },
                new Template() { Content = styleTemplate, TemplateType = TemplateType.Style },
                new Template() { Content = bootstrapTemplate, TemplateType = TemplateType.Bootstrap },
                new Template() { Content = homePageTemplate, TemplateType = TemplateType.HomePage },
                new Template() { Content = itemTemplate, TemplateType = TemplateType.Item }
            };
        }

    }
}
