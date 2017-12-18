using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PPTail
{
    public static class TemplateCollectionExtensions
    {
        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore, string rootTemplatePath)
        {
            string styleTemplatePath = Path.Combine(rootTemplatePath, ".\\Style.template.css");
            string bootstrapTemplatePath = Path.Combine(rootTemplatePath, ".\\bootstrap.min.css");
            string homePageTemplatePath = Path.Combine(rootTemplatePath, ".\\HomePage.template.html");
            string searchPageTemplatePath = Path.Combine(rootTemplatePath, ".\\ContentPage.template.html");
            string contentPageTemplatePath = Path.Combine(rootTemplatePath, ".\\ContentPage.template.html");
            string postPageTemplatePath = Path.Combine(rootTemplatePath, ".\\PostPage.template.html");
            string redirectTemplatePath = Path.Combine(rootTemplatePath, ".\\Redirect.template.html");
            string archiveTemplatePath = Path.Combine(rootTemplatePath, ".\\Archive.template.html");
            string archiveItemTemplatePath = Path.Combine(rootTemplatePath, ".\\ArchiveItem.template.html");
            string syndicationTemplatePath = Path.Combine(rootTemplatePath, ".\\Syndication.template.xml");
            string syndicationItemTemplatePath = Path.Combine(rootTemplatePath, ".\\SyndicationItem.template.xml");
            string contactPageTemplatePath = Path.Combine(rootTemplatePath, ".\\ContactPage.template.html");
            string itemTemplatePath = Path.Combine(rootTemplatePath, ".\\ContentItem.template.html");

            return (null as IEnumerable<Template>).Create(styleTemplatePath, bootstrapTemplatePath, homePageTemplatePath, contentPageTemplatePath, postPageTemplatePath, contactPageTemplatePath, redirectTemplatePath, syndicationTemplatePath, syndicationItemTemplatePath, itemTemplatePath, searchPageTemplatePath, archiveTemplatePath, archiveItemTemplatePath);

        }
    }
}
