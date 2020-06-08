using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Templates.Yaml
{
    public static class TemplateExtensions
    {
        internal static Entities.Template ToHtmlContent(this Entities.Template yamlTemplate)
        {
            return new Entities.Template()
            {
                TemplateType = yamlTemplate.TemplateType,
                Content = yamlTemplate.Content.ParseContent()
            };
        }
    }
}
