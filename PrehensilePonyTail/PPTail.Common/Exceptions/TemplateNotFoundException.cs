using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Exceptions
{
    public class TemplateNotFoundException : Exception
    {
        public Enumerations.TemplateType TemplateType { get; set; }

        public string TemplateName { get; set; }


        public TemplateNotFoundException(Enumerations.TemplateType templateType, string templateName) 
            : base($"Unable to load template '{templateName}' of type '{templateType.ToString()}'")
        {
            this.TemplateType = templateType;
            this.TemplateName = templateName;
        }
    }
}
