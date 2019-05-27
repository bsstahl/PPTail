using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.MediaBlog.Test
{
    internal class MockMediaFile
    {
        public Guid Id { get; set; }
        public string Extension { get; set; } = "json";
        public string FolderPath { get; set; }
        public string Contents { get; set; }

        public string GetFilename()
        {
            return $"{this.Id.ToString()}.{this.Extension}";
        }

        public string GetFullPath()
        {
            return System.IO.Path.Combine(this.FolderPath, this.GetFilename());
        }
    }
}
