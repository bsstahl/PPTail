using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class TemplateExtensions
    {
        public static string ProcessContentItemTemplate(this Template template, IServiceProvider serviceProvider, ContentItem item, string sidebarContent, string navContent, string pathToRoot, bool xmlEncodeContent)
        {
            return template.Content
                .ReplaceContentItemVariables(serviceProvider, item, pathToRoot, xmlEncodeContent)
                .ReplaceNonContentItemSpecificVariables(serviceProvider, sidebarContent, navContent, string.Empty);
        }

        public static string ProcessNonContentItemTemplate(this Template template, IServiceProvider serviceProvider, string sidebarContent, string navContent, string content, string pageTitle)
        {
            return template.Content
                .ReplaceNonContentItemSpecificVariables(serviceProvider, sidebarContent, navContent, content)
                .Replace("{Title}", pageTitle);
        }

        public static void Validate(this IEnumerable<Template> templates, TemplateType templateType)
        {
            if (!templates.Any(t => t.TemplateType == templateType))
                throw new Exceptions.TemplateNotFoundException(templateType, string.Empty);
        }

        public static Template Find(this IEnumerable<Template> templates, TemplateType templateType)
        {
            // TODO: Handle multiple templates of the same type
            templates.Validate(templateType);
            return templates.Single(t => t.TemplateType == templateType);
        }

        public static bool Contains(this IEnumerable<Template> templates, TemplateType templateType)
        {
            return templates.Any(t => t.TemplateType == templateType);
        }

    }
}
