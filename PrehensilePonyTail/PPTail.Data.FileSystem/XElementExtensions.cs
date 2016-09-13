using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PPTail.Data.FileSystem
{
    public static class XElementExtensions
    {
        public static string GetElementValue(this XElement node, string localName)
        {
            string result = string.Empty;
            var value = node.Descendants().Where(n => n.Name.LocalName == localName).SingleOrDefault();
            if (node != null)
                result = node.Value;
            return result;
        }
    }
}
