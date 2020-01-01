using System;
using System.Collections.Generic;

namespace PPTail.Interfaces
{
    public interface ISettings
    {
        String DateFormatSpecifier { get; set; }
        String DateTimeFormatSpecifier { get; set; }
        String ItemSeparator { get; set; }
        String OutputFileExtension { get; set; }

        String SourceConnection { get; set; }
        String TargetConnection { get; set; }
        String TemplateConnection { get; set; }

        ExtendedSettingsCollection ExtendedSettings { get; }
    }
}