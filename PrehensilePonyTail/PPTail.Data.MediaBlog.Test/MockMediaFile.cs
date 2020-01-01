using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog.Test
{
    internal class MockMediaFile
    {
        public Guid Id { get; set; }
        public String Extension { get; set; } = "json";
        public String FolderPath { get; set; }
        public String Contents { get; set; }

        public String GetFilename()
        {
            return $"{this.Id.ToString()}.{this.Extension}";
        }

        public String GetFullPath()
        {
            return System.IO.Path.Combine(this.FolderPath, this.GetFilename());
        }
    }
}
