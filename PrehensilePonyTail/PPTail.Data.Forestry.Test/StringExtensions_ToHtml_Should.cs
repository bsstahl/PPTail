using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Interfaces;
using System;
using Xunit;

namespace PPTail.Data.Forestry.Test
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
            const String markdown = "```csharp\r\nConsole.WriteLine(\"Hello\");\r\n```";

            var target = CreateRepository();

            var actual = markdown.ToHtml(target.MarkdownPipeline);

            Assert.Contains("<pre><code class=\"language-csharp\">", actual);
            Assert.Contains("Console.WriteLine(&quot;Hello&quot;);", actual);
            Assert.DoesNotContain("class=\"mermaid\"", actual);
        }

        private static Repository CreateRepository()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IFile>(Mock.Of<IFile>());
            services.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            return new Repository(services.BuildServiceProvider(), $"Provider=Test;FilePath=c:\\{Guid.NewGuid()}");
        }
    }
}
