using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;

namespace PPTail.Output.FileSystem.Test
{
    public static class Extensions
    {
        public static Repository Create(this IOutputRepository ignore, IFile file, Settings settings)
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(file);
            container.AddSingleton<Settings>(settings);
            var serviceProvider = container.BuildServiceProvider();
            return new PPTail.Output.FileSystem.Repository(serviceProvider);
        }

        public static IEnumerable<SiteFile> Create(this IEnumerable<SiteFile> ignore)
        {
            return ignore.Create(25.GetRandom(10));
        }

        public static IEnumerable<SiteFile> Create(this IEnumerable<SiteFile> ignore, int count)
        {
            var files = new List<SiteFile>();

            for (int i = 0; i < count; i++)
                files.Add(new SiteFile()
                {
                    Content = string.Empty.GetRandom(),
                    RelativeFilePath = $"./{string.Empty.GetRandom()}",
                    SourceTemplateType = Enumerations.TemplateType.ContentPage
                });

            return files;
        }

        public static Settings Create(this Settings ignore)
        {
            return ignore.Create("yyyyMMdd", "yyyyMMdd hh:mm", "<hr/>", "html", $"\\{string.Empty.GetRandom()}");
        }

        public static Settings Create(this Settings ignore, string outputPath)
        {
            return ignore.Create("yyyyMMdd", "yyyyMMdd hh:mm", "<hr/>", "html", outputPath);
        }

        public static Settings Create(this Settings ignore, string dateFormatSpecifier, string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension, string outputPath)
        {
            var result = new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                outputFileExtension = outputFileExtension
            };

            result.ExtendedSettings.Add(new Tuple<string, string>("outputPath", outputPath));
            return result;
        }

    }
}
