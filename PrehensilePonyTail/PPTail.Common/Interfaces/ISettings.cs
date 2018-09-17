using System;
using System.Collections.Generic;

namespace PPTail.Interfaces
{
    public interface ISettings
    {
        string DateFormatSpecifier { get; set; }
        string DateTimeFormatSpecifier { get; set; }
        string ItemSeparator { get; set; }
        string OutputFileExtension { get; set; }

        string SourceConnection { get; set; }
        string TargetConnection { get; set; }
        string TemplateConnection { get; set; }

        ExtendedSettingsCollection ExtendedSettings { get; }
    }
}