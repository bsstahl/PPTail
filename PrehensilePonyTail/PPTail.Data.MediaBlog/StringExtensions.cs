using PPTail.Entities;
using PPTail.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PPTail.Data.MediaBlog
{
    public static class StringExtensions
    {
        const string HR = "---";

        internal static (string FrontMatter, string Content) SplitYamlFile(this string value)
        {
            var fileSections = value.Split(new[] { HR }, StringSplitOptions.RemoveEmptyEntries);
            var frontMatter = fileSections[0];
            var content = String.Join(HR, fileSections.Skip(1));
            return (frontMatter, content);
        }

        public static String ToHtml(this string markdown, Markdig.MarkdownPipeline markdownPipeline)
        {
            return Markdig.Markdown.ToHtml(markdown, markdownPipeline);
        }

        public static Entities.Widget ParseWidgetYaml(this string value, Markdig.MarkdownPipeline markdownPipeline)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            var (frontMatter, content) = value.SplitYamlFile();
            var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(LowerCaseNamingConvention.Instance)
            .Build();

            var results = yamlDeserializer
                .Deserialize<YamlWidget>(frontMatter);
            var entity = results.AsEntity();
            
            entity.Dictionary = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Content", content.ToHtml(markdownPipeline))
            };

            return entity;
        }

        public static Entities.SiteSettings ParseYamlSettings(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            var (frontMatter, content) = value.SplitYamlFile();
            var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(LowerCaseNamingConvention.Instance)
            .Build();

            YamlSiteSettings results = null;
            try
            {
                results = yamlDeserializer
                    .Deserialize<YamlSiteSettings>(frontMatter);
            }
            catch (Exception ex)
            {
                throw new SettingNotFoundException(nameof(SiteSettings), ex);
            }

            return results.AsEntity();
        }



    }
}
