using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IContentEncoder
    {
        string HTMLEncode(string data);

        string XmlEncode(string data);

        string UrlEncode(string data);
    }
}
