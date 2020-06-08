using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPTail.Templates.Yaml
{
    public static class StringExtensions
    {

        internal static (string FrontMatter, string Content) ParseYaml(this string value)
        {
            const string HR = "---";
            var fileSections = value.Split(new[] { HR }, StringSplitOptions.RemoveEmptyEntries);
            var frontMatter = fileSections[0];
            var content = String.Join(HR, fileSections.Skip(1));
            return (frontMatter, content);
        }

        internal static String ParseContent(this String yamlText)
        {
            var (_, content) = yamlText.ParseYaml();
            return content.Trim();
        }
    }
}
