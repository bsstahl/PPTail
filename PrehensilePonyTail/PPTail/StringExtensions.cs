using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail
{
    public static class StringExtensions
    {

        public static (string sourceConnection, string targetConnection, string templateConnection) ParseArguments(this string[] args)
        {
            if (args is null)
                return (null, null, null);
            else
                return (args[0], args[1], args[2]);
        }


        public static (bool argsAreValid, string[] argumentErrors) ValidateArguments(this string[] args)
        {
            string[] errors = new string[] { };
            bool isValid = ((args?.Length == 3) && !args.IsNullOrWhiteSpace());
            return (isValid, errors);
        }


        /// <summary>
        /// Checks to see if any of the supplied values are null or empty whitespace
        /// </summary>
        /// <param name="args">A string array containing the strings to be checked for nullness</param>
        /// <returns>Returns TRUE if ANY of the supplied values are null or empty whitespace, FALSE if
        /// all contain values.  Returns FALSE if the argument array is empty or null since none of
        /// it's containing values are then null or whitespace (since there are none).</returns>
        public static bool IsNullOrWhiteSpace(this string[] args)
        {
            bool result = false;
            if (!(args is null))
                foreach (var arg in args)
                    result = (result || string.IsNullOrWhiteSpace(arg));
            return result;
        }
    }
}
