using PPTail.Interfaces;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Entities;
using System.Xml.Linq;
using TestHelperExtensions;
using PPTail.Enumerations;
using System.Text;
using Xunit;
using Castle.DynamicProxy.Contributors;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace PPTail.Data.Forestry.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Extensions
    {
        const String _sourceDataPathSettingName = "sourceDataPath";
        const String _connectionStringFilepathKey = "FilePath";

        const String _widgetZoneNodeFormat = "<widget id=\"{0}\" title=\"{1}\" showTitle=\"{2}\">{3}</widget>";
        const String _categoryNodeFormat = "<category id=\"{0}\" description=\"{2}\" parent=\"\">{1}</category>";

        public static IContentRepository Create(this IContentRepository ignore)
        {
            IFile mockFileSystem = Mock.Of<IFile>();
            return ignore.Create(mockFileSystem, "c:\\");
        }

        public static IContentRepository Create(this IContentRepository ignore, IFile fileSystem, String sourcePath)
        {
            return ignore.Create(fileSystem, Mock.Of<IDirectory>(), sourcePath);
        }

        public static IContentRepository Create(this IContentRepository? _, IFile fileSystem, IDirectory directoryProvider, String sourcePath)
        {
            var container = new ServiceCollection();

            String sourceConnection = $"Provider=Test;{_connectionStringFilepathKey}={sourcePath}";

            container.AddSingleton<IFile>(fileSystem);
            container.AddSingleton<IDirectory>(directoryProvider);

            return new Repository(container.BuildServiceProvider(), sourceConnection);
        }

        public static IEnumerable<Widget> Create(this IEnumerable<Widget> ignore)
        {
            return ignore.Create(25.GetRandom(10));
        }

        public static IEnumerable<Widget> Create(this IEnumerable<Widget> ignore, Int32 count)
        {
            var result = new List<Widget>();

            var widgetTypes = Enum.GetValues(typeof(Enumerations.WidgetType));
            Int32 loopCount = Convert.ToInt32(System.Math.Ceiling(Convert.ToDouble(count / 3)));
            for (Int32 i = 0; i < loopCount; i++)
            {
                foreach (WidgetType widgetType in widgetTypes)
                    result.Add(widgetType.CreateWidget());
            }

            return result.Take(count);
        }

        public static Widget CreateWidget(this Enumerations.WidgetType widgetType)
        {
            return new Widget()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty.GetRandom(),
                ShowTitle = true.GetRandom(),
                WidgetType = widgetType,
                Dictionary = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("Content", string.Empty.GetRandom())
                    }
            };
        }

        public static IEnumerable<Widget> ConfigureWidgets(this Mock<IFile> fileSystem, Mock<IDirectory> directory, String rootPath, bool addInvalidTypes)
        {
            var widgets = (null as IEnumerable<Widget>).Create();
            return fileSystem.ConfigureWidgets(directory, widgets, rootPath, addInvalidTypes);
        }

        public static IEnumerable<Widget> ConfigureWidgets(this Mock<IFile> fileSystem, Mock<IDirectory> directory, IEnumerable<Widget> widgets, String rootPath, bool addInvalidTypes)
        {
            const String widgetPath = "Widgets";

            var theseWidgets = new List<Widget>(widgets);
            var fileNames = new List<String>();

            // Add Invalid Widget Types
            if (addInvalidTypes)
            {
                var w1 = new Widget()
                {
                    Id = Guid.NewGuid(),
                    Title = string.Empty.GetRandom(),
                    ShowTitle = true.GetRandom(),
                    WidgetType = WidgetType.Unknown
                };

                var w2 = new Widget()
                {
                    Id = Guid.NewGuid(),
                    Title = string.Empty.GetRandom(),
                    ShowTitle = true.GetRandom(),
                    WidgetType = WidgetType.Unknown
                };

                theseWidgets.Add(w1);
                theseWidgets.Add(w2);
            }

            String fullWidgetPath = System.IO.Path.Combine(rootPath, widgetPath);

            bool replacedAlpha = false;
            bool replacedNumeric = false;
            foreach (var widget in theseWidgets)
            {
                string fileName = $"{widget.Title.CreateSlug()}.md";
                string widgetFilePath = System.IO.Path.Combine(fullWidgetPath, fileName);
                fileNames.Add(widgetFilePath);

                string widgetText = widget.Serialize();

                if (widget.WidgetType == WidgetType.Unknown)
                {
                    // Replace the widget type in 2 of the unknowns
                    string pattern = "widgettype: Unknown";
                    if (!replacedAlpha)
                    {
                        string alphaValue = $"widgettype: {String.Empty.GetRandom()}";
                        widgetText = widgetText.Replace(pattern, alphaValue);
                        replacedAlpha = true;
                    }
                    else if (!replacedNumeric)
                    {
                        string numericValue = $"widgettype: {9999.GetRandom(999).ToString()}";
                        widgetText = widgetText.Replace(pattern, numericValue);
                        replacedNumeric = true;
                    }
                }

                fileSystem.Setup(f => f.ReadAllText(widgetFilePath))
                    .Returns(widgetText);
            }

            directory.Setup(d => d.EnumerateFiles(fullWidgetPath))
                .Returns(fileNames);

            return theseWidgets;
        }

        public static string ReplaceFirst(this string input, string pattern, string replacement)
        {
            var match = Regex.Match(input, pattern);
            return input.Substring(0, match.Index) + replacement + input.Substring(match.Index + match.Length);
        }

        public static void ConfigureCategories(this Mock<IFile> fileSystem, IEnumerable<Category> categories, String rootPath)
        {
            const String dataPath = "Data\\Categories.md";
            var categoryFileContents = categories.Serialize();
            var categoryFilePath = System.IO.Path.Combine(rootPath, dataPath);
            fileSystem.ConfigureCategories(categoryFileContents, categoryFilePath);
        }

        public static void ConfigureCategories(this Mock<IFile> fileSystem, String categoryFileContents, String categoryFilePath)
        {
            fileSystem.Setup(f => f.Exists(categoryFilePath)).Returns(true);
            fileSystem.Setup(f => f.ReadAllText(categoryFilePath))
                .Returns(categoryFileContents);
        }

        public static String Serialize(this IEnumerable<Widget> widgets)
        {
            var sb = new StringBuilder();
            sb.AppendLine("---");
            sb.AppendLine("widgets:");

            foreach (var widget in widgets)
                sb.AppendLine(widget.Serialize());

            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();
            return sb.ToString();
        }

        public static String Serialize(this Widget widget)
        {
            var results = new List<String>
            {
                "---",
                $"id: {widget.Id.ToString()}",
                $"title: {widget.Title}",
                $"showtitle: {widget.ShowTitle.ToString().ToLower()}",
                $"widgettype: {widget.WidgetType.ToString()}",
                $"showinsidebar: { widget.ShowInSidebar.ToString().ToLower() }",
                $"orderindex: {widget.OrderIndex.ToString()}",
                "---",
                widget.Dictionary?.SingleOrDefault()?.Item2
            };

            return String.Join("\r\n", results);
        }

        public static String Serialize(this IEnumerable<Tuple<string, string>> dictionary)
        {
            String results = "[]";
            if (dictionary is not null && dictionary.Any())
            {
                results = "\r\n";
                foreach (var (key, value) in dictionary)
                    results += $"  - key: {key}\r\n    value: {value}";
            }

            return results;
        }

        public static String Serialize(this IEnumerable<Category> categories)
        {
            var sb = new StringBuilder();

            sb.AppendLine("---");
            sb.AppendLine("categories:");

            foreach (var category in categories)
            {
                sb.AppendLine($"- id: {category.Id.ToString()}");
                sb.AppendLine($"  name: {category.Name}");
                sb.AppendLine($"  description: {category.Description}");
            }

            sb.AppendLine("");
            sb.AppendLine("---");
            sb.AppendLine("");

            return sb.ToString();
        }

        public static IEnumerable<SourceFile> Create(this IEnumerable<SourceFile>? _, String relativePath, Int32 count)
        {
            var result = new List<SourceFile>();

            for (Int32 i = 0; i < count; i++)
                result.Add((null as SourceFile).Create(relativePath));

            return result;
        }

        private static SourceFile Create(this SourceFile? _, String relativePath)
        {
            return new SourceFile()
            {
                Contents = string.Empty.GetRandom().Select(c => Convert.ToByte(c)).ToArray(),
                FileName = $"{string.Empty.GetRandom()}.{string.Empty.GetRandom(3)}",
                RelativePath = relativePath
            };
        }

        internal static String AsHash<T>(this IEnumerable<T> values)
        {
            String result = string.Empty;

            if (values is not null && values.Any())
            {
                var cleanedValues = values
                    .OrderBy(v => v.ToString().Trim())
                    .Select(v => v.ToString().Trim())
                    .ToArray();
                result = String.Join(";", cleanedValues);
            }

            return result;
        }

        internal static void ExecutePagePropertyTest<T>(this String fileContents, T expected, Func<ContentItem, T> fieldValueDelegate, IEnumerable<Category> categories = null)
        {
            Func<IContentRepository, IEnumerable<ContentItem>> methodDelegate = r => r.GetAllPages();
            ExecutePropertyTest(fileContents, expected, fieldValueDelegate, methodDelegate, categories);
        }

        internal static void ExecutePostPropertyTest<T>(this String fileContents, T expected, Func<ContentItem, T> fieldValueDelegate, IEnumerable<Category> categories = null)
        {
            Func<IContentRepository, IEnumerable<ContentItem>> methodDelegate = r => r.GetAllPosts();
            ExecutePropertyTest(fileContents, expected, fieldValueDelegate, methodDelegate, categories);
        }

        private static void ExecutePropertyTest<T>(String fileContents, T expected, Func<ContentItem, T> fieldValueDelegate, Func<IContentRepository, IEnumerable<ContentItem>> methodDelegate, IEnumerable<Category> categories)
        {
            var fileSystemBuilder = new FileSystemBuilder()
                .AddContentItemFile($"{string.Empty.GetRandom()}.md", fileContents);

            if (categories is not null && categories.Any())
                fileSystemBuilder.AddCategories(categories);
            else
                fileSystemBuilder.AddRandomCategories();

            var directoryProvider = new Mock<IDirectory>();

            directoryProvider.Setup(f => f.EnumerateFiles(It.IsAny<string>()))
                    .Returns(fileSystemBuilder.ContentItemFileNames);

            var target = (null as IContentRepository).Create(fileSystemBuilder.Build(), directoryProvider.Object, "c:\\");
            var contentItems = methodDelegate.Invoke(target);
            var actual = contentItems.ToArray()[0];

            Assert.Equal(expected, fieldValueDelegate(actual));
        }

        public static ContentItemFileBuilder UseRandomValues(this ContentItemFileBuilder builder)
        {
            return builder
                .Tags(new[] { string.Empty.GetRandom() })
                .Id(Guid.NewGuid())
                .Author(string.Empty.GetRandom(10))
                .Title(string.Empty.GetRandom(15))
                .Description(string.Empty.GetRandom(25))
                .IsPublished(true)
                .ShowInList(true)
                .PublicationDate(DateTime.Parse("1/1/2000").AddSeconds(Int32.MaxValue))
                .LastModificationDate(DateTime.Parse("1/1/2000").AddSeconds(Int32.MaxValue))
                .Slug(string.Empty.GetRandom(20))
                .Categories(new[] { string.Empty.GetRandom() })
                .MenuOrder(10.GetRandom())
                .Content(string.Empty.GetRandom(200));
        }

        public static IEnumerable<Category> AsCategoryEntities(this IEnumerable<String> categoryNames)
        {
            return categoryNames.Select(n =>
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = n,
                    Description = $"Category '{n}'"
                });
        }

        public static IEnumerable<SiteVariable> CreateRandom(this IEnumerable<SiteVariable> ignore)
        {
            return ignore.CreateRandom(10.GetRandom(2));
        }

        public static IEnumerable<SiteVariable> CreateRandom(this IEnumerable<SiteVariable> ignore, int count)
        {
            var results = new List<SiteVariable>();
            for (int i = 0; i < count; i++)
                results.Add(new SiteVariable() { Name = String.Empty.GetRandom(), Value = String.Empty.GetRandom() });
            return results;
        }

    }
}