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
        byte[] ReadAllBytes(string path);
        void WriteAllText(string path, string data);
        void WriteAllBytes(string path, byte[] data);
        bool Exists(string path);
    }
}
