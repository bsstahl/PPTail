using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Io
{
    public class Directory : IDirectory
    {
        public void CreateDirectory(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        public bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            return System.IO.Directory.EnumerateFiles(path);
        }

    }
}
