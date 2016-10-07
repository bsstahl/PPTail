using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Entities
{
    public class SourceFile
    {
        public byte[] Contents { get; set; }
        public string FileName { get; set; }
        public string RelativePath { get; set; }
    }
}
