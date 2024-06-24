using PPTail.Entities;
using PPTail.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPTail.Output.FileSystem.Extensions;

public static class SiteFileExtensions
{
    public static IEnumerable<SiteFile> ConvertLineEndingsToUnix(this IEnumerable<SiteFile> siteFiles)
    {
        return siteFiles.Select(f => f.ConvertLineEndingsToUnix());
    }

    internal static SiteFile ConvertLineEndingsToUnix(this SiteFile siteFile)
    {
        return siteFile.IsPreEncoded()
            ? siteFile
            : new SiteFile()
            {
                Content = siteFile.Content.ConvertLineEndingsToUnix(),
                SourceTemplateType = siteFile.SourceTemplateType,
                RelativeFilePath = siteFile.RelativeFilePath,
                IsBase64Encoded = siteFile.IsBase64Encoded
            };
    }

    internal static bool IsPreEncoded(this SiteFile siteFile)
    {
        return siteFile.SourceTemplateType.Equals(TemplateType.Raw) 
            || siteFile.IsBase64Encoded;
    }
}
