using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface ILinkProvider
    {
        String GetUrl(String pathToRoot, String relativePath, String fileName);

        String GetUrl(String pathToRoot, String relativePath, String fileName, String fileExtension);
    }
}
