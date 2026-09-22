using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Interfaces;
using System;
using Xunit;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class StringExtensions_ToHtml_Should
    {
        [Fact]
        public void ConvertMermaidFencedCodeBlocksToMermaidContainers()
        {
            const String markdown = "```mermaid\r\ngraph TD\r\n    A-->B\r\n```";

            var target = CreateRepository();

            var actual = markdown.ToHtml(target.MarkdownPipeline);

            Assert.Contains("<pre class=\"mermaid\">", actual);
            Assert.Contains("graph TD", actual);
            Assert.Contains("A--&gt;B", actual);
            Assert.DoesNotContain("language-mermaid", actual);
        }

        [Fact]
        public void PreserveOrdinaryFencedCodeBlocks()
        {
            const String markdown = "```json\r\n{\"enabled\":true}\r\n```";

            var target = CreateRepository();

            var actual = markdown.ToHtml(target.MarkdownPipeline);

            Assert.Contains("<pre><code class=\"language-json\">", actual);
            Assert.Contains("{&quot;enabled&quot;:true}", actual);
            Assert.DoesNotContain("class=\"mermaid\"", actual);
        }

        private static YamlRepository CreateRepository()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IFile>(Mock.Of<IFile>());
            services.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            return new YamlRepository(services.BuildServiceProvider(), $"Provider=Test;FilePath=c:\\{Guid.NewGuid()}");
        }
    }
}
