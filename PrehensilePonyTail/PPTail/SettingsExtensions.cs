using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public static class SettingsExtensions
    {
        public static Settings Create(this Settings ignore, string sourceDataPath, string outputPath, string dateTimeFormatSpecifier, string itemSeparator)
        {
            var settings = new Settings()
            {
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator
            };

            settings.ExtendedSettings.Set("sourceDataPath", sourceDataPath);
            settings.ExtendedSettings.Set("outputPath", outputPath);
            return settings;

        }
    }
}
