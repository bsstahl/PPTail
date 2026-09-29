namespace PPTail.Enumerations
{
    public enum SitemapMode
    {
        /// <summary>
        /// Generate a sitemap when a PublicBaseUrl is configured, otherwise skip it with a warning (default)
        /// </summary>
        Auto,

        /// <summary>
        /// Always generate a sitemap. A valid PublicBaseUrl is required.
        /// </summary>
        Enabled,

        /// <summary>
        /// Never generate a sitemap
        /// </summary>
        Disabled
    }
}
