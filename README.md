# PPTail
Prehensile Pony Tail - A static website generator written in c# that produces plain HTML5/CSS output for ultimate scalability.

## Template Variable Replacements

PPTail uses a token-substitution system in HTML and XML templates. Tokens are written as `{VariableName}` and are replaced at build time with the appropriate values. Some tokens accept parameters using the syntax `{VariableName:parameter}` or `{VariableName:parameter|option}`.

### Template Types

| Template File | Description |
|---|---|
| `Homepage.template.html` | The site home page |
| `ContentItem.template.html` | Repeated block for each post/page item listed in a multi-item page |
| `ContentPage.template.html` | A single content page |
| `PostPage.template.html` | A single blog post page |
| `Archive.template.html` | The archive listing page |
| `ArchiveItem.template.html` | Repeated block for each item in the archive listing |
| `ContactPage.template.html` | The contact page |
| `SearchPage.template.html` | A tag search results page |
| `Syndication.template.xml` | The RSS syndication feed wrapper |
| `SyndicationItem.template.xml` | Repeated block for each item in the RSS feed |
| `Redirect.template.html` | A redirect page used for permalinks |
| Widget TextBox | Inline HTML content stored in a widget's dictionary |

---

### Content Item Variables

These variables are replaced with values from an individual post or page. They are available in all **content item templates**: `ContentItem.template.html`, `ArchiveItem.template.html`, `SyndicationItem.template.xml`, `PostPage.template.html`, and `ContentPage.template.html`.

> **Note:** In `SyndicationItem.template.xml` all text values are XML-encoded and dates are formatted in ISO 8601 / RFC formats instead of the site-configured date formats.

| Variable | Description | Notes |
|---|---|---|
| `{Title}` | The title of the post or page | |
| `{Content}` | The full HTML body content of the post or page | XML-encoded in syndication |
| `{Author}` | The author name | |
| `{ByLine}` | The by-line text (e.g. "by Jane Smith") | |
| `{Description}` | The short description / summary | XML-encoded in syndication |
| `{Teaser}` | The teaser text; empty string if not set | XML-encoded in syndication |
| `{TeaserOrDescription}` | The teaser if set, otherwise the description | XML-encoded in syndication |
| `{PublicationDate}` | Publication date formatted with `SiteSettings.DateFormatSpecifier` (default `yyyy-MM-dd`) | ISO 8601 date in syndication |
| `{PublicationDateTime}` | Publication date and time formatted with `SiteSettings.DateTimeFormatSpecifier` (default `yyyy-MM-dd H:mm UTC`) | ISO 8601 date-time in syndication |
| `{PublicationDateTimeRFC}` | Publication date and time in RFC 2822 format (e.g. `Wed, 02 Oct 2002 13:00:00 GMT`) | Always RFC format |
| `{LastModificationDate}` | Last modification date formatted with `SiteSettings.DateFormatSpecifier`; empty if never modified | ISO 8601 date in syndication; empty if never modified |
| `{LastModificationDateTime}` | Last modification date and time formatted with `SiteSettings.DateTimeFormatSpecifier`; empty if never modified | ISO 8601 date-time in syndication; empty if never modified |
| `{Link}` | Relative URL to the post (e.g. `../Posts/my-post-slug.html`) | |
| `{Permalink}` | Full `<a>` anchor element linking to the item's permanent URL | Rendered as a URL in `SyndicationItem` (`{PermalinkUrl}` equivalent) |
| `{PermalinkUrl}` | The raw URL of the item's permanent link (no wrapping anchor tag) | |
| `{Tags}` | Comma-separated list of tag links rendered as `<a>` elements | |
| `{Hashtags}` | Space-separated list of tags rendered as `#hashtag` strings (suitable for social sharing) | |
| `{Categories}` | Comma-separated list of category links rendered as `<a>` elements | |

---

### Page-Level Variables

These variables are available in all page-level templates (`Homepage.template.html`, `ContentPage.template.html`, `PostPage.template.html`, `Archive.template.html`, `ContactPage.template.html`, `SearchPage.template.html`, `Syndication.template.xml`, and Widget TextBoxes).

| Variable | Description |
|---|---|
| `{Content}` | The main content area (e.g. rendered list of posts, or full page body) |
| `{NavigationMenu}` | The rendered navigation menu HTML |
| `{Sidebar}` | The rendered sidebar HTML |
| `{Title}` | The page title (e.g. archive heading, search term, or page name) |

---

### Site Settings Variables

These variables are replaced with values from the site configuration and are available in **all templates**.

| Variable | Description | Configured In |
|---|---|---|
| `{SiteTitle}` | The site title | `SiteSettings.Title` |
| `{SiteDescription}` | The site description / tagline | `SiteSettings.Description` |
| `{ContactEmail}` | The contact email address | `SiteSettings.ContactEmail` |
| `{Copyright}` | The copyright notice text | `SiteSettings.Copyright` |

