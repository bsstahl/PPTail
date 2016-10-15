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
        public static string ProcessContentItemTemplate(this Template template, ContentItem item, string sidebarContent, string navContent, SiteSettings siteSettings, ISettings settings)
        {
            return template.Content
                .ReplaceContentItemVariables(settings, siteSettings, item)
                .ReplaceNonContentItemSpecificVariables(settings, siteSettings, sidebarContent, navContent, string.Empty);
        }

        public static string ProcessNonContentItemTemplate(this Template template, string sidebarContent, string navContent, SiteSettings siteSettings, ISettings settings, string content, string pageTitle)
        {
            return template.Content
                .ReplaceNonContentItemSpecificVariables(settings, siteSettings, sidebarContent, navContent, content)
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
    }
}
