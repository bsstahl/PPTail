using PPTail.Common.Builders;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog.Test
{
    public static class SettingsBuilderExtensions
    {
        //public static SettingsBuilder UseRandomValues(this SettingsBuilder builder)
        //{
        //    return builder
        //        .DateFormatSpecifier("")
        //        .DateTimeFormatSpecifier("")
        //        .ItemSeparator("")
        //        .OutputFileExtension("")
        //        .SourceConnection("")
        //        .TargetConnection("")
        //        .TemplateConnection("");
        //}

        public static SettingsBuilder UseGenericValues(this SettingsBuilder builder)
        {
            return builder
                .DateFormatSpecifier("yyyy-MM-dd")
                .DateTimeFormatSpecifier("yyyy-MM-dd H:mm UTC")
                .ItemSeparator("<hr/>")
                .OutputFileExtension("html")
                .SourceConnection("SourceConnection")
                .TargetConnection("TargetConnection")
                .TemplateConnection("TemplateConnection");
        }
    }
}
