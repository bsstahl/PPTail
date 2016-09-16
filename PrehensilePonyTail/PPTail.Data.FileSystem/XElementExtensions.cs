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
            var childNode = node.Descendants().Where(n => n.Parent == node && n.Name.LocalName == localName).SingleOrDefault();
            if (childNode != null)
                result = childNode.Value;
            return result;
        }


    }
}
