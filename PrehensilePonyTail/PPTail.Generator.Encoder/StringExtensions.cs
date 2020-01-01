using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.Encoder
{
    public static class StringExtensions
    {
        internal static String RemoveConsecutiveDashes(this String data)
        {
            String original = string.Empty;
            String current = data;

            do
            {
                original = current;
                current = current.Replace("--", "-");
            } while (current != original);

            return current;
        }


    }
}
