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
        // TODO: Replace OuputPath extended setting with a ConnectionString implementation on the provider
        const string _outputPathSettingName = "outputPath";

        const string _additionalFilePathsSettingName = "additionalFilePaths";

        const string _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";
        const string _createDasBlogPostsCompatibilityFileSettingName = "createDasBlogPostsCompatibilityFile";


        public static ISettings Create(this ISettings ignore, string sourceConnection, string targetConnection, string templateConnection)
        {
            string outputFileExtension = "html";
            string dateFormatSpecifier = "yyyy-MM-dd";
            string dateTimeFormatSpecifier = "yyyy-MM-dd H:mm UTC";
            string itemSeparator = "<hr/>";
            string additionalFilePaths = "images,pics";

            bool createDasBlogSyndicationCompatibilityFile = true;
            bool createDasBlogPostsCompatibilityFile = true;

            return (null as ISettings).Create(sourceConnection, targetConnection, templateConnection, dateFormatSpecifier, dateTimeFormatSpecifier, itemSeparator, outputFileExtension, additionalFilePaths, createDasBlogSyndicationCompatibilityFile, createDasBlogPostsCompatibilityFile);
        }

        public static ISettings Create(this ISettings ignore, string sourceConnection, string targetConnection, string templateConnection, string dateFormatSpecifier, string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension, string additionalFilePaths, bool createDasBlogSyndicationCompatibilityFile, bool createDasBlogPostsCompatibilityFile)
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

            // TODO: Remove this (Provider connection string should be used instead)
            settings.ExtendedSettings.Set(_outputPathSettingName, targetConnection);

            settings.ExtendedSettings.Set(_additionalFilePathsSettingName, additionalFilePaths);
            settings.ExtendedSettings.Set(_createDasBlogSyndicationCompatibilityFileSettingName, createDasBlogSyndicationCompatibilityFile.ToString());
            settings.ExtendedSettings.Set(_createDasBlogPostsCompatibilityFileSettingName, createDasBlogPostsCompatibilityFile.ToString());

            return settings;
        }
    }
}
