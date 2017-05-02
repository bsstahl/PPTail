using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail
{
    public static class TemplateCollectionExtensions
    {
        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore)
        {
            string styleTemplatePath = "..\\Style.template.css";
            string bootstrapTemplatePath = "..\\bootstrap.min.css";
            string homePageTemplatePath = "..\\HomePage.template.html";
            string searchPageTemplatePath = "..\\ContentPage.template.html";
            string contentPageTemplatePath = "..\\ContentPage.template.html";
            string postPageTemplatePath = "..\\PostPage.template.html";
            string redirectTemplatePath = "..\\Redirect.template.html";
            string archiveTemplatePath = "..\\Archive.template.html";
            string archiveItemTemplatePath = "..\\ArchiveItem.template.html";
            string syndicationTemplatePath = "..\\Syndication.template.xml";
            string syndicationItemTemplatePath = "..\\SyndicationItem.template.xml";
            string contactPageTemplatePath = "..\\ContactPage.template.html";
            string itemTemplatePath = "..\\ContentItem.template.html";
            return (null as IEnumerable<Template>).Create(styleTemplatePath, bootstrapTemplatePath, homePageTemplatePath, contentPageTemplatePath, postPageTemplatePath, contactPageTemplatePath, redirectTemplatePath, syndicationTemplatePath, syndicationItemTemplatePath, itemTemplatePath, searchPageTemplatePath, archiveTemplatePath, archiveItemTemplatePath);

        }
    }
}
