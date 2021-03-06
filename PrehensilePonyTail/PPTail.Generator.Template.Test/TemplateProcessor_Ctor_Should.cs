﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using TestHelperExtensions;
using PPTail.Exceptions;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Generator.Template.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TemplateProcessor_Ctor_Should
    {
        [Fact]
        public void ThrowArgumentNullExceptionIfServiceProviderNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new TemplateProcessor(null));
        }

        [Fact]
        public void ThrowWithTheProperArgumentNameIfServiceProviderNotProvided()
        {
            String expected = "serviceProvider";
            String actual = string.Empty;
            try
            {
                var target = new TemplateProcessor(null);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }

            Assert.Equal(expected, actual);
        }

    }
}
