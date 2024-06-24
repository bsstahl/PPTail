using PPTail.Entities;
using PPTail.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PPTail.Data.Forestry
{
    public static class StringExtensions
    {
        public static string GetRelativePathFrom(this string fileLocation, string containingFolderPath)
        {
            var fullFileLocation = Path.GetFullPath(fileLocation);
            var fullContainingFolderPath = Path.GetFullPath(containingFolderPath);
            var fileName = Path.GetFileName(fullFileLocation);
            var result = fullFileLocation.StartsWith(fullContainingFolderPath, StringComparison.OrdinalIgnoreCase)
                ? fullFileLocation
                    .Substring(fullContainingFolderPath.Length)
                    .Replace(fileName, string.Empty)
                    .Trim(Path.DirectorySeparatorChar)
                : throw new ArgumentException("The file must be in the directory", nameof(containingFolderPath));
            return result;
        }

        public static String Sanitize(this string value)
        {
            return value?
                .Replace(" : ", " -- ")
                .Replace(" :", " -- ")
                .Replace(": ", " -- ")
                .Replace(":", "--");
        }

        public static String ToMarkdown(this string html)
        {
            var config = new ReverseMarkdown.Config() { UnknownTags = ReverseMarkdown.Config.UnknownTagsOption.Bypass };
            var converter = new ReverseMarkdown.Converter(config);
            return converter.Convert(html).Replace("<br>", "\r\n");
        }

        public static String ToHtml(this string markdown, Markdig.MarkdownPipeline markdownPipeline)
        {
            return Markdig.Markdown.ToHtml(markdown, markdownPipeline);
        }

        public static String ConditionalWrap(this string value, char? delimiter)
        {
            return delimiter.HasValue ? $"{delimiter}{value}{delimiter}" : value;
        }

        public static StringBuilder ConditionalAppendLine(this StringBuilder builder, bool addLine, String name, String value)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (addLine)
                _ = string.IsNullOrEmpty(name)
                    ? builder.AppendLine(value)
                    : builder.AppendLine($"{name}: {value}");

            return builder;
        }

        public static StringBuilder ConditionalAppendList(this StringBuilder builder, bool addList, String name, IEnumerable<String> values)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (addList)
            {
                if (values is not null && values.Any())
                {
                    builder.AppendLine($"{name}:");
                    foreach (var value in values ?? Array.Empty<String>())
                        builder.AppendLine($"- {value}");
                }
                else
                    builder.AppendLine($"{name}: []");
            }
            return builder;
        }

        public static StringBuilder ConditionalAppendSiteVariables(this StringBuilder builder, bool addList, String collectionName, String fieldNameKey, String fieldValueKey, IEnumerable<Entities.SiteVariable> values)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (addList)
            {
                if (values is not null && values.Any())
                {
                    builder.AppendLine($"{collectionName}:");
                    foreach (var value in values ?? new List<Entities.SiteVariable>())
                    {
                        builder.AppendLine($"{fieldNameKey}: {value.Name}");
                        builder.AppendLine($"{fieldValueKey}: \"{value.Value}\"");
                    }
                }
                else
                    builder.AppendLine($"{collectionName}: []");
            }
            return builder;
        }

        internal static (string FrontMatter, string Content) ParseForestryYaml(this string value)
        {
            const string HR = "---";
            var fileSections = value.Split(new[] { HR }, StringSplitOptions.RemoveEmptyEntries);
            var frontMatter = fileSections[0];
            var content = String.Join(HR, fileSections.Skip(1));
            return (frontMatter, content);
        }

        public static Entities.SiteSettings ParseSettings(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            var (frontMatter, content) = value.ParseForestryYaml();
            var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(LowerCaseNamingConvention.Instance)
            .Build();

            SiteSettings results = null;
            try
            {
                results = yamlDeserializer
                    .Deserialize<SiteSettings>(frontMatter);
            }
            catch (Exception ex)
            {
                throw new SettingNotFoundException(nameof(SiteSettings), ex);
            }

            return results.AsEntity();
        }

        public static Entities.ContentItem ParseContentItem(this string value, IEnumerable<Category> categories, Markdig.MarkdownPipeline markdownPipeline)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            var (frontMatter, content) = value.ParseForestryYaml();
            var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(LowerCaseNamingConvention.Instance)
            .Build();

            var results = yamlDeserializer
                .Deserialize<ContentItem>(frontMatter);
            results.ByLine = String.IsNullOrEmpty(results.Author) ? String.Empty : $"by {results.Author}";
            results.Content = content.ToHtml(markdownPipeline);

            return results.AsEntity(categories);
        }

        public static Entities.Widget ParseWidget(this string value, Markdig.MarkdownPipeline markdownPipeline)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            var (frontMatter, content) = value.ParseForestryYaml();
            var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(LowerCaseNamingConvention.Instance)
            .Build();

            var results = yamlDeserializer
                .Deserialize<Widget>(frontMatter);

            var entity = results.AsEntity();
            entity.Dictionary = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Content", content.ToHtml(markdownPipeline))
            };

            return entity;
        }

        public static IEnumerable<Entities.Category> ParseCategories(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            var (frontMatter, content) = value.ParseForestryYaml();
            var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(LowerCaseNamingConvention.Instance)
            .Build();

            var results = yamlDeserializer
                .Deserialize<CategoryCollection>(frontMatter);

            return results.AsEntity();
        }
    }
}
