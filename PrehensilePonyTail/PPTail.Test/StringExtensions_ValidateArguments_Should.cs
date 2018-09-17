using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PPTail;
using TestHelperExtensions;

namespace PPTail.Test
{
    public class StringExtensions_ValidateArguments_Should
    {
        [Fact]
        public void ReturnInvalidIfArgsIsNull()
        {
            string[] target = null;
            (bool isValid, var errors) = target.ValidateArguments();
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnValidIfAllArgumentsSupplied()
        {
            string expectedSource = string.Empty.GetRandom();
            string expectedTarget = string.Empty.GetRandom();
            string expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (bool isValid, var errors) = target.ValidateArguments();
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnInvalidIfTheWrongNumberOfArgumentsAreSupplied()
        {
            string expectedSource = string.Empty.GetRandom();
            string expectedTarget = string.Empty.GetRandom();
            string expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate, expectedSource };
            (bool isValid, var errors) = target.ValidateArguments();
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnInvalidIfSourceArgumentMissing()
        {
            string expectedSource = string.Empty;
            string expectedTarget = string.Empty.GetRandom();
            string expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (bool isValid, var errors) = target.ValidateArguments();
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnInvalidIfTargetArgumentMissing()
        {
            string expectedSource = string.Empty.GetRandom();
            string expectedTarget = string.Empty;
            string expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (bool isValid, var errors) = target.ValidateArguments();
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnInvalidIfTemplateArgumentMissing()
        {
            string expectedSource = string.Empty.GetRandom();
            string expectedTarget = string.Empty.GetRandom();
            string expectedTemplate = string.Empty;
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (bool isValid, var errors) = target.ValidateArguments();
            Assert.False(isValid);
        }

        // TODO: Add tests to validate the errors return parameter
    }
}
