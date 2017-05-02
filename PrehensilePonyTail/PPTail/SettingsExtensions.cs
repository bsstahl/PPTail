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
        const string _sourceDataPathSettingName = "sourceDataPath";
        const string _outputPathSettingName = "outputPath";
        const string _additionalFilePathsSettingName = "additionalFilePaths";
        const string _createDasBlogSyndicationCompatibilityFileSettingName = "createDasBlogSyndicationCompatibilityFile";
        const string _createDasBlogPostsCompatibilityFileSettingName = "createDasBlogPostsCompatibilityFile";


        public static ISettings Create(this ISettings ignore)
        {
            const string _sourceDataPathSettingName = "sourceDataPath";
            const string _outputPathSettingName = "outputPath";

            string outputFileExtension = "html";
            string dateFormatSpecifier = "yyyy-MM-dd";
            string dateTimeFormatSpecifier = "yyyy-MM-dd H:mm UTC";
            string itemSeparator = "<hr/>";
            string additionalFilePaths = "images,pics";

            bool createDasBlogSyndicationCompatibilityFile = true;
            bool createDasBlogPostsCompatibilityFile = true;

            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            string sourceDataPath = config[_sourceDataPathSettingName];
            string outputPath = config[_outputPathSettingName];

            return (null as ISettings).Create(sourceDataPath, outputPath, dateFormatSpecifier, dateTimeFormatSpecifier, itemSeparator, outputFileExtension, additionalFilePaths, createDasBlogSyndicationCompatibilityFile, createDasBlogPostsCompatibilityFile);

        }

        public static ISettings Create(this ISettings ignore, string sourceDataPath, string outputPath, string dateFormatSpecifier, string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension, string additionalFilePaths, bool createDasBlogSyndicationCompatibilityFile, bool createDasBlogPostsCompatibilityFile)
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
            settings.ExtendedSettings.Set(_createDasBlogPostsCompatibilityFileSettingName, createDasBlogPostsCompatibilityFile.ToString());

            return settings;
        }
    }
}
