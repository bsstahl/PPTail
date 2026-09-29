using PPTail.Enumerations;
using System;

namespace PPTail.Entities
{
    /// <summary>
    /// Build-level settings that are independent of the content provider
    /// </summary>
    public class BuildSettings
    {
        /// <summary>
        /// The absolute public deployment root of the site (including any subdirectory),
        /// used to build canonical URLs. e.g. https://example.com/blog/
        /// </summary>
        public String? PublicBaseUrl { get; set; }

        public SitemapMode SitemapMode { get; set; } = SitemapMode.Auto;
    }
}
