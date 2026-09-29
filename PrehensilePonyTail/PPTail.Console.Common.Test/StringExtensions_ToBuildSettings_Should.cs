using System;
using PPTail.Console.Common.Extensions;
using PPTail.Enumerations;

namespace PPTail.Console.Common.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class StringExtensions_ToBuildSettings_Should
    {
        private static readonly String[] _requiredArgs = ["source", "target", "template"];

        private static String[] WithSwitches(params String[] switches)
            => [.. _requiredArgs, .. switches];

        [Fact]
        public void DefaultToAutoModeWithNoBaseUrl()
        {
            var (_, _, _, switches) = WithSwitches().ParseArguments();
            var actual = switches.ToBuildSettings();
            Assert.Equal(SitemapMode.Auto, actual.SitemapMode);
            Assert.Null(actual.PublicBaseUrl);
        }

        [Fact]
        public void ReturnDefaultsIfSwitchesAreNull()
        {
            String[]? switches = null;
            var actual = switches.ToBuildSettings();
            Assert.Equal(SitemapMode.Auto, actual.SitemapMode);
            Assert.Null(actual.PublicBaseUrl);
        }

        [Fact]
        public void PreserveTheCaseOfThePublicBaseUrl()
        {
            const String expected = "https://Example.com/My/Blog/";
            var (_, _, _, switches) = WithSwitches($"--PublicBaseUrl={expected}").ParseArguments();
            var actual = switches.ToBuildSettings();
            Assert.Equal(expected, actual.PublicBaseUrl);
        }

        [Theory]
        [InlineData("auto", SitemapMode.Auto)]
        [InlineData("Enabled", SitemapMode.Enabled)]
        [InlineData("DISABLED", SitemapMode.Disabled)]
        public void ParseTheSitemapMode(String value, SitemapMode expected)
        {
            var (_, _, _, switches) = WithSwitches($"--SitemapMode={value}").ParseArguments();
            var actual = switches.ToBuildSettings();
            Assert.Equal(expected, actual.SitemapMode);
        }

        [Fact]
        public void TreatSwitchesWithValuesAsValid()
        {
            var args = WithSwitches("--publicbaseurl=https://example.com/blog/", "--sitemapmode=Enabled", "--verbose");
            (bool isValid, var errors) = args.ValidateParameters("PPTail.Console.Common.Test");
            Assert.True(isValid);
            Assert.Empty(errors);
        }

        [Theory]
        [InlineData("--sitemapmode=sometimes")]
        [InlineData("--sitemapmode=1")]
        [InlineData("--sitemapmode=")]
        [InlineData("--sitemapmode")]
        [InlineData("--publicbaseurl=")]
        [InlineData("--publicbaseurl")]
        public void TreatInvalidOptionValuesAsInvalid(String option)
        {
            (bool isValid, var errors) = WithSwitches(option).ValidateParameters("PPTail.Console.Common.Test");
            Assert.False(isValid);
            Assert.NotEmpty(errors);
        }
    }
}
