using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail
{
    public static class TemplateCollectionExtensions
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
    }
}
