using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PPTail;
using TestHelperExtensions;

namespace PPTail.Test
{
    public class StringExtensions_ParseArguments_Should
    {
        [Fact]
        public void ReturnNullForSourceConnectionIfArgsIsNull()
        {
            string[] target = null;
            (string actualSource, string actualTarget, string actualTemplate) = target.ParseArguments();
            Assert.Null(actualSource);
        }

        [Fact]
        public void ReturnNullForTargetConnectionIfArgsIsNull()
        {
            string[] target = null;
            (string actualSource, string actualTarget, string actualTemplate) = target.ParseArguments();
            Assert.Null(actualTarget);
        }

        [Fact]
        public void ReturnNullForTemplateConnectionIfArgsIsNull()
        {
            string[] target = null;
            (string actualSource, string actualTarget, string actualTemplate) = target.ParseArguments();
            Assert.Null(actualTemplate);
        }

        [Fact]
        public void ReturnTheSpecifiedValueForSourceConnection()
        {
            string expectedSource = string.Empty.GetRandom();
            string expectedTarget = string.Empty.GetRandom();
            string expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate};
            (string actualSource, string actualTarget, string actualTemplate) = target.ParseArguments();
            Assert.Equal(expectedSource, actualSource);
        }

        [Fact]
        public void ReturnTheSpecifiedValueForTargetConnection()
        {
            string expectedSource = string.Empty.GetRandom();
            string expectedTarget = string.Empty.GetRandom();
            string expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (string actualSource, string actualTarget, string actualTemplate) = target.ParseArguments();
            Assert.Equal(expectedTarget, actualTarget);
        }

        [Fact]
        public void ReturnTheSpecifiedValueForTemplateConnection()
        {
            string expectedSource = string.Empty.GetRandom();
            string expectedTarget = string.Empty.GetRandom();
            string expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (string actualSource, string actualTarget, string actualTemplate) = target.ParseArguments();
            Assert.Equal(expectedTemplate, actualTemplate);
        }
    }
}
