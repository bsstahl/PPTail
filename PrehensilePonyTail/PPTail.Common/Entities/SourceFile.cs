using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class SourceFile
    {
        public Byte[] Contents { get; set; }
        public String FileName { get; set; }
        public String RelativePath { get; set; }
    }
}
