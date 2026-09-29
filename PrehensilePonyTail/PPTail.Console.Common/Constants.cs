using System;

namespace PPTail.Console.Common;

public static class Constants
{
    public const String VALIDATEONLY_SWITCH = "--validateonly";
    public const String VERBOSE_SWITCH = "--verbose";
    public const String WINDOWSLINEENDINGS_SWITCH = "--windowslineendings";

    // Options that take a value, e.g. --publicbaseurl=https://example.com/blog/
    public const String PUBLICBASEURL_OPTION = "--publicbaseurl";
    public const String SITEMAPMODE_OPTION = "--sitemapmode";
    public const Char OPTION_VALUE_SEPARATOR = '=';
}
