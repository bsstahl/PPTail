using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PPTail;
using TestHelperExtensions;

namespace PPTail.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class StringExtensions_ParseArguments_Should
    {
        [Fact]
        public void ReturnNullForSourceConnectionIfArgsIsNull()
        {
            string[] target = null;
            (String actualSource, String actualTarget, String actualTemplate, var switches) = target.ParseArguments();
            Assert.Null(actualSource);
        }

        [Fact]
        public void ReturnNullForTargetConnectionIfArgsIsNull()
        {
            string[] target = null;
            (String actualSource, String actualTarget, String actualTemplate, var switches) = target.ParseArguments();
            Assert.Null(actualTarget);
        }

        [Fact]
        public void ReturnNullForTemplateConnectionIfArgsIsNull()
        {
            string[] target = null;
            (String actualSource, String actualTarget, String actualTemplate, var switches) = target.ParseArguments();
            Assert.Null(actualTemplate);
        }

        [Fact]
        public void ReturnTheSpecifiedValueForSourceConnection()
        {
            String expectedSource = string.Empty.GetRandom();
            String expectedTarget = string.Empty.GetRandom();
            String expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate};
            (String actualSource, String actualTarget, String actualTemplate, var switches) = target.ParseArguments();
            Assert.Equal(expectedSource, actualSource);
        }

        [Fact]
        public void ReturnTheSpecifiedValueForTargetConnection()
        {
            String expectedSource = string.Empty.GetRandom();
            String expectedTarget = string.Empty.GetRandom();
            String expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (String actualSource, String actualTarget, String actualTemplate, var switches) = target.ParseArguments();
            Assert.Equal(expectedTarget, actualTarget);
        }

        [Fact]
        public void ReturnTheSpecifiedValueForTemplateConnection()
        {
            String expectedSource = string.Empty.GetRandom();
            String expectedTarget = string.Empty.GetRandom();
            String expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (String actualSource, String actualTarget, String actualTemplate, var switches) = target.ParseArguments();
            Assert.Equal(expectedTemplate, actualTemplate);
        }
    }
}
