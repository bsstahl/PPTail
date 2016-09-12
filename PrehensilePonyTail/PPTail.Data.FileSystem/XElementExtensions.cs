using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PPTail.Data.FileSystem
{
    public static class XElementExtensions
    {
        public static string GetElementValue(this XElement node, string childElementLocalName)
        {
            string result = string.Empty;
            var childNodes = node.Descendants().Where(d => d.Parent == node && d.Name.LocalName == childElementLocalName);
            if (childNodes.Any())
                result = childNodes.Single().Value;
            return result;
        }
    }
}
