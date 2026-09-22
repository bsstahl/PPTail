using System;
using System.Collections.Generic;
using Xunit;
using TestHelperExtensions;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;

namespace PPTail.Generator.Template.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TemplateProcessor_ProcessContentItemTemplate_Should
    {
        [Fact]
        public void IncludeMermaidAssetsWhenTheContentContainsMermaidMarkup()
        {
            const String mermaidContent = "<pre class=\"mermaid\">graph TD\nA--&gt;B</pre>";
            const String templateContent = "<html><body>{MermaidAssets}{Content}</body></html>";

            var template = new PPTail.Entities.Template() { Content = templateContent, TemplateType = Enumerations.TemplateType.PostPage };
            var contentItem = (null as ContentItem).Create(string.Empty.GetRandom(), new List<Guid>() { Guid.NewGuid() }, mermaidContent, string.Empty.GetRandom(), true, DateTime.UtcNow, DateTime.UtcNow, string.Empty.GetRandom(), new List<string>() { string.Empty.GetRandom() }, string.Empty.GetRandom(), string.Empty.GetRandom());

            var container = (null as IServiceCollection).Create();
            var target = (null as ITemplateProcessor).Create(container);

            var actual = target.ProcessContentItemTemplate(template, contentItem, string.Empty, string.Empty, string.Empty, false);

            Assert.Contains("cdnjs.cloudflare.com/ajax/libs/mermaid", actual);
            Assert.Contains("mermaid.run()", actual);
        }

        [Fact]
        public void RemoveTheMermaidAssetsPlaceholderWhenMermaidMarkupIsAbsent()
        {
            const String templateContent = "<html><body>{MermaidAssets}{Content}</body></html>";
            const String pageContent = "<pre><code class=\"language-csharp\">Console.WriteLine(&quot;Hi&quot;);</code></pre>";

            var template = new PPTail.Entities.Template() { Content = templateContent, TemplateType = Enumerations.TemplateType.ContactPage };

            var container = (null as IServiceCollection).Create();
            var target = (null as ITemplateProcessor).Create(container);

            var actual = target.ProcessNonContentItemTemplate(template, string.Empty, string.Empty, pageContent, string.Empty.GetRandom(), string.Empty);

            Assert.DoesNotContain("{MermaidAssets}", actual);
            Assert.DoesNotContain("cdnjs.cloudflare.com/ajax/libs/mermaid", actual);
        }

        [Fact]
        public void IncludeMermaidAssetsWhenNonContentPagesContainMermaidMarkup()
        {
            const String templateContent = "<html><body>{MermaidAssets}{Content}</body></html>";
            const String pageContent = "<pre class=\"mermaid\">graph TD\nA--&gt;B</pre>";

            var template = new PPTail.Entities.Template() { Content = templateContent, TemplateType = Enumerations.TemplateType.HomePage };

            var container = (null as IServiceCollection).Create();
            var target = (null as ITemplateProcessor).Create(container);

            var actual = target.ProcessNonContentItemTemplate(template, string.Empty, string.Empty, pageContent, string.Empty.GetRandom(), string.Empty);

            Assert.Contains("cdnjs.cloudflare.com/ajax/libs/mermaid", actual);
            Assert.Contains("mermaid.run()", actual);
        }
    }
}
