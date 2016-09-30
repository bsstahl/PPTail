using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    // TODO: Replace with a more mature implementation
    // such as the System.IO.Abstraction project
    public interface IFile
    {
        string ReadAllText(string path);
        void WriteAllText(string path, string data);
        IEnumerable<string> EnumerateFiles(string path);
        bool Exists(string path);
    }
}
