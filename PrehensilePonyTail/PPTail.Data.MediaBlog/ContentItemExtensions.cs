using Markdig;
using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PPTail.Data.MediaBlog
{
    public static class ContentItemExtensions
    {
        public static ContentItem FromJson(this ContentItem _1, String contentJson, Guid Id)
        {
            var contentItem = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.ContentItem>(contentJson);
            if (contentItem != null)
            {
                contentItem.Id = Id;
                contentItem.ByLine = $"by {contentItem.Author}";
            }

            return contentItem;
        }

        public static ContentItem FromYaml(this ContentItem _1, String contentText, IEnumerable<Category> categories, MarkdownPipeline markdownPipeline)
        {
            ContentItem result = null;

            if (!string.IsNullOrWhiteSpace(contentText))
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(LowerCaseNamingConvention.Instance)
                    .Build();

                var (frontMatter, content) = contentText.SplitYamlFile();

                var yamlContentItem = deserializer.Deserialize<YamlContentItem>(frontMatter);
                yamlContentItem.Content = content.ToHtml(markdownPipeline);

                result = yamlContentItem.AsEntity(categories);
            }

            return result;
        }
    }
}
