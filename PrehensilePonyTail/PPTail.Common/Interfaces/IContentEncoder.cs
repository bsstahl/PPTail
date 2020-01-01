using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IContentEncoder
    {
        String HTMLEncode(String data);

        String XmlEncode(String data);

        String UrlEncode(String data);
    }
}
