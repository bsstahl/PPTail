using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using PPTail.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Interfaces;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_GenerateSidebarContent_Should
    {
        [Fact]
        public void SendTheTextBoxWidgetsContentToTheTemplateProcessor()
        {
            var widget = Enumerations.WidgetType.TextBox.CreateWidget();
            var expected = widget.Dictionary.First().Item2;
            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor.Setup(p => p.ProcessNonContentItemTemplate(
                It.IsAny<Template>(), It.IsAny<String>(), It.IsAny<String>(), 
                It.IsAny<string>(), It.IsAny<String>(), It.IsAny<String>()))
                .Returns(expected);

            var posts = new List<ContentItem>();
            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings, templateProcessor.Object);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");

            templateProcessor.Verify(p => p.ProcessNonContentItemTemplate(
                    It.Is<Template>(t => t.Content.Equals(expected) && t.TemplateType == Enumerations.TemplateType.Raw),
                    It.IsAny<String>(), It.IsAny<String>(), It.IsAny<string>(),
                    It.IsAny<String>(), It.IsAny<String>()), Times.Once);
        }

        [Fact]
        public void RenderATextBoxWidgetsContent()
        {
            var widget = Enumerations.WidgetType.TextBox.CreateWidget();
            var expected = widget.Dictionary.First().Item2;
            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var templateProcessor = new Mock<ITemplateProcessor>();
            templateProcessor.Setup(p => p.ProcessNonContentItemTemplate(
                It.IsAny<Template>(), It.IsAny<String>(), It.IsAny<String>(),
                It.IsAny<string>(), It.IsAny<String>(), It.IsAny<String>()))
                .Returns(expected);

            var posts = new List<ContentItem>();
            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings, templateProcessor.Object);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void RenderATagInATagCloudWidgetsContent()
        {
            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var container = (null as IServiceCollection).Create();

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var contentEncoder = new Mock<IContentEncoder>();
            Func<String, String> valueFunction = p => p;
            _ = contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(templates);

            _ = container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);
            _ = container.ReplaceDependency<ISettings>(settings);
            _ = container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(container);

            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");
            var expected = posts.Single().Tags.Single();

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void RenderTwoTagsInATagCloudFromASinglePost()
        {
            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var container = (null as IServiceCollection).Create();

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(templates);

            _ = container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var posts = (null as IEnumerable<ContentItem>).Create(1);

            var thisPost = posts.Single();
            String tag1 = string.Empty.GetRandom();
            String tag2 = string.Empty.GetRandom();
            thisPost.Tags = new List<string>() { tag1, tag2 };

            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(container);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");

            Assert.Contains(tag1, actual);
            Assert.Contains(tag2, actual);
        }

        [Fact]
        public void RenderTagsInATagCloudFromMultiplePosts()
        {
            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var container = (null as IServiceCollection).Create();

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");
            var posts = (null as IEnumerable<ContentItem>).Create(2);

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);

            var templateRepo = new Mock<ITemplateRepository>();
            templateRepo.Setup(r => r.GetAllTemplates())
                .Returns(templates);

            _ = container.ReplaceDependency<ITemplateRepository>(templateRepo.Object);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            String tag1 = posts.First().Tags.Single();
            String tag2 = posts.Last().Tags.Single();
            System.Diagnostics.Debug.Assert(tag1 != tag2);

            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(container);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");

            Assert.Contains(tag1, actual);
            Assert.Contains(tag2, actual);
        }

        [Fact]
        public void CallTheLinkProviderOncePerTag()
        {
            String pathToRoot = ".";

            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var pages = new List<ContentItem>();
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var tagName = posts.Single().Tags.Single();

            var linkProvider = new Mock<ILinkProvider>();
            linkProvider.Setup(l => l.GetUrl(pathToRoot, "Search", tagName)).Verifiable();

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);
            
            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var pageGen = (null as Interfaces.IPageGenerator).Create(container);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, pathToRoot);

            linkProvider.VerifyAll();
        }

        [Fact]
        public void ReturnTheOutputOfTheLinkProvider()
        {
            String pathToRoot = ".";

            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var pages = new List<ContentItem>();
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var tagName = posts.Single().Tags.Single();

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);

            var linkProvider = new Mock<ILinkProvider>();
            linkProvider.Setup(l => l.GetUrl(pathToRoot, "Search", tagName)).Returns($"http_{tagName}");

            var container = (null as IServiceCollection).Create();
            container.ReplaceDependency<ILinkProvider>(linkProvider.Object);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var pageGen = (null as Interfaces.IPageGenerator).Create(container);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, pathToRoot);

            Assert.Contains($"http_{tagName}", actual);
        }

        [Fact]
        public void RenderWidgetTitleIfShowTitleIsTrue()
        {
            var widget = Enumerations.WidgetType.TextBox.CreateWidget();
            widget.ShowTitle = true;

            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            // var siteSettings = (null as SiteSettings).Create();
            var posts = new List<ContentItem>();
            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");
            var expected = widget.Title;

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void DoNotRenderWidgetTitleIfShowTitleIsFalse()
        {
            var widget = Enumerations.WidgetType.TextBox.CreateWidget();
            widget.ShowTitle = false;

            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            // var siteSettings = (null as SiteSettings).Create();
            var posts = new List<ContentItem>();
            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");
            var expected = widget.Title;

            Assert.DoesNotContain(expected, actual);
        }
    }
}
