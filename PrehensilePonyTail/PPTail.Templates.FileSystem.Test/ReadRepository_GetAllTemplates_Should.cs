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
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ReadRepository_GetAllTemplates_Should
    {
        [Theory]
        [InlineData("Style.template.css")]
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

        [Fact]
        public void RetrieveEachTemplateExactlyOnceRegardlessOfHowManyTimesRequested()
        {
            string templatePath = string.Empty.GetRandom();

            var mockFileService = new Mock<IFile>();
            mockFileService.ConfigureTemplate(templatePath, "Style.template.css");
            mockFileService.ConfigureTemplate(templatePath, "HomePage.template.html");
            mockFileService.ConfigureTemplate(templatePath, "ContentPage.template.html");
            mockFileService.ConfigureTemplate(templatePath, "SearchPage.template.html");
            mockFileService.ConfigureTemplate(templatePath, "PostPage.template.html");
            mockFileService.ConfigureTemplate(templatePath, "Redirect.template.html");
            mockFileService.ConfigureTemplate(templatePath, "Archive.template.html");
            mockFileService.ConfigureTemplate(templatePath, "ArchiveItem.template.html");
            mockFileService.ConfigureTemplate(templatePath, "Syndication.template.xml");
            mockFileService.ConfigureTemplate(templatePath, "SyndicationItem.template.xml");
            mockFileService.ConfigureTemplate(templatePath, "ContactPage.template.html");
            mockFileService.ConfigureTemplate(templatePath, "ContentItem.template.html");

            var serviceProvider = new ServiceCollection()
                .AddFileService(mockFileService)
                .BuildServiceProvider();

            var target = new FileSystem.ReadRepository(serviceProvider, templatePath);
            var actual = target.GetAllTemplates();
            actual = target.GetAllTemplates();

            mockFileService.VerifyOnce(templatePath, "Style.template.css");
            mockFileService.VerifyOnce(templatePath, "HomePage.template.html");
            mockFileService.VerifyOnce(templatePath, "ContentPage.template.html");
            mockFileService.VerifyOnce(templatePath, "SearchPage.template.html");
            mockFileService.VerifyOnce(templatePath, "PostPage.template.html");
            mockFileService.VerifyOnce(templatePath, "Redirect.template.html");
            mockFileService.VerifyOnce(templatePath, "Archive.template.html");
            mockFileService.VerifyOnce(templatePath, "ArchiveItem.template.html");
            mockFileService.VerifyOnce(templatePath, "Syndication.template.xml");
            mockFileService.VerifyOnce(templatePath, "SyndicationItem.template.xml");
            mockFileService.VerifyOnce(templatePath, "ContactPage.template.html");
            mockFileService.VerifyOnce(templatePath, "ContentItem.template.html");
        }
    }
}
