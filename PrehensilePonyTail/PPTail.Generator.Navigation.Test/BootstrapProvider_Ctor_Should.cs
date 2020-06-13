using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Builders;
using Moq;
using PPTail.Interfaces;

namespace PPTail.Generator.Navigation.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class BootstrapProvider_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheServiceProviderIsNotSupplied()
        {
            Assert.Throws<ArgumentNullException>(() => new BootstrapProvider(null));
        }
    }
}
