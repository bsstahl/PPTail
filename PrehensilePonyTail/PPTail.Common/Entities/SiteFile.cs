using PPTail.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class SiteFile
    {
        public string RelativeFilePath { get; set; }
        public string Content { get; set; }
        public TemplateType SourceTemplateType { get; set; }
    }
}
