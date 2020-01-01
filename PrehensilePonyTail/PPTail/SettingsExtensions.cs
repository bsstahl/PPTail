using Microsoft.Extensions.Configuration;
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
        // TODO: See if the Additional File Paths setting needs to be modified to be provider specific
        const String _additionalFilePathsSettingName = "additionalFilePaths";

        const String _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";
        const String _createDasBlogPostsCompatibilityFileSettingName = "createDasBlogPostsCompatibilityFile";


        public static ISettings Create(this ISettings ignore, String sourceConnection, String targetConnection, String templateConnection)
        {
            String outputFileExtension = "html";
            String dateFormatSpecifier = "yyyy-MM-dd";
            String dateTimeFormatSpecifier = "yyyy-MM-dd H:mm UTC";
            String itemSeparator = "<hr/>";
            String additionalFilePaths = "images,pics";

            bool createDasBlogSyndicationCompatibilityFile = true;
            bool createDasBlogPostsCompatibilityFile = true;

            return (null as ISettings).Create(sourceConnection, targetConnection, templateConnection, dateFormatSpecifier, dateTimeFormatSpecifier, itemSeparator, outputFileExtension, additionalFilePaths, createDasBlogSyndicationCompatibilityFile, createDasBlogPostsCompatibilityFile);
        }

        public static ISettings Create(this ISettings ignore, String sourceConnection, String targetConnection, String templateConnection, String dateFormatSpecifier, String dateTimeFormatSpecifier, String itemSeparator, String outputFileExtension, String additionalFilePaths, bool createDasBlogSyndicationCompatibilityFile, bool createDasBlogPostsCompatibilityFile)
        {
            var settings = new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                OutputFileExtension = outputFileExtension, 
                SourceConnection  = sourceConnection,
                TargetConnection = targetConnection,
                TemplateConnection = templateConnection
            };

            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalFilePaths);
            settings.ExtendedSettings.Set(_createDasBlogSyndicationCompatibilityFileSettingName, createDasBlogSyndicationCompatibilityFile.ToString());
            settings.ExtendedSettings.Set(_createDasBlogPostsCompatibilityFileSettingName, createDasBlogPostsCompatibilityFile.ToString());

            return settings;
        }
    }
}
