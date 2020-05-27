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
        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore, String rootTemplatePath)
        {
            var styleTemplatePath = rootTemplatePath.CombinePaths("Style.template.css");
            var bootstrapTemplatePath = rootTemplatePath.CombinePaths("bootstrap.min.css");
            var homePageTemplatePath = rootTemplatePath.CombinePaths("HomePage.template.html");
            var searchPageTemplatePath = rootTemplatePath.CombinePaths("ContentPage.template.html");
            var contentPageTemplatePath = rootTemplatePath.CombinePaths("ContentPage.template.html");
            var postPageTemplatePath = rootTemplatePath.CombinePaths("PostPage.template.html");
            var redirectTemplatePath = rootTemplatePath.CombinePaths("Redirect.template.html");
            var archiveTemplatePath = rootTemplatePath.CombinePaths("Archive.template.html");
            var archiveItemTemplatePath = rootTemplatePath.CombinePaths("ArchiveItem.template.html");
            var syndicationTemplatePath = rootTemplatePath.CombinePaths("Syndication.template.xml");
            var syndicationItemTemplatePath = rootTemplatePath.CombinePaths("SyndicationItem.template.xml");
            var contactPageTemplatePath = rootTemplatePath.CombinePaths("ContactPage.template.html");
            var itemTemplatePath = rootTemplatePath.CombinePaths("ContentItem.template.html");
            return (null as IEnumerable<Template>).Create(styleTemplatePath, bootstrapTemplatePath, homePageTemplatePath, contentPageTemplatePath, postPageTemplatePath, contactPageTemplatePath, redirectTemplatePath, syndicationTemplatePath, syndicationItemTemplatePath, itemTemplatePath, searchPageTemplatePath, archiveTemplatePath, archiveItemTemplatePath);

        }

        public static IEnumerable<Template> Create(this IEnumerable<Template> ignore, String styleTemplatePath, String bootstrapTemplatePath, String homePageTemplatePath, String contentPageTemplatePath, String postPageTemplatePath, String contactPageTemplatePath, String redirectTemplatePath, String syndicationTemplatePath, String syndicationItemTemplatePath, String itemTemplatePath, String searchTemplatePath, String archiveTemplatePath, String archiveItemTemplatePath)
        {
            var contentPageTemplate = contentPageTemplatePath.ReadAllTextFromFile();
            var styleTemplate = styleTemplatePath.ReadAllTextFromFile();
            var bootstrapTemplate = bootstrapTemplatePath.ReadAllTextFromFile();
            var homePageTemplate = homePageTemplatePath.ReadAllTextFromFile();
            var postPageTemplate = postPageTemplatePath.ReadAllTextFromFile();
            var contactPageTemplate = contactPageTemplatePath.ReadAllTextFromFile();
            var redirectTemplate = redirectTemplatePath.ReadAllTextFromFile();
            var syndicationTemplate = syndicationTemplatePath.ReadAllTextFromFile();
            var syndicationItemTemplate = syndicationItemTemplatePath.ReadAllTextFromFile();
            var itemTemplate = itemTemplatePath.ReadAllTextFromFile();
            var searchTemplate = searchTemplatePath.ReadAllTextFromFile();
            var archiveTemplate = archiveTemplatePath.ReadAllTextFromFile();
            var archiveItemTemplate = archiveItemTemplatePath.ReadAllTextFromFile();

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
