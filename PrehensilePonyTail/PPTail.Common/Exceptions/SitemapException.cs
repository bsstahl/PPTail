using System;

namespace PPTail.Exceptions
{
    /// <summary>
    /// Thrown when the sitemap cannot be generated due to invalid configuration or invalid output
    /// </summary>
    public class SitemapException : Exception
    {
        public SitemapException(String message)
            : base(message)
        { }

        public SitemapException(String message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
