using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.FileSystem.FileSystemAbstraction
{
    // TODO: Replace with a more mature implementation
    // such as the System.IO.Abstraction project
    public class Provider : IFileSystem
    {
        public IEnumerable<string> EnumerateFiles(string path)
        {
            return System.IO.Directory.EnumerateFiles(path);
        }

        public string ReadAllText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
    }
}
