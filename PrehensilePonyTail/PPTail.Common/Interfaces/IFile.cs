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
        String ReadAllText(String path);
        byte[] ReadAllBytes(String path);
        void WriteAllText(String path, String data);
        void WriteAllBytes(String path, byte[] data);
        bool Exists(String path);
    }
}
