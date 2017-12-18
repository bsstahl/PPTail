using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.FileSystem.Wordpress
{
    public static class JsonExtensions
    {

        public static DateTime ParseDate(this JToken node, string nodeName)
        {
            DateTime result = DateTime.MinValue;
            string dateText = node[nodeName]?.ToString();
            if (!string.IsNullOrEmpty(dateText))
                result = DateTime.Parse(dateText);
            return result;
        }

        public static int ParseInt32(this JToken node, string nodeName)
        {
            if (!Int32.TryParse(node[nodeName]?.ToString(), out int result))
                result = 0;
            return result;
        }
    }
}
