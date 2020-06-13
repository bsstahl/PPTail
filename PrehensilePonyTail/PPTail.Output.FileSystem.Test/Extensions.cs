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
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        const string _defaultConnectionString = "Provider=PPTail.Output.FileSystem.Repository;FilePath=c:\\";

        public static Repository Create(this IOutputRepository ignore)
        {
            return ignore.Create(_defaultConnectionString);
        }

        public static Repository Create(this IOutputRepository ignore, String targetConnection)
        {
            return ignore.Create(Mock.Of<IFile>(), targetConnection);
        }

        public static Repository Create(this IOutputRepository ignore, IFile file, String targetConnection)
        {
            return ignore.Create(file, Mock.Of<IDirectory>(), targetConnection);
        }

        public static Repository Create(this IOutputRepository ignore, IFile file, IDirectory directory, string targetConnection)
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(file);
            container.AddSingleton<IDirectory>(directory);
            return ignore.Create(container, targetConnection);
        }

        public static Repository Create(this IOutputRepository ignore, IServiceCollection container, String targetConnection)
        {
            var serviceProvider = container.BuildServiceProvider();
            return new PPTail.Output.FileSystem.Repository(serviceProvider, targetConnection);
        }

        public static IEnumerable<SiteFile> Create(this IEnumerable<SiteFile> ignore)
        {
            return ignore.Create(25.GetRandom(10));
        }

        public static IEnumerable<SiteFile> Create(this IEnumerable<SiteFile> ignore, Int32 count)
        {
            var files = new List<SiteFile>();

            for (Int32 i = 0; i < count; i++)
                files.Add((null as SiteFile).Create());

            return files;
        }

        public static IEnumerable<SiteFile> Create(this IEnumerable<SiteFile> ignore, Int32 count, bool isEncoded)
        {
            var files = new List<SiteFile>();

            for (Int32 i = 0; i < count; i++)
                files.Add((null as SiteFile).Create(string.Empty.GetRandom(), $"./{string.Empty.GetRandom()}", Enumerations.TemplateType.ContentPage, isEncoded));

            return files;
        }

        public static SiteFile Create(this SiteFile ignore)
        {
            return ignore.Create(string.Empty.GetRandom(), $"./{string.Empty.GetRandom()}", Enumerations.TemplateType.ContentPage, false);
        }

        public static SiteFile Create(this SiteFile ignore, bool isEncoded)
        {
            String content;
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

        public static SiteFile Create(this SiteFile ignore, String content, String relativeFilePath, TemplateType sourceTemplateType, bool isEncoded)
        {
            return new SiteFile()
            {
                Content = content,
                RelativeFilePath = relativeFilePath,
                SourceTemplateType = sourceTemplateType,
                IsBase64Encoded = isEncoded
            };
        }

    }
}
