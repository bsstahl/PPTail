using Microsoft.Extensions.Configuration;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public static class SettingsExtensions
    {
        // TODO: See if the Additional File Paths setting needs to be modified to be provider specific
        const String _additionalFilePathsSettingName = "additionalFilePaths";

        const String _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";
        const String _createDasBlogPostsCompatibilityFileSettingName = "createDasBlogPostsCompatibilityFile";


        public static ISettings Create(this ISettings ignore, String sourceConnection)
        {
            var outputFileExtension = "html";
            var dateFormatSpecifier = "yyyy-MM-dd";
            var dateTimeFormatSpecifier = "yyyy-MM-dd H:mm UTC";
            var itemSeparator = "<hr/>";
            var additionalFilePaths = "Images,Pics,Files";

            var createDasBlogSyndicationCompatibilityFile = true;
            var createDasBlogPostsCompatibilityFile = true;

            return (null as ISettings).Create(sourceConnection, dateFormatSpecifier, dateTimeFormatSpecifier, itemSeparator, outputFileExtension, additionalFilePaths, createDasBlogSyndicationCompatibilityFile, createDasBlogPostsCompatibilityFile);
        }

        public static ISettings Create(this ISettings ignore, String sourceConnection, String dateFormatSpecifier, String dateTimeFormatSpecifier, String itemSeparator, String outputFileExtension, String additionalFilePaths, Boolean createDasBlogSyndicationCompatibilityFile, Boolean createDasBlogPostsCompatibilityFile)
        {
            var settings = new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                OutputFileExtension = outputFileExtension
            };

            _ = settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalFilePaths);
            _ = settings.ExtendedSettings.Set(_createDasBlogSyndicationCompatibilityFileSettingName, createDasBlogSyndicationCompatibilityFile.ToString(CultureInfo.InvariantCulture));
            _ = settings.ExtendedSettings.Set(_createDasBlogPostsCompatibilityFileSettingName, createDasBlogPostsCompatibilityFile.ToString(CultureInfo.InvariantCulture));

            return settings;
        }
    }
}
