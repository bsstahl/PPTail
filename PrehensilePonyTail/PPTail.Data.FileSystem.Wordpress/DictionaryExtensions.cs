using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Data.FileSystem.Wordpress
{
    public static class DictionaryExtensions
    {

        public static string Find(this IDictionary<int, string> dict, int key)
        {
            return dict.Find(key, string.Empty);
        }

        public static string Find(this IDictionary<int, string> dict, int key, string defaultValue)
        {
            string result = defaultValue;
            if (dict.ContainsKey(key))
                result = dict[key];
            return result;
        }
    }
}
