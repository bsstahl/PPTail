using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public static class TemplateExtensions
    {
        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore, string rootTemplatePath)
        {
            string styleTemplatePath = $"{rootTemplatePath}\\Style.template.css";
            string bootstrapTemplatePath = $"{rootTemplatePath}\\bootstrap.min.css";
            string homePageTemplatePath = $"{rootTemplatePath}\\HomePage.template.html";
            string searchPageTemplatePath = $"{rootTemplatePath}\\ContentPage.template.html";
            string contentPageTemplatePath = $"{rootTemplatePath}\\ContentPage.template.html";
            string postPageTemplatePath = $"{rootTemplatePath}\\PostPage.template.html";
            string redirectTemplatePath = $"{rootTemplatePath}\\Redirect.template.html";
            string archiveTemplatePath = $"{rootTemplatePath}\\Archive.template.html";
            string archiveItemTemplatePath = $"{rootTemplatePath}\\ArchiveItem.template.html";
            string syndicationTemplatePath = $"{rootTemplatePath}\\Syndication.template.xml";
            string syndicationItemTemplatePath = $"{rootTemplatePath}\\SyndicationItem.template.xml";
            string contactPageTemplatePath = $"{rootTemplatePath}\\ContactPage.template.html";
            string itemTemplatePath = $"{rootTemplatePath}\\ContentItem.template.html";
            return (null as IEnumerable<Template>).Create(styleTemplatePath, bootstrapTemplatePath, homePageTemplatePath, contentPageTemplatePath, postPageTemplatePath, contactPageTemplatePath, redirectTemplatePath, syndicationTemplatePath, syndicationItemTemplatePath, itemTemplatePath, searchPageTemplatePath, archiveTemplatePath, archiveItemTemplatePath);

        }

        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore, string styleTemplatePath, string bootstrapTemplatePath, string homePageTemplatePath, string contentPageTemplatePath, string postPageTemplatePath, string contactPageTemplatePath, string redirectTemplatePath, string syndicationTemplatePath, string syndicationItemTemplatePath, string itemTemplatePath, string searchTemplatePath, string archiveTemplatePath, string archiveItemTemplatePath)
        {
            string contentPageTemplate = contentPageTemplatePath.ReadAllTextFromFile();
            string styleTemplate = styleTemplatePath.ReadAllTextFromFile();
            string bootstrapTemplate = bootstrapTemplatePath.ReadAllTextFromFile();
            string homePageTemplate = homePageTemplatePath.ReadAllTextFromFile();
            string postPageTemplate = postPageTemplatePath.ReadAllTextFromFile();
            string contactPageTemplate = contactPageTemplatePath.ReadAllTextFromFile();
            string redirectTemplate = redirectTemplatePath.ReadAllTextFromFile();
            string syndicationTemplate = syndicationTemplatePath.ReadAllTextFromFile();
            string syndicationItemTemplate = syndicationItemTemplatePath.ReadAllTextFromFile();
            string itemTemplate = itemTemplatePath.ReadAllTextFromFile();
            string searchTemplate = searchTemplatePath.ReadAllTextFromFile();
            string archiveTemplate = archiveTemplatePath.ReadAllTextFromFile();
            string archiveItemTemplate = archiveItemTemplatePath.ReadAllTextFromFile();

            return new List<Template>()
            {
                new Template() { Content = contentPageTemplate, TemplateType = TemplateType.ContentPage },
                new Template() { Content = postPageTemplate, TemplateType = TemplateType.PostPage },
                new Template() { Content = styleTemplate, TemplateType = TemplateType.Style },
                new Template() { Content = bootstrapTemplate, TemplateType = TemplateType.Bootstrap },
                new Template() { Content = homePageTemplate, TemplateType = TemplateType.HomePage },
                new Template() { Content = contactPageTemplate, TemplateType = TemplateType.ContactPage },
                new Template() { Content = redirectTemplate, TemplateType = TemplateType.Redirect },
                new Template() { Content = syndicationTemplate, TemplateType = TemplateType.Syndication },
                new Template() { Content = syndicationItemTemplate, TemplateType = TemplateType.SyndicationItem },
                new Template() { Content = itemTemplate, TemplateType = TemplateType.Item },
                new Template() { Content = searchTemplate, TemplateType = TemplateType.SearchPage },
                new Template() { Content = archiveTemplate, TemplateType = TemplateType.Archive },
                new Template() { Content = archiveItemTemplate, TemplateType = TemplateType.ArchiveItem }
            };
        }

    }
}
