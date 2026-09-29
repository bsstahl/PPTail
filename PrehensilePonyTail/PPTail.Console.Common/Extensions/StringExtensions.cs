using System;
using System.Collections.ObjectModel;
using System.Globalization;
using PPTail.Entities;
using PPTail.Enumerations;

namespace PPTail.Console.Common.Extensions;

public static class StringExtensions
{

    public static string CombinePaths(this string path1, string path2)
    {
        return System.IO.Path.Combine(path1, path2);
    }

    public static (String? sourceConnection, String? targetConnection, String? templateConnection, String[] switches) ParseArguments(this string[]? allArgs)
    {
        (String?, String?, String?, String[]) result = (null, null, null, []);
        if (allArgs is not null)
        {
            var (args, switches) = SeparateArgumentsAndSwitches(allArgs);
            result = (args[0], args[1], args[2], switches);
        }
        return result;
    }


    public static (bool argsAreValid, IEnumerable<string> argumentErrors) ValidateParameters(this string[] allArgs, string executable)
    {
        var (args, switches) = allArgs.SeparateArgumentsAndSwitches();

        var errors = new Collection<string>();
        var isValid = ValidateArguments(errors, args, executable) && ValidateSwitches(errors, switches);

        return (isValid, errors);
    }

    private static Boolean ValidateArguments(Collection<String> errors, String[] args, string executable)
    {
        bool isValid = ((args?.Length == 3) && !args.IsNullOrWhiteSpace());

        if ((args is null) || (args.Length != 3))
            errors.Add($"Usage - {executable} SourceConnectionString TargetConnectionString TemplateConnectionString [--Switches] [{Constants.PUBLICBASEURL_OPTION}=https://example.com/blog/] [{Constants.SITEMAPMODE_OPTION}=Auto|Enabled|Disabled]");
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

    private static Boolean ValidateSwitches(Collection<String> errors, string[] switches)
    {
        bool result = true;

        foreach (var item in switches ?? Array.Empty<String>())
        {
            var (name, value) = item.SplitOption();
            switch (name)
            {
                // Add switches here

                case Constants.VALIDATEONLY_SWITCH:
                    break;

                case Constants.VERBOSE_SWITCH:
                    break;

                case Constants.PUBLICBASEURL_OPTION:
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        result = false;
                        errors?.Add($"A value must be supplied for the '{Constants.PUBLICBASEURL_OPTION}' option, e.g. {Constants.PUBLICBASEURL_OPTION}=https://example.com/blog/");
                    }
                    break;

                case Constants.SITEMAPMODE_OPTION:
                    if (!value.TryParseSitemapMode(out _))
                    {
                        result = false;
                        errors?.Add($"Invalid value '{value}' for the '{Constants.SITEMAPMODE_OPTION}' option. Valid values are Auto, Enabled or Disabled");
                    }
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
        var switchResults = allArgs?.Where(a => a.StartsWith(SWITCH_KEY, StringComparison.InvariantCulture)).Select(a => a.NormalizeSwitch()).ToArray();
        return (argResults, switchResults);
    }

    /// <summary>
    /// Lower-cases the name of a switch or option while preserving the case of any option value
    /// (e.g. --PublicBaseUrl=https://example.com/Blog/ becomes --publicbaseurl=https://example.com/Blog/)
    /// </summary>
    private static String NormalizeSwitch(this String value)
    {
        var (name, optionValue) = value.SplitOption();
        return optionValue is null
            ? name
            : $"{name}{Constants.OPTION_VALUE_SEPARATOR}{optionValue}";
    }

    private static (String name, String? value) SplitOption(this String value)
    {
        Int32 separatorIndex = value.IndexOf(Constants.OPTION_VALUE_SEPARATOR);
        return separatorIndex < 0
            ? (value.ToLower(CultureInfo.InvariantCulture), null)
            : (value.Substring(0, separatorIndex).ToLower(CultureInfo.InvariantCulture), value.Substring(separatorIndex + 1));
    }

    private static bool TryParseSitemapMode(this String? value, out SitemapMode result)
    {
        result = SitemapMode.Auto;
        return !string.IsNullOrWhiteSpace(value)
            && !value!.Trim().All(char.IsDigit)
            && Enum.TryParse(value.Trim(), true, out result)
            && Enum.IsDefined(typeof(SitemapMode), result);
    }

    /// <summary>
    /// Creates the build-level settings from the supplied switches and options
    /// (--publicbaseurl=value and --sitemapmode=Auto|Enabled|Disabled)
    /// </summary>
    public static BuildSettings ToBuildSettings(this String[]? switches)
    {
        var result = new BuildSettings();
        foreach (var item in switches ?? Array.Empty<String>())
        {
            var (name, value) = item.SplitOption();
            if (name == Constants.PUBLICBASEURL_OPTION)
                result.PublicBaseUrl = value?.Trim();
            else if (name == Constants.SITEMAPMODE_OPTION && value.TryParseSitemapMode(out var mode))
                result.SitemapMode = mode;
        }
        return result;
    }

    /// <summary>
    /// Checks to see if any of the supplied values are null or empty whitespace
    /// </summary>
    /// <param name="args">A String array containing the strings to be checked for nullness</param>
    /// <returns>Returns TRUE if ANY of the supplied values are null or empty whitespace, FALSE if
    /// all contain values.  Returns FALSE if the argument array is empty or null since none of
    /// it's containing values are then null or whitespace (since there are none).</returns>
    public static bool IsNullOrWhiteSpace(this string[]? args)
    {
        bool result = false;
        if (!(args is null))
            foreach (var arg in args)
                result = (result || string.IsNullOrWhiteSpace(arg));
        return result;
    }

}
