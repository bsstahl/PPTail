using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class SourceFile
    {
        public byte[] Contents { get; set; }
        public String FileName { get; set; }
        public String RelativePath { get; set; }
    }
}
