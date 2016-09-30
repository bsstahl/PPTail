using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Io
{
    // TODO: Replace with a more mature implementation
    // such as the System.IO.Abstraction project
    public class File : IFile
    {
        public string ReadAllText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public byte[] ReadAllBytes(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }

        public void WriteAllText(string path, string data)
        {
            System.IO.File.WriteAllText(path, data);
        }

        public bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }
    }
}
