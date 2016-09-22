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
            var actual = pageGen.GenerateSidebarContent(siteSettings, posts, pages, widgets);
            var expected = widget.Dictionary.First().Item2;

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
            var actual = pageGen.GenerateSidebarContent(siteSettings, posts, pages, widgets);
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
            var actual = pageGen.GenerateSidebarContent(siteSettings, posts, pages, widgets);
            var expected = widget.Title;

            Assert.DoesNotContain(expected, actual);
        }
    }
}
