using PPTail.Entities;
using PPTail.Interfaces;
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
        const string _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";

        public static ISettings Create(this ISettings ignore, string sourceDataPath, string outputPath, string dateFormatSpecifier, string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension, string additionalFilePaths, bool createDasBlogSyndicationCompatibilityFile)
        {
            var settings = new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                OutputFileExtension = outputFileExtension
            };

            settings.ExtendedSettings.Set(_sourceDataPathSettingName, sourceDataPath);
            settings.ExtendedSettings.Set(_outputPathSettingName, outputPath);
            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalFilePaths);
            settings.ExtendedSettings.Set(_createDasBlogSyndicationCompatibilityFileSettingName, createDasBlogSyndicationCompatibilityFile.ToString());

            return settings;
        }
    }
}
