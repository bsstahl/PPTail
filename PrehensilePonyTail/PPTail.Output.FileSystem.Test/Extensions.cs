using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
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

        public static Repository Create(this IOutputRepository ignore)
        {
            return ignore.Create(Mock.Of<IFile>(), Mock.Of<Settings>());
        }

        public static Repository Create(this IOutputRepository ignore, IFile file, Settings settings)
        {
            return ignore.Create(file, Mock.Of<IDirectory>(), settings);
        }

        public static Repository Create(this IOutputRepository ignore, IFile file, IDirectory directory, Settings settings)
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(file);
            container.AddSingleton<IDirectory>(directory);
            container.AddSingleton<ISettings>(settings);
            return ignore.Create(container);
        }

        public static Repository Create(this IOutputRepository ignore, IServiceCollection container)
        {
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
                files.Add((null as SiteFile).Create());

            return files;
        }

        public static IEnumerable<SiteFile> Create(this IEnumerable<SiteFile> ignore, int count, bool isEncoded)
        {
            var files = new List<SiteFile>();

            for (int i = 0; i < count; i++)
                files.Add((null as SiteFile).Create(string.Empty.GetRandom(), $"./{string.Empty.GetRandom()}", Enumerations.TemplateType.ContentPage, isEncoded));

            return files;
        }

        public static SiteFile Create(this SiteFile ignore)
        {
            return ignore.Create(string.Empty.GetRandom(), $"./{string.Empty.GetRandom()}", Enumerations.TemplateType.ContentPage, false);
        }

        public static SiteFile Create(this SiteFile ignore, bool isEncoded)
        {
            string content;
            TemplateType templateType;

            if (isEncoded)
            {
                content = string.Empty.GetRandom();
                templateType = TemplateType.Raw;
            }
            else
            {
                content = Convert.ToBase64String(string.Empty.GetRandom().Select(s => Convert.ToByte(s)).ToArray());
                templateType = TemplateType.ContactPage;
            }

            return ignore.Create(content, $"./{string.Empty.GetRandom()}", templateType, isEncoded);
        }

        public static SiteFile Create(this SiteFile ignore, string content, string relativeFilePath, TemplateType sourceTemplateType, bool isEncoded)
        {
            return new SiteFile()
            {
                Content = content,
                RelativeFilePath = relativeFilePath,
                SourceTemplateType = sourceTemplateType,
                IsBase64Encoded = isEncoded
            };
        }


        public static Settings Create(this ISettings ignore)
        {
            return ignore.Create("yyyyMMdd", "yyyyMMdd hh:mm", "<hr/>", "html", $"\\{string.Empty.GetRandom()}");
        }

        public static Settings Create(this ISettings ignore, string outputPath)
        {
            return ignore.Create("yyyyMMdd", "yyyyMMdd hh:mm", "<hr/>", "html", outputPath);
        }

        public static Settings Create(this ISettings ignore, string dateFormatSpecifier, string dateTimeFormatSpecifier, string itemSeparator, string outputFileExtension, string outputPath)
        {
            var result = new Settings()
            {
                DateFormatSpecifier = dateFormatSpecifier,
                DateTimeFormatSpecifier = dateTimeFormatSpecifier,
                ItemSeparator = itemSeparator,
                OutputFileExtension = outputFileExtension,
                TargetConnection = $"Provider=Test;FilePath={outputPath}"
            };

            result.ExtendedSettings.Add(new Tuple<string, string>("outputPath", outputPath));
            return result;
        }

    }
}
