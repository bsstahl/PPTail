using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog.Test
{
    public class SecurableSourceFile: SourceFile
    {
        public SecurableSourceFile(SourceFile sourceFile)
        {
            this.Load(sourceFile);
        }

        public SecurableSourceFile(SourceFile sourceFile, bool isSecured)
        {
            this.Load(sourceFile);
            this.IsSecured = isSecured;
        }

        public bool IsSecured { get; set; }

        private void Load(SourceFile s)
        {
            this.IsSecured = false;
            this.Contents = s.Contents;
            this.FileName = s.FileName;
            this.RelativePath = s.RelativePath;
        }
    }
}
