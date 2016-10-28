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
