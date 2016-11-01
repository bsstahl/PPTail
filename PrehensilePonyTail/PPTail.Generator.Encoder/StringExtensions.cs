using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.Encoder
{
    public static class StringExtensions
    {
        internal static string RemoveConsecutiveDashes(this string data)
        {
            string original = string.Empty;
            string current = data;

            do
            {
                original = current;
                current = current.Replace("--", "-");
            } while (current != original);

            return current;
        }


    }
}
