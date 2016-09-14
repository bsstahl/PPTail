using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class Template
    {
        public string Name { get; set; }
        public Enumerations.TemplateType TemplateType { get; set; }
        public string Content { get; set; }
    }
}
