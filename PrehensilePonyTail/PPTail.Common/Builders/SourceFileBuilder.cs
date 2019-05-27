using System;
using System.Collections.Generic;
using System.Text;
using PPTail.Entities;

namespace PPTail.Common.Builders
{
    public class SourceFileBuilder : SourceFile
    {
        public SourceFile Build()
        {
            return this;
        }

        public new SourceFileBuilder Contents(byte[] contents)
        {
            base.Contents = contents;
            return this;
        }

        public new SourceFileBuilder FileName(string fileName)
        {
            base.FileName = fileName;
            return this;
        }

        public new SourceFileBuilder RelativePath(string path)
        {
            base.RelativePath = path;
            return this;
        }
    }
}
