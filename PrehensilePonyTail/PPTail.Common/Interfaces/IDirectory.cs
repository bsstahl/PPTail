using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IDirectory
    {
        bool Exists(string path);
        void CreateDirectory(string path);
    }
}
