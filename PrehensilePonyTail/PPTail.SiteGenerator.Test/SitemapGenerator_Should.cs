using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using PPTail.Exceptions;
using Xunit;

namespace PPTail.SiteGenerator.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SitemapGenerator_Should
    {
        private static readonly XNamespace _ns = SitemapGenerator.XmlNamespace;

        private static IEnumerable<String> GetLocs(String xml)
        {
            return XDocument.Parse(xml)
                .Root!
                .Elements(_ns + "url")
                .Select(u => u.Element(_ns + "loc")!.Value)
                .ToList();
        }

        [Theory]
        [InlineData("https://example.com", "https://example.com/")]
        [InlineData("https://example.com/", "https://example.com/")]
        [InlineData("http://example.com/blog", "http://example.com/blog/")]
        [InlineData("https://example.com/blog/", "https://example.com/blog/")]
        [InlineData("https://example.com/Deep/Sub/Dir", "https://example.com/Deep/Sub/Dir/")]
        [InlineData("  https://example.com/blog  ", "https://example.com/blog/")]
        [InlineData("https://example.com:8443/blog", "https://example.com:8443/blog/")]
        [InlineData("https://example.com/my%20blog", "https://example.com/my%20blog/")]
        [InlineData("https://example.com/my%7Eblog", "https://example.com/my%7Eblog/")]
        [InlineData("https://example.com/café", "https://example.com/caf%C3%A9/")]
        public void NormalizeTheBaseUrl(String baseUrl, String expected)
        {
            Assert.Equal(expected, SitemapGenerator.NormalizeBaseUrl(baseUrl));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("example.com")]
        [InlineData("/blog/")]
        [InlineData("ftp://example.com/")]
        [InlineData("file:///c:/blog/")]
        [InlineData("mailto:someone@example.com")]
        [InlineData("https://user@example.com/")]
        [InlineData("https://example.com/?page=1")]
        [InlineData("https://example.com/#top")]
        [InlineData("https://example.com/my blog/")]
        [InlineData("https://example.com/../blog/")]
        [InlineData("https:example.com")]
        public void RejectInvalidBaseUrls(String? baseUrl)
        {
            Assert.Throws<SitemapException>(() => SitemapGenerator.NormalizeBaseUrl(baseUrl));
        }

        [Fact]
        public void ProduceAValidSitemapDocument()
        {
            var actual = SitemapGenerator.Generate("https://example.com/", new[] { "./index.html", "./archive.html" });

            var doc = XDocument.Parse(actual);
            Assert.Equal(_ns + "urlset", doc.Root!.Name);
            Assert.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>", actual);
            Assert.All(doc.Root.Elements(), e => Assert.Equal(_ns + "url", e.Name));
        }

        [Fact]
        public void NotEmitLastmodChangefreqOrPriority()
        {
            var actual = SitemapGenerator.Generate("https://example.com/", new[] { "./index.html", "Posts/a.html", "Pages/b.html" });

            var doc = XDocument.Parse(actual);
            var childNames = doc.Root!.Elements(_ns + "url").SelectMany(u => u.Elements()).Select(e => e.Name.LocalName).Distinct();
            Assert.Equal(new[] { "loc" }, childNames);
            Assert.DoesNotContain("lastmod", actual);
            Assert.DoesNotContain("changefreq", actual);
            Assert.DoesNotContain("priority", actual);
        }

        [Fact]
        public void RepresentTheHomepageAsTheBaseUrl()
        {
            var actual = GetLocs(SitemapGenerator.Generate("https://example.com/blog", new[] { "./index.html" }));
            Assert.Equal(new[] { "https://example.com/blog/" }, actual);
        }

        [Fact]
        public void PreserveTheDeploymentSubdirectory()
        {
            var actual = GetLocs(SitemapGenerator.Generate("https://example.com/My/Blog", new[] { "./index.html", "Posts\\My-Post.html", "Pages/About.html" }));

            Assert.Equal(new[]
            {
                "https://example.com/My/Blog/",
                "https://example.com/My/Blog/Pages/About.html",
                "https://example.com/My/Blog/Posts/My-Post.html"
            }, actual);
        }

        [Fact]
        public void ProduceTheSameUrlsWithOrWithoutATrailingSlash()
        {
            var paths = new[] { "./index.html", "./archive.html", "Posts/a.html" };
            var withSlash = SitemapGenerator.Generate("https://example.com/blog/", paths);
            var withoutSlash = SitemapGenerator.Generate("https://example.com/blog", paths);
            Assert.Equal(withSlash, withoutSlash);
        }

        [Fact]
        public void UseTheActualOutputExtension()
        {
            var actual = GetLocs(SitemapGenerator.Generate("https://example.com/", new[] { "Posts/my-post.htm", "Pages/page.aspx" }));
            Assert.Contains("https://example.com/Posts/my-post.htm", actual);
            Assert.Contains("https://example.com/Pages/page.aspx", actual);
        }

        [Theory]
        [InlineData("Posts/my post.html", "https://example.com/Posts/my%20post.html")]
        [InlineData("Posts/café.html", "https://example.com/Posts/caf%C3%A9.html")]
        [InlineData("Posts/caf%C3%A9.html", "https://example.com/Posts/caf%C3%A9.html")]
        [InlineData("Posts/already%20encoded.html", "https://example.com/Posts/already%20encoded.html")]
        [InlineData("Posts/100%.html", "https://example.com/Posts/100%25.html")]
        [InlineData("Posts/a?b#c.html", "https://example.com/Posts/a%3Fb%23c.html")]
        [InlineData("Posts/😀.html", "https://example.com/Posts/%F0%9F%98%80.html")]
        [InlineData("Posts/MixedCase.html", "https://example.com/Posts/MixedCase.html")]
        public void EncodeUrlsCorrectly(String relativePath, String expected)
        {
            var actual = GetLocs(SitemapGenerator.Generate("https://example.com/", new[] { relativePath }));
            Assert.Equal(expected, actual.Single());
        }

        [Fact]
        public void EscapeXmlSpecialCharacters()
        {
            var actual = SitemapGenerator.Generate("https://example.com/", new[] { "Posts/rock&roll's.html" });

            Assert.Contains("rock&amp;roll's.html", actual);
            Assert.Equal("https://example.com/Posts/rock&roll's.html", GetLocs(actual).Single());
        }

        [Fact]
        public void NotIncludeDuplicateUrls()
        {
            var paths = new[] { "./index.html", "index.html", "Posts/a.html", "Posts\\a.html", "./Posts/a.html", "./archive.html", "archive.html" };
            var actual = GetLocs(SitemapGenerator.Generate("https://example.com/", paths));

            Assert.Equal(3, actual.Count());
            Assert.Equal(actual.Distinct().Count(), actual.Count());
        }

        [Fact]
        public void OrderUrlsDeterministically()
        {
            var paths = new[] { "Posts/b.html", "./contact.html", "Pages/z.html", "./index.html", "Posts/a.html", "./archive.html" };
            var first = SitemapGenerator.Generate("https://example.com/", paths);
            var second = SitemapGenerator.Generate("https://example.com/", paths.Reverse());

            Assert.Equal(first, second);
            Assert.Equal(new[]
            {
                "https://example.com/",
                "https://example.com/Pages/z.html",
                "https://example.com/Posts/a.html",
                "https://example.com/Posts/b.html",
                "https://example.com/archive.html",
                "https://example.com/contact.html"
            }, GetLocs(first));
        }

        [Theory]
        [InlineData("../outside.html")]
        [InlineData("Posts/../../outside.html")]
        [InlineData("Posts//a.html")]
        [InlineData("")]
        public void RejectPathsThatEscapeTheSiteRoot(String relativePath)
        {
            Assert.Throws<SitemapException>(() => SitemapGenerator.Generate("https://example.com/", new[] { relativePath }));
        }

        [Fact]
        public void FailIfTheUrlLimitIsExceeded()
        {
            var paths = Enumerable.Range(0, SitemapGenerator.MaxUrlCount + 1).Select(i => $"Posts/{i}.html");
            Assert.Throws<SitemapException>(() => SitemapGenerator.Generate("https://example.com/", paths));
        }

        [Fact]
        public void ProduceAValidSitemapForAnEmptyList()
        {
            var actual = SitemapGenerator.Generate("https://example.com/", Array.Empty<String>());
            Assert.Empty(GetLocs(actual));
        }

        [Theory]
        [InlineData("sitemap.xml", true)]
        [InlineData("./sitemap.xml", true)]
        [InlineData("/sitemap.xml", true)]
        [InlineData("SiteMap.XML", true)]
        [InlineData("Files/sitemap.xml", false)]
        [InlineData("sitemap.xml.bak", false)]
        [InlineData(null, false)]
        public void IdentifyTheSitemapPath(String? relativePath, bool expected)
        {
            Assert.Equal(expected, SitemapGenerator.IsSitemapPath(relativePath));
        }
    }
}
