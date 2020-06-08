using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.FileSystem.Test
{
    public class ReadRepository_GetAllTemplates_Should
    {
        [Theory]
        [InlineData("Style.template.css")]
        [InlineData("bootstrap.min.css")]
        [InlineData("HomePage.template.html")]
        [InlineData("ContentPage.template.html")]
        [InlineData("SearchPage.template.html")]
        [InlineData("PostPage.template.html")]
        [InlineData("Redirect.template.html")]
        [InlineData("Archive.template.html")]
        [InlineData("ArchiveItem.template.html")]
        [InlineData("Syndication.template.xml")]
        [InlineData("SyndicationItem.template.xml")]
        [InlineData("ContactPage.template.html")]
        [InlineData("ContentItem.template.html")]
        public void RequestATemplateFromTheProperLocation(string templateFilename)
        {
            templateFilename.ExecuteTemplateRetrievalTest();
        }

        [Theory]
        [InlineData("Style.template.css", TemplateType.Style)]
        [InlineData("bootstrap.min.css", TemplateType.Bootstrap)]
        [InlineData("HomePage.template.html", TemplateType.HomePage)]
        [InlineData("ContentPage.template.html", TemplateType.ContentPage)]
        [InlineData("SearchPage.template.html", TemplateType.SearchPage)]
        [InlineData("PostPage.template.html", TemplateType.PostPage)]
        [InlineData("Redirect.template.html", TemplateType.Redirect)]
        [InlineData("Archive.template.html", TemplateType.Archive)]
        [InlineData("ArchiveItem.template.html", TemplateType.ArchiveItem)]
        [InlineData("Syndication.template.xml", TemplateType.Syndication)]
        [InlineData("SyndicationItem.template.xml", TemplateType.SyndicationItem)]
        [InlineData("ContactPage.template.html", TemplateType.ContactPage)]
        [InlineData("ContentItem.template.html", TemplateType.Item)]
        public void RetrieveTheProperTemplateContent(string templateFilename, Enumerations.TemplateType templateType)
        {
            templateFilename.ExecuteTemplateContentTest(templateType);
        }

    }
}
