using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    // TODO: Replace with a more mature implementation
    // such as the System.IO.Abstraction project

    public interface IDirectory
    {
        bool Exists(String path);
        void CreateDirectory(String path);
        IEnumerable<string> EnumerateFiles(String path);
    }
}
