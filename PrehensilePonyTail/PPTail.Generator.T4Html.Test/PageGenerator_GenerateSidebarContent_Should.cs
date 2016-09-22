using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

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
            var actual = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);
            var expected = widget.Dictionary.First().Item2;

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void RenderATagInATagCloudWidgetsContent()
        {
            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var siteSettings = (null as SiteSettings).Create();
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);
            var expected = posts.Single().Tags.Single();

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void RenderTwoTagsInATagCloudFromASinglePost()
        {
            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var siteSettings = (null as SiteSettings).Create();
            var posts = (null as IEnumerable<ContentItem>).Create(1);

            var thisPost = posts.Single();
            string tag1 = string.Empty.GetRandom();
            string tag2 = string.Empty.GetRandom();
            thisPost.Tags = new List<string>() { tag1, tag2 };

            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);

            Assert.Contains(tag1, actual);
            Assert.Contains(tag2, actual);
        }

        [Fact]
        public void RenderTagsInATagCloudFromMultiplePosts()
        {
            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var siteSettings = (null as SiteSettings).Create();
            var posts = (null as IEnumerable<ContentItem>).Create(2);

            string tag1 = posts.First().Tags.Single();
            string tag2 = posts.Last().Tags.Single();
            System.Diagnostics.Debug.Assert(tag1 != tag2);

            var pages = new List<ContentItem>();

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);

            Assert.Contains(tag1, actual);
            Assert.Contains(tag2, actual);
        }

        [Fact]
        public void RendersTagsLinkedToTheTagPage()
        {
            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var siteSettings = (null as SiteSettings).Create();
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var tagName = posts.Single().Tags.Single();
            var extension = settings.outputFileExtension;

            var pages = new List<ContentItem>();

            string expected = $"href=\"/tags/{tagName}.{extension}\"";

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);

            Assert.Contains(expected, actual);
        }

        [Fact]
        public void RendersTagLinksWithoutSpaces()
        {
            var widget = Enumerations.WidgetType.Tag_cloud.CreateWidget();
            var widgets = new List<Widget>() { widget };

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            var settings = (null as Settings).CreateDefault("yyyy-MM-dd");

            var siteSettings = (null as SiteSettings).Create();
            var posts = (null as IEnumerable<ContentItem>).Create(1);
            var thisPost = posts.Single();
            var tagValue = $"{String.Empty.GetRandom()} {string.Empty.GetRandom()}";
            thisPost.Tags = new List<string>() { tagValue };

            var tagName = tagValue.Replace(" ", "_");
            var extension = settings.outputFileExtension;

            var pages = new List<ContentItem>();

            string expected = $"href=\"/tags/{tagName}.{extension}\"";

            var pageGen = (null as Interfaces.IPageGenerator).Create(templates, settings);
            var actual = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);

            Assert.Contains(expected, actual);
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
            var actual = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);
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
            var actual = pageGen.GenerateSidebarContent(settings, siteSettings, posts, pages, widgets);
            var expected = widget.Title;

            Assert.DoesNotContain(expected, actual);
        }
    }
}
