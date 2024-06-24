using PPTail.Entities;
using System;

namespace PPTail.Output.FileSystem.Extensions;

internal static class StringExtensions
{
    internal static string ConvertLineEndingsToUnix(this string value)
    {
        return value.Replace("\r\n", "\n").Replace("\r", "\n");
    }

}
