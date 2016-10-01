using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public static class SettingsExtensions
    {
        const string _sourceDataPathSettingName = "sourceDataPath";
        const string _outputPathSettingName = "outputPath";
        const string _additionalFilePathsSettingName = "additionalFilePaths";

        public static Settings Create(this Settings ignore, string sourceDataPath, string outputPath, string dateFormatSpecifier, string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension, string additionalFilePaths)
        {
            var settings = new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                outputFileExtension = outputFileExtension
            };

            settings.ExtendedSettings.Set(_sourceDataPathSettingName, sourceDataPath);
            settings.ExtendedSettings.Set(_outputPathSettingName, outputPath);
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalFilePaths);

            return settings;
        }
    }
}
