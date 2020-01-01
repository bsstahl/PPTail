using System;
using System.Collections.Generic;
using System.Text;

namespace PPTConvert
{
    public static class StringExtensions
    {

        public static (String sourceConnection, String targetConnection) ParseArguments(this string[] args)
        {
            if (args is null)
                return (null, null);
            else
                return (args[0], args[1]);
        }


        public static (bool argsAreValid, IEnumerable<string> argumentErrors) ValidateArguments(this string[] args)
        {
            const Int32 expectedArgCount = 2;

            var errors = new List<string>();
            bool isValid = ((args?.Length == expectedArgCount) && !args.IsNullOrWhiteSpace());

            if ((args is null) || (args.Length != expectedArgCount))
                errors.Add("Usage - PPTConvert.exe SourceConnectionString TargetConnectionString");
            else
            {
                if (string.IsNullOrEmpty(args[0]))
                    errors.Add("A value must be supplied for the SourceConnectionString argument");

                if (string.IsNullOrEmpty(args[1]))
                    errors.Add("A value must be supplied for the TargetConnectionString argument");
            }

            return (isValid, errors);
        }


        /// <summary>
        /// Checks to see if any of the supplied values are null or empty whitespace
        /// </summary>
        /// <param name="args">A String array containing the strings to be checked for nullness</param>
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
