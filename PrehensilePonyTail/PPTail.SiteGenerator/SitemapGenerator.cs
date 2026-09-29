using PPTail.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace PPTail.SiteGenerator
{
    /// <summary>
    /// Generates the content of a sitemap.xml file (https://www.sitemaps.org/protocol.html)
    /// from a public base URL and the relative output paths of the pages to include
    /// </summary>
    public static class SitemapGenerator
    {
        public const String FileName = "sitemap.xml";
        public const String XmlNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        public const Int32 MaxUrlCount = 50000;
        public const Int32 MaxFileSizeBytes = 52428800; // 50 MiB

        private static readonly String[] _homepageFileNames = ["index.html", "index.htm"];

        /// <summary>
        /// Validates and normalizes a public base URL. Only absolute HTTP/HTTPS URLs without
        /// credentials, query strings or fragments are accepted. The path (including casing and
        /// percent-encoded characters) is preserved and a trailing slash is added if missing.
        /// </summary>
        /// <exception cref="SitemapException">Thrown if the value is missing or invalid</exception>
        public static String NormalizeBaseUrl(String? publicBaseUrl)
        {
            if (String.IsNullOrWhiteSpace(publicBaseUrl))
                throw new SitemapException("A PublicBaseUrl is required to generate a sitemap.");

            String value = publicBaseUrl!.Trim();

            if (value.Any(c => Char.IsWhiteSpace(c) || Char.IsControl(c) || c == '\\'))
                throw InvalidBaseUrl(publicBaseUrl, "it contains whitespace, control characters or backslashes");

            if (value.IndexOf('?') >= 0 || value.IndexOf('#') >= 0)
                throw InvalidBaseUrl(publicBaseUrl, "query strings and fragments are not allowed");

            if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
                throw InvalidBaseUrl(publicBaseUrl, "it is not an absolute URL");

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                throw InvalidBaseUrl(publicBaseUrl, "only http and https URLs are supported");

            if (!String.IsNullOrEmpty(uri.UserInfo))
                throw InvalidBaseUrl(publicBaseUrl, "credentials are not allowed");

            if (String.IsNullOrEmpty(uri.Host))
                throw InvalidBaseUrl(publicBaseUrl, "a host name is required");

            String schemeSeparator = Uri.SchemeDelimiter;
            if (value.IndexOf(schemeSeparator, StringComparison.Ordinal) != uri.Scheme.Length)
                throw InvalidBaseUrl(publicBaseUrl, "it is not an absolute URL");

            // Take the path from the original value so that casing and encoding are preserved
            Int32 authorityStart = uri.Scheme.Length + schemeSeparator.Length;
            Int32 pathStart = value.IndexOf('/', authorityStart);
            String originalPath = pathStart < 0 ? "/" : value.Substring(pathStart);

            var segments = originalPath.Split('/');
            if (segments.Any(s => s == "." || s == ".."))
                throw InvalidBaseUrl(publicBaseUrl, "relative path segments are not allowed");

            String path = String.Join("/", segments.Select(EscapePathSegment));
            if (!path.EndsWith("/", StringComparison.Ordinal))
                path += "/";

            String host = uri.HostNameType == UriHostNameType.Dns ? uri.IdnHost : uri.Host;
            String port = uri.IsDefaultPort ? String.Empty : $":{uri.Port.ToString(CultureInfo.InvariantCulture)}";

            return $"{uri.Scheme}{schemeSeparator}{host}{port}{path}";
        }

        /// <summary>
        /// Returns true if the supplied relative output path targets the sitemap file at the output root
        /// </summary>
        public static bool IsSitemapPath(String? relativeFilePath)
        {
            if (relativeFilePath is null)
                return false;

            return String.Equals(NormalizeRelativePath(relativeFilePath), FileName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Creates the canonical absolute URL for a generated output file
        /// </summary>
        /// <param name="normalizedBaseUrl">A base URL as returned by <see cref="NormalizeBaseUrl(string?)"/></param>
        /// <param name="relativeFilePath">The relative output path of the file, e.g. Posts/my-post.html</param>
        public static String ToAbsoluteUrl(String normalizedBaseUrl, String relativeFilePath)
        {
            if (normalizedBaseUrl is null)
                throw new ArgumentNullException(nameof(normalizedBaseUrl));

            if (relativeFilePath is null)
                throw new ArgumentNullException(nameof(relativeFilePath));

            String path = NormalizeRelativePath(relativeFilePath);
            var segments = path.Split('/');

            if (segments.Any(s => String.IsNullOrEmpty(s) || s == "." || s == ".."))
                throw new SitemapException($"The output path '{relativeFilePath}' cannot be included in the sitemap because it is empty or escapes the site root.");

            // The site homepage is represented by the base URL itself
            if (segments.Length == 1 && _homepageFileNames.Contains(segments[0], StringComparer.OrdinalIgnoreCase))
                return normalizedBaseUrl;

            return normalizedBaseUrl + String.Join("/", segments.Select(EscapePathSegment));
        }

        /// <summary>
        /// Generates the sitemap XML for the supplied output paths. URLs are de-duplicated and
        /// sorted deterministically (homepage first, then ordinal). No lastmod, changefreq or
        /// priority values are emitted.
        /// </summary>
        public static String Generate(String? publicBaseUrl, IEnumerable<String> relativeFilePaths)
        {
            if (relativeFilePaths is null)
                throw new ArgumentNullException(nameof(relativeFilePaths));

            String baseUrl = NormalizeBaseUrl(publicBaseUrl);

            var urls = relativeFilePaths
                .Select(p => ToAbsoluteUrl(baseUrl, p))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(u => String.Equals(u, baseUrl, StringComparison.Ordinal) ? 0 : 1)
                .ThenBy(u => u, StringComparer.Ordinal)
                .ToList();

            if (urls.Count > MaxUrlCount)
                throw new SitemapException($"The sitemap would contain {urls.Count} URLs which exceeds the limit of {MaxUrlCount} URLs per sitemap.");

            var encoding = new UTF8Encoding(false);
            var settings = new XmlWriterSettings()
            {
                Encoding = encoding,
                Indent = true,
                NewLineChars = "\n",
                NewLineHandling = NewLineHandling.Replace
            };

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset", XmlNamespace);
                    foreach (var url in urls)
                    {
                        writer.WriteStartElement("url", XmlNamespace);
                        writer.WriteElementString("loc", XmlNamespace, url);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                bytes = stream.ToArray();
            }

            if (bytes.Length > MaxFileSizeBytes)
                throw new SitemapException($"The sitemap would be {bytes.Length} bytes which exceeds the limit of {MaxFileSizeBytes} bytes per sitemap.");

            return encoding.GetString(bytes);
        }

        private static String NormalizeRelativePath(String relativeFilePath)
        {
            String path = relativeFilePath.Trim().Replace('\\', '/');
            while (path.StartsWith("./", StringComparison.Ordinal))
                path = path.Substring(2);
            return path.TrimStart('/');
        }

        private static String EscapePathSegment(String segment)
        {
            var result = new StringBuilder();
            for (Int32 i = 0; i < segment.Length; i++)
            {
                Char c = segment[i];
                if (c == '%' && i + 2 < segment.Length && IsHexDigit(segment[i + 1]) && IsHexDigit(segment[i + 2]))
                {
                    // Preserve characters that are already percent-encoded
                    result.Append(segment, i, 3);
                    i += 2;
                }
                else if (IsUnescapedPathChar(c))
                    result.Append(c);
                else
                {
                    Int32 length = (Char.IsHighSurrogate(c) && i + 1 < segment.Length && Char.IsLowSurrogate(segment[i + 1])) ? 2 : 1;
                    foreach (var b in Encoding.UTF8.GetBytes(segment.Substring(i, length)))
                        result.Append('%').Append(b.ToString("X2", CultureInfo.InvariantCulture));
                    i += length - 1;
                }
            }
            return result.ToString();
        }

        private static bool IsHexDigit(Char c)
            => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');

        // RFC 3986 pchar (excluding pct-encoded): unreserved / sub-delims / ":" / "@"
        private static bool IsUnescapedPathChar(Char c)
            => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')
            || "-._~!$&'()*+,;=:@".IndexOf(c) >= 0;

        private static SitemapException InvalidBaseUrl(String? value, String reason)
            => new($"The PublicBaseUrl '{value}' is invalid: {reason}. An absolute http or https URL such as 'https://example.com/blog/' is required.");
    }
}
