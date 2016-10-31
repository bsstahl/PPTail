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
        public void RenderATextBoxWidgetsContent()
        {
            var widget = Enumerations.WidgetType.TextBox.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var siteSettings = (null as SiteSettings).Create();
            var posts = new List<ContentItem>();
            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");
            var expected = widget.Dictionary.First().Item2;

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
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);

            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

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

            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            var posts = (null as IEnumerable<ContentItem>).Create(1);

            var thisPost = posts.Single();
            string tag1 = string.Empty.GetRandom();
            string tag2 = string.Empty.GetRandom();
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

            container.ReplaceDependency<IEnumerable<Template>>(templates);
            container.ReplaceDependency<ISettings>(settings);
            container.ReplaceDependency<IContentEncoder>(contentEncoder.Object);

            string tag1 = posts.First().Tags.Single();
            string tag2 = posts.Last().Tags.Single();
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
            string pathToRoot = ".";

            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var pages = new List<ContentItem>();
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var tagName = posts.Single().Tags.Single();

            var linkProvider = new Mock<ILinkProvider>();
            linkProvider.Setup(l => l.GetUrl(pathToRoot, "search", tagName)).Verifiable();

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
            string pathToRoot = ".";

            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var pages = new List<ContentItem>();
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var tagName = posts.Single().Tags.Single();

            var contentEncoder = new Mock<IContentEncoder>();
            Func<string, string> valueFunction = p => p;
            contentEncoder.Setup(c => c.UrlEncode(It.IsAny<string>())).Returns(valueFunction);

            var linkProvider = new Mock<ILinkProvider>();
            linkProvider.Setup(l => l.GetUrl(pathToRoot, "search", tagName)).Returns($"http_{tagName}");

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

            var siteSettings = (null as SiteSettings).Create();
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

            var siteSettings = (null as SiteSettings).Create();
            var posts = new List<ContentItem>();
            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(posts, pages, widgets, ".");
            var expected = widget.Title;

            Assert.DoesNotContain(expected, actual);
        }
    }
}
