using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.FileSystem
{
    // TODO: Replace with a more mature implementation
    // such as the System.IO.Abstraction project
    public interface IFileSystem
    {
        string ReadAllText(string path);
        IEnumerable<string> EnumerateFiles(string path);
    }
}
