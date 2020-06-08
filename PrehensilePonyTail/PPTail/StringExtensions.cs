using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using PPTail.Extensions;

namespace PPTail
{
    public static class StringExtensions
    {

        public static string CombinePaths(this string path1, string path2)
        {
            return System.IO.Path.Combine(path1, path2);
        }

        public static (String sourceConnection, String targetConnection, String templateConnection, String[] switches) ParseArguments(this string[] allArgs)
        {
            (String, String, String, String[]) result = (null, null, null, null);
            if (allArgs.IsNotNull())
            {
                var (args, switches) = SeparateArgumentsAndSwitches(allArgs);
                result = (args[0], args[1], args[2], switches);
            }
            return result;
        }


        public static (bool argsAreValid, IEnumerable<string> argumentErrors) ValidateParameters(this string[] allArgs)
        {
            var (args, switches) = allArgs.SeparateArgumentsAndSwitches();

            var errors = new List<string>();
            var isValid = ValidateArguments(errors, args) && ValidateSwitches(errors, switches);

            return (isValid, errors);
        }

        private static Boolean ValidateArguments(List<String> errors, String[] args)
        {
            bool isValid = ((args?.Length == 3) && !args.IsNullOrWhiteSpace());

            if ((args is null) || (args.Length != 3))
                errors.Add("Usage - PPTail.exe SourceConnectionString TargetConnectionString TemplatePath [--Switches]");
            else
            {
                if (string.IsNullOrEmpty(args[0]))
                    errors.Add("A value must be supplied for the SourceConnectionString argument");

                if (string.IsNullOrEmpty(args[1]))
                    errors.Add("A value must be supplied for the TargetConnectionString argument");

                if (string.IsNullOrEmpty(args[2]))
                    errors.Add("A value must be supplied for the TemplatePath argument");
            }

            return isValid;
        }

        public static Boolean ValidateSwitches(List<String> errors, string[] switches)
        {
            bool result = true;

            if (switches.IsNotNull())
                foreach (var item in switches ?? Array.Empty<String>())
                {
                    switch (item)
                    {
                        // Add switches here

                        case Constants.VALIDATEONLY_SWITCH:
                            break;

                        default:
                            result = false;
                            errors?.Add($"Invalid Switch '{item}'");
                            break;
                    }
                }

            return result;
        }

        public static (String[] args, String[] switches) SeparateArgumentsAndSwitches(this string[] allArgs)
        {
            const string SWITCH_KEY = "--";
            var argResults = allArgs?.Where(a => !a.StartsWith(SWITCH_KEY, StringComparison.InvariantCulture)).ToArray();
            var switchResults = allArgs?.Where(a => a.StartsWith(SWITCH_KEY, StringComparison.InvariantCulture)).Select(a => a.ToLower(CultureInfo.InvariantCulture)).ToArray();
            return (argResults, switchResults);
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
