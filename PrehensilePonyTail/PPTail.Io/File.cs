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
        public String ReadAllText(String path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public byte[] ReadAllBytes(String path)
        {
            return System.IO.File.ReadAllBytes(path);
        }

        public void WriteAllText(String path, String data)
        {
            var value = data ?? String.Empty;
            using (var writer = new System.IO.StreamWriter(path))
            {
                writer.NewLine = "\n";
                writer.Write(value.Replace("\r\n", "\n"));
                writer.Flush();
            }
        }

        public void WriteAllBytes(String path, byte[] data)
        {
            System.IO.File.WriteAllBytes(path, data);
        }

        public bool Exists(String path)
        {
            return System.IO.File.Exists(path);
        }
    }
}
