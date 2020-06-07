﻿using System;
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
            (bool isValid, var errors) = target.ValidateParameters();
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnValidIfAllArgumentsSupplied()
        {
            String expectedSource = string.Empty.GetRandom();
            String expectedTarget = string.Empty.GetRandom();
            String expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (bool isValid, var errors) = target.ValidateParameters();
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnInvalidIfTheWrongNumberOfArgumentsAreSupplied()
        {
            String expectedSource = string.Empty.GetRandom();
            String expectedTarget = string.Empty.GetRandom();
            String expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate, expectedSource };
            (bool isValid, var errors) = target.ValidateParameters();
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnInvalidIfSourceArgumentMissing()
        {
            String expectedSource = string.Empty;
            String expectedTarget = string.Empty.GetRandom();
            String expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (bool isValid, var errors) = target.ValidateParameters();
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnInvalidIfTargetArgumentMissing()
        {
            String expectedSource = string.Empty.GetRandom();
            String expectedTarget = string.Empty;
            String expectedTemplate = string.Empty.GetRandom();
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (bool isValid, var errors) = target.ValidateParameters();
            Assert.False(isValid);
        }

        [Fact]
        public void ReturnInvalidIfTemplateArgumentMissing()
        {
            String expectedSource = string.Empty.GetRandom();
            String expectedTarget = string.Empty.GetRandom();
            String expectedTemplate = string.Empty;
            var target = new string[] { expectedSource, expectedTarget, expectedTemplate };
            (bool isValid, var errors) = target.ValidateParameters();
            Assert.False(isValid);
        }

        // TODO: Add tests to validate the errors return parameter
    }
}