#### User-Defined Site Variables

Any variable defined in `SiteSettings.Variables` is also available in all templates using `{VariableName}`. This allows you to define custom tokens such as `{TwitterLink}` or `{AnalyticsId}` in the site settings and reference them in templates.

---

### Path Variables

These variables are replaced with the relative path from the current page back to the site root. They are available in **all templates and content**.

| Variable | Description |
|---|---|
| `{PathToRoot}` | Relative path from the current page to the site root (e.g. `../`) |
| `{PathToSiteRoot}` | Alias for `{PathToRoot}` |

> **Note:** The URL-encoded variants `%7BPathToRoot%7D` and `%7BPathToSiteRoot%7D` are also supported for use inside URL attributes that may get encoded by an editor.

---

### Page Link Variables

These variables generate HTML anchor (`<a>`) elements or URLs pointing to specific pages on the site. They accept an optional parameter to control link text. They are available in **all templates and content**.

#### Simple Page Links

| Variable | Default Link Text | Description |
|---|---|---|
| `{HomePageLink}` | `Home` | Link to the site home page |
| `{HomePageLink:link text}` | *link text* | Link to the home page with custom text |
| `{ArchivePageLink}` | `Archive` | Link to the archive page |
| `{ArchivePageLink:link text}` | *link text* | Link to the archive page with custom text |
| `{ContactPageLink}` | `Contact Me` | Link to the contact page |
| `{ContactPageLink:link text}` | *link text* | Link to the contact page with custom text |
| `{FeedLink}` | *(URL only, no anchor)* | URL of the RSS/Atom syndication feed |

#### Content Item Links

| Variable | Description |
|---|---|
| `{PageLink:slug}` | Link to a content page by its slug; link text defaults to the page title |
| `{PageLink:slug\|link text}` | Link to a content page by its slug with custom link text |
| `{PostLink:slug}` | Link to a blog post by its slug; link text defaults to the post title |
| `{PostLink:slug\|link text}` | Link to a blog post by its slug with custom link text |

#### Search/Tag Links

| Variable | Description |
|---|---|
| `{SearchLink:tag}` | Link to the search results page for the given tag; link text defaults to the tag name |
| `{SearchLink:tag\|link text}` | Link to the search results page for the given tag with custom link text |

#### File and Image Links

| Variable | Description |
|---|---|
| `{FileLink:filename}` | Link to a file in the configured files folder (default: `Files/`); link text defaults to the filename |
| `{FileLink:filename\|link text}` | Link to a file with custom link text |
| `{ImageLink:path}` | Embeds an `<img>` element for the image at the given path in the configured images folder (default: `Images/`); no `alt` text |
| `{ImageLink:path\|alt text}` | Embeds an `<img>` element with the given `alt` text |

> The files folder and images folder paths can be customized by setting the `FilesFolder` and `ImagesFolder` keys in `SiteSettings.Variables`.

---

### Redirect Template Variables

These variables are only available in the `Redirect.template.html` template.

| Variable | Description |
|---|---|
| `{Url}` | The target URL that the page should redirect to |

---

### Variable Availability by Template

| Variable | Homepage | ContentItem | ContentPage | PostPage | Archive | ArchiveItem | ContactPage | SearchPage | Syndication | SyndicationItem | Redirect | Widget TextBox |
|---|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| `{Title}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | ✓ | | |
| `{Content}` | ✓ | ✓ | ✓ | ✓ | ✓ | | | ✓ | ✓ | ✓ | | ✓ |
| `{Author}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{ByLine}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | ✓ | | |
| `{Description}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{Teaser}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{TeaserOrDescription}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{PublicationDate}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{PublicationDateTime}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{PublicationDateTimeRFC}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{LastModificationDate}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{LastModificationDateTime}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{Link}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{Permalink}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{PermalinkUrl}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{Tags}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{Hashtags}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{Categories}` | | ✓ | ✓ | ✓ | | ✓ | | | | ✓ | | |
| `{NavigationMenu}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | |
| `{Sidebar}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | |
| `{SiteTitle}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | ✓ |
| `{SiteDescription}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | ✓ |
| `{ContactEmail}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | ✓ |
| `{Copyright}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | ✓ |
| `{PathToRoot}` / `{PathToSiteRoot}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{HomePageLink}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{ArchivePageLink}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{ContactPageLink}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{FeedLink}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{PageLink:...}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{PostLink:...}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{SearchLink:...}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{FileLink:...}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{ImageLink:...}` | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | | ✓ |
| `{Url}` | | | | | | | | | | | ✓ | |
| *User-defined* | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | | | ✓ |
