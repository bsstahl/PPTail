using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface ILinkProvider
    {
        string GetUrl(string pathToRoot, string relativePath, string fileName);

        string GetUrl(string pathToRoot, string relativePath, string fileName, string fileExtension);
    }
}
