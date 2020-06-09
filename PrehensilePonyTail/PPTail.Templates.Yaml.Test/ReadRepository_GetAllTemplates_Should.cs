using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.IO;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.Yaml.Test
{
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
        public void RequestTheTemplateFromTheProperLocation(string templateFilename)
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
        public void RetrieveTheProperTemplateContentEvenIfNotYaml()
        {
            string expected = "body {\r\n font-size: 0.8rem;\r\n}\r\n\r\nimg {\r\n max-width: 100%;\r\n height: auto; \r\n}\r\n\r\n.h2, h2 {\r\n font-size: 1.25rem;\r\n font-weight: bold;\r\n}\r\n\r\n.h3, h3 {\r\n font-size: 1.25rem;\r\n}\r\n\r\n.h4, h4 {\r\n font-size: 1.00rem;\r\n}\r\n\r\n.h5, h5 {\r\n font-size: 0.90rem;\r\n}\r\n\r\na, a:visited {\r\n color: #909090;\r\n}\r\n\r\n.widget {\r\n z-index: 1;\r\n min-width: 1px;\r\n}\r\n\r\n .widget h4 {\r\n font-size: 18px;\r\n margin: 20px 0px 10px 0px;\r\n font-weight: normal;\r\n }\r\n\r\n .widget .content {\r\n margin: 0px 0px 20px 0px;\r\n padding: 0px;\r\n }\r\n\r\n .widget .delete {\r\n margin-left: 10px;\r\n float: right;\r\n font-family: Arial,Sans-Serif;\r\n font-size: 11px;\r\n }\r\n\r\n .widget .edit {\r\n float: right;\r\n }\r\n\r\n.tagcloud li {\r\n display: inline;\r\n margin-right: 5px;\r\n}\r\n\r\n.tagcloud ul {\r\n padding: 0px;\r\n}\r\n\r\n.tagcloud a.biggest {\r\n font-size: 20px;\r\n}\r\n\r\n.tagcloud a.big {\r\n font-size: 17px;\r\n}\r\n\r\n.tagcloud a.medium {\r\n font-size: 13px;\r\n}\r\n\r\n.tagcloud a.small {\r\n font-size: 12px;\r\n}\r\n\r\n.tagcloud a.smallest {\r\n font-size: 10px;\r\n}\r\n\r\nblockquote {\r\n position: relative;\r\n display: -ms-flexbox;\r\n display: flex;\r\n -ms-flex-direction: column;\r\n flex-direction: column;\r\n min-width: 0;\r\n word-wrap: break-word;\r\n background-color: #6c757d47;\r\n background-clip: border-box;\r\n border: 2px solid rgba(0, 0, 0, 0.125);\r\n border-radius: 1.0rem;\r\n padding: 0.5rem;\r\n font-weight: bold;\r\n font-family: cursive;\r\n}\r\n\r\ncode {\r\n font-family: monospace;\r\n background-color: #f7f7f7;\r\n padding-top: 2px;\r\n padding-bottom: 2px;\r\n padding-left: 10px;\r\n display:block;\r\n border:solid 1px #909090\r\n}\r\n\r\ntable tr .pubdate {\r\n width: 33%;\r\n}";
            "Style.template.css".ExecuteTemplateContentTest(TemplateType.Style, expected, expected);
        }
    }
}
