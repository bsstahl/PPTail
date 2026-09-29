using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Builders;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Interfaces;
using Xunit;

namespace PPTail.SiteGenerator.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Builder_Build_Sitemap_Should
    {
        private const String _baseUrl = "https://example.com/blog/";
        private static readonly XNamespace _ns = SitemapGenerator.XmlNamespace;

        private static (IServiceCollection container, Mock<IContentRepository> contentRepo) CreateContainer(BuildSettings? buildSettings, SiteSettings? siteSettings = null)
        {
            var contentRepo = new Mock<IContentRepository>();
            contentRepo.Setup(r => r.GetSiteSettings()).Returns(siteSettings ?? new SiteSettings());

            var container = (null as IServiceCollection).Create(contentRepo.Object);
            if (buildSettings is not null)
                container.AddSingleton(buildSettings);

            return (container, contentRepo);
        }

        private static IEnumerable<SiteFile> Build(IServiceCollection container)
            => (null as Builder).Create(container).Build();

        private static IEnumerable<String> GetSitemapLocs(IEnumerable<SiteFile> files)
        {
            var sitemap = files.Single(f => SitemapGenerator.IsSitemapPath(f.RelativeFilePath));
            return XDocument.Parse(sitemap.Content).Root!
                .Elements(_ns + "url")
                .Select(u => u.Element(_ns + "loc")!.Value)
                .ToList();
        }

        private static ContentItem CreateItem(String slug, bool isPublished = true, bool buildIfNotPublished = false, bool excludeFromSitemap = false)
        {
            var item = (null as ContentItem).Create();
            item.Slug = slug;
            item.IsPublished = isPublished;
            item.BuildIfNotPublished = buildIfNotPublished;
            item.ExcludeFromSitemap = excludeFromSitemap;
            return item;
        }

        [Theory]
        [InlineData(SitemapMode.Auto)]
        [InlineData(SitemapMode.Enabled)]
        public void WriteExactlyOneSitemapAtTheOutputRootWhenConfigured(SitemapMode mode)
        {
            var (container, _) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl, SitemapMode = mode });

            var actual = Build(container);

            var sitemap = actual.Single(f => SitemapGenerator.IsSitemapPath(f.RelativeFilePath));
            Assert.Equal("./sitemap.xml", sitemap.RelativeFilePath);
            Assert.Equal(TemplateType.Sitemap, sitemap.SourceTemplateType);
            Assert.False(sitemap.IsBase64Encoded);
        }

        [Fact]
        public void IncludeTheBuiltInPagesForAnEmptyBlog()
        {
            var (container, _) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl });

            var actual = GetSitemapLocs(Build(container));

            Assert.Equal(new[]
            {
                "https://example.com/blog/",
                "https://example.com/blog/archive.html",
                "https://example.com/blog/contact.html"
            }, actual);
        }

        [Fact]
        public void IncludeOnlyPublicCanonicalPages()
        {
            var siteSettings = new SiteSettings() { OutputFileExtension = "htm" };
            var (container, contentRepo) = CreateContainer(new BuildSettings() { PublicBaseUrl = "https://example.com/blog" }, siteSettings);

            var posts = new List<ContentItem>()
            {
                CreateItem("published-post"),
                CreateItem("preview-post", isPublished: false, buildIfNotPublished: true),
                CreateItem("draft-post", isPublished: false),
                CreateItem("hidden-post", excludeFromSitemap: true)
            };
            var pages = new List<ContentItem>()
            {
                CreateItem("about"),
                CreateItem("draft-page", isPublished: false),
                CreateItem("unpublished-posts", excludeFromSitemap: true)
            };
            contentRepo.Setup(r => r.GetAllPosts()).Returns(posts);
            contentRepo.Setup(r => r.GetAllPages()).Returns(pages);

            var files = Build(container);
            var actual = GetSitemapLocs(files);

            Assert.Equal(new[]
            {
                "https://example.com/blog/",
                "https://example.com/blog/Pages/about.htm",
                "https://example.com/blog/Posts/published-post.htm",
                "https://example.com/blog/archive.html",
                "https://example.com/blog/contact.html"
            }, actual);

            // Excluded content types are still generated, just not listed in the sitemap
            Assert.Contains(files, f => f.SourceTemplateType == TemplateType.Redirect);
            Assert.Contains(files, f => f.SourceTemplateType == TemplateType.SearchPage);
            Assert.Contains(files, f => f.SourceTemplateType == TemplateType.Syndication);
            Assert.Contains(files, f => f.RelativeFilePath.Replace('\\', '/') == "Posts/preview-post.htm");
            Assert.DoesNotContain(actual, u => u.Contains("Permalinks") || u.Contains("Search") || u.Contains("syndication") || u.Contains(".css"));
        }

        [Fact]
        public void ExcludeTheGeneratedUnpublishedPostsPage()
        {
            var (container, contentRepo) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl });

            var preview = CreateItem("wip-post", isPublished: false, buildIfNotPublished: true);
            var pages = new List<ContentItem>();
            contentRepo.Setup(r => r.GetAllPosts()).Returns(new[] { preview });
            contentRepo.Setup(r => r.GetAllPages()).Returns(() => pages);
            contentRepo.Setup(r => r.AddPages(It.IsAny<IEnumerable<ContentItem>>()))
                .Callback<IEnumerable<ContentItem>>(p => pages.AddRange(p));
            container.AddScoped<IContentItemGenerator>(c => new PPTail.Content.UnpublishedPosts.Generator(contentRepo.Object));

            var files = Build(container);
            var actual = GetSitemapLocs(files);

            Assert.Contains(files, f => f.RelativeFilePath == "Pages/unpublished-posts.html");
            Assert.DoesNotContain(actual, u => u.Contains("unpublished-posts"));
            Assert.DoesNotContain(actual, u => u.Contains("wip-post"));
        }

        [Fact]
        public void NotIncludeDuplicateUrlsForDuplicateSlugs()
        {
            var (container, contentRepo) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl });
            contentRepo.Setup(r => r.GetAllPosts()).Returns(new[] { CreateItem("same"), CreateItem("same") });

            var actual = GetSitemapLocs(Build(container));

            Assert.Single(actual, u => u == "https://example.com/blog/Posts/same.html");
            Assert.Equal(actual.Distinct().Count(), actual.Count());
        }

        [Fact]
        public void EncodeSlugsInSitemapUrls()
        {
            var (container, contentRepo) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl });
            contentRepo.Setup(r => r.GetAllPosts()).Returns(new[] { CreateItem("Café Post"), CreateItem("already%20encoded") });

            var actual = GetSitemapLocs(Build(container));

            Assert.Contains("https://example.com/blog/Posts/Caf%C3%A9%20Post.html", actual);
            Assert.Contains("https://example.com/blog/Posts/already%20encoded.html", actual);
        }

        [Fact]
        public void NotGenerateASitemapWhenDisabled()
        {
            var (container, _) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl, SitemapMode = SitemapMode.Disabled });
            var actual = Build(container);
            Assert.DoesNotContain(actual, f => SitemapGenerator.IsSitemapPath(f.RelativeFilePath));
        }

        [Fact]
        public void NotValidateTheBaseUrlWhenDisabled()
        {
            var (container, _) = CreateContainer(new BuildSettings() { PublicBaseUrl = "not a url", SitemapMode = SitemapMode.Disabled });
            var actual = Build(container);
            Assert.DoesNotContain(actual, f => SitemapGenerator.IsSitemapPath(f.RelativeFilePath));
        }

        [Fact]
        public void NotWarnWhenDisabled()
        {
            var (container, _) = CreateContainer(new BuildSettings() { SitemapMode = SitemapMode.Disabled });
            var logger = new FakeLogger();
            container.AddSingleton<Microsoft.Extensions.Logging.ILogger<Builder>>(logger);

            Build(container);

            Assert.Empty(logger.Warnings);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void SkipTheSitemapAndWarnOnceInAutoModeWhenBaseUrlIsMissing(String? baseUrl)
        {
            var (container, _) = CreateContainer(new BuildSettings() { PublicBaseUrl = baseUrl, SitemapMode = SitemapMode.Auto });
            var logger = new FakeLogger();
            container.AddSingleton<Microsoft.Extensions.Logging.ILogger<Builder>>(logger);

            var actual = Build(container);

            Assert.DoesNotContain(actual, f => SitemapGenerator.IsSitemapPath(f.RelativeFilePath));
            Assert.Single(logger.Warnings);
        }

        [Fact]
        public void SkipTheSitemapWhenNoBuildSettingsAreRegistered()
        {
            var (container, _) = CreateContainer(null);
            var actual = Build(container);
            Assert.DoesNotContain(actual, f => SitemapGenerator.IsSitemapPath(f.RelativeFilePath));
        }

        [Theory]
        [InlineData(SitemapMode.Auto)]
        [InlineData(SitemapMode.Enabled)]
        public void FailIfTheBaseUrlIsInvalid(SitemapMode mode)
        {
            var (container, _) = CreateContainer(new BuildSettings() { PublicBaseUrl = "ftp://example.com/", SitemapMode = mode });
            Assert.Throws<SitemapException>(() => Build(container));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FailIfEnabledAndTheBaseUrlIsMissing(String? baseUrl)
        {
            var (container, _) = CreateContainer(new BuildSettings() { PublicBaseUrl = baseUrl, SitemapMode = SitemapMode.Enabled });
            Assert.Throws<SitemapException>(() => Build(container));
        }

        [Fact]
        public void FailBeforeGeneratingAnyContentIfTheBaseUrlIsInvalid()
        {
            var (container, contentRepo) = CreateContainer(new BuildSettings() { PublicBaseUrl = "invalid", SitemapMode = SitemapMode.Auto });
            Assert.Throws<SitemapException>(() => Build(container));
            contentRepo.Verify(r => r.GetAllPosts(), Times.Never());
        }

        [Theory]
        [InlineData("", "sitemap.xml")]
        [InlineData(".", "sitemap.xml")]
        [InlineData("", "SITEMAP.xml")]
        public void FailIfACopiedFileWouldOverwriteTheSitemap(String relativePath, String fileName)
        {
            var siteSettings = new SiteSettingsBuilder().AddAdditionalFilePaths(new[] { "CopiedRoot" }).Build();
            var (container, contentRepo) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl }, siteSettings);
            var sourceFile = (null as SourceFile).Create(new byte[] { 1, 2, 3 }, relativePath, fileName);
            contentRepo.Setup(r => r.GetFolderContents("CopiedRoot", It.IsAny<bool>())).Returns(new[] { sourceFile });

            var ex = Assert.Throws<SitemapException>(() => Build(container));
            Assert.Contains("sitemap.xml", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void LeaveACopiedSitemapAloneWhenDisabled()
        {
            var siteSettings = new SiteSettingsBuilder().AddAdditionalFilePaths(new[] { "CopiedRoot" }).Build();
            var (container, contentRepo) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl, SitemapMode = SitemapMode.Disabled }, siteSettings);
            var sourceFile = (null as SourceFile).Create(new byte[] { 1, 2, 3 }, "", "sitemap.xml");
            contentRepo.Setup(r => r.GetFolderContents("CopiedRoot", It.IsAny<bool>())).Returns(new[] { sourceFile });

            var actual = Build(container);

            var sitemap = actual.Single(f => SitemapGenerator.IsSitemapPath(f.RelativeFilePath));
            Assert.Equal(TemplateType.Raw, sitemap.SourceTemplateType);
        }

        [Fact]
        public void AllowCopiedSitemapsInSubfolders()
        {
            var siteSettings = new SiteSettingsBuilder().AddAdditionalFilePaths(new[] { "CopiedRoot" }).Build();
            var (container, contentRepo) = CreateContainer(new BuildSettings() { PublicBaseUrl = _baseUrl }, siteSettings);
            var sourceFile = (null as SourceFile).Create(new byte[] { 1, 2, 3 }, "Files", "sitemap.xml");
            contentRepo.Setup(r => r.GetFolderContents("CopiedRoot", It.IsAny<bool>())).Returns(new[] { sourceFile });

            var actual = Build(container);

            Assert.Single(actual, f => SitemapGenerator.IsSitemapPath(f.RelativeFilePath));
        }

        private sealed class FakeLogger : Microsoft.Extensions.Logging.ILogger<Builder>
        {
            public List<String> Warnings { get; } = new();

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => true;

            public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception? exception, Func<TState, Exception?, String> formatter)
            {
                if (logLevel == Microsoft.Extensions.Logging.LogLevel.Warning)
                    Warnings.Add(formatter(state, exception));
            }
        }
    }
}
