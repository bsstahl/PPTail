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
        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore, string styleTemplatePath, string bootstrapTemplatePath, string homePageTemplatePath, string contentPageTemplatePath, string postPageTemplatePath, string contactPageTemplatePath, string redirectTemplatePath, string itemTemplatePath, string searchTemplatePath)
        {
            string contentPageTemplate = System.IO.File.ReadAllText(contentPageTemplatePath);
            string styleTemplate = System.IO.File.ReadAllText(styleTemplatePath);
            string bootstrapTemplate = System.IO.File.ReadAllText(bootstrapTemplatePath);
            string homePageTemplate = System.IO.File.ReadAllText(homePageTemplatePath);
            string postPageTemplate = System.IO.File.ReadAllText(postPageTemplatePath);
            string contactPageTemplate = System.IO.File.ReadAllText(contactPageTemplatePath);
            string redirectTemplate = System.IO.File.ReadAllText(redirectTemplatePath);
            string itemTemplate = System.IO.File.ReadAllText(itemTemplatePath);
            string searchTemplate = System.IO.File.ReadAllText(searchTemplatePath);

            return new List<Template>()
            {
                new Template() { Content = contentPageTemplate, TemplateType = TemplateType.ContentPage },
                new Template() { Content = postPageTemplate, TemplateType = TemplateType.PostPage },
                new Template() { Content = styleTemplate, TemplateType = TemplateType.Style },
                new Template() { Content = bootstrapTemplate, TemplateType = TemplateType.Bootstrap },
                new Template() { Content = homePageTemplate, TemplateType = TemplateType.HomePage },
                new Template() { Content = contactPageTemplate, TemplateType = TemplateType.ContactPage },
                new Template() { Content = redirectTemplate, TemplateType = TemplateType.Redirect },
                new Template() { Content = itemTemplate, TemplateType = TemplateType.Item },
                new Template() { Content = searchTemplate, TemplateType = TemplateType.SearchPage }
            };
        }

    }
}
