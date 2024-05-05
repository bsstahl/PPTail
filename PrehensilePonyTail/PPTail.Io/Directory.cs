using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Io
{
    public class Directory : IDirectory
    {
        public void CreateDirectory(String path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        public bool Exists(String path)
        {
            return System.IO.Directory.Exists(path);
        }

        public IEnumerable<string> EnumerateFiles(String path)
        {
            return EnumerateFiles(path, false);
        }

        public IEnumerable<string> EnumerateFiles(string path, bool recursive)
        {
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return System.IO.Directory.EnumerateFiles(path, "*", searchOption);
        }
    }
}
