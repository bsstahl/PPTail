# PPTail
Prehensile Pony Tail - A static website generator written in c# that produces plain HTML5/CSS output for ultimate scalability.

Markdown fenced code blocks tagged with `mermaid` render client-side as Mermaid diagrams in generated pages.

## Sitemap

PPTail can generate a [`sitemap.xml`](https://www.sitemaps.org/protocol.html) at the root of the output folder. Sitemap generation is controlled by two build-level command-line options, which work with any content provider:

| Option | Description |
|---|---|
| `--PublicBaseUrl=<url>` | The absolute public URL where the site is deployed, including any subdirectory (e.g. `https://example.com/blog/`). Only `http` and `https` URLs without credentials, query strings or fragments are accepted. A missing trailing slash is added automatically; path casing and percent-encoded characters are preserved. |
| `--SitemapMode=<mode>` | `Auto` (default), `Enabled` or `Disabled`. |

```
pptail "Provider=...;FilePath=..." "Provider=PPTail.Output.FileSystem.Repository;FilePath=..." "Provider=...;FilePath=..." --PublicBaseUrl=https://example.com/blog/ --SitemapMode=Enabled
```

| `SitemapMode` | `PublicBaseUrl` | Behavior |
|---|---|---|
| `Auto` | missing/blank | The site is built without a sitemap and a warning is logged |
| `Auto` | valid | `sitemap.xml` is generated |
| `Auto` | invalid | The build fails before any output is saved |
| `Enabled` | missing or invalid | The build fails before any output is saved |
| `Enabled` | valid | `sitemap.xml` is generated |
| `Disabled` | any | No sitemap is generated and no warning is logged |

The sitemap contains only the canonical URLs of the public pages: the homepage (represented by the `PublicBaseUrl` itself), published posts, published standalone pages, the archive page and the contact page. Unpublished and preview-only posts, the generated "Unpublished Posts" overview page, tag/category search pages, permalink redirects, the RSS feed, static assets and copied files are never included. A published post or page can be omitted by setting `excludefromsitemap: true` in its frontmatter (`ExcludeFromSitemap` in JSON content). URLs are listed once each in a deterministic order, and `lastmod`, `changefreq` and `priority` are not emitted.

When a sitemap is being generated, the build fails if a copied file (e.g. from an `AdditionalFilePaths` folder) would overwrite `sitemap.xml` at the output root. When sitemap generation is disabled or skipped, a manually supplied `sitemap.xml` is copied as-is. Note that disabling sitemap generation does not remove a sitemap that has already been deployed.
