using PPTail.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class SiteFile
    {
        public String RelativeFilePath { get; set; }
        public String Content { get; set; }
        public TemplateType SourceTemplateType { get; set; }
        public bool IsBase64Encoded { get; set; }
    }
}
