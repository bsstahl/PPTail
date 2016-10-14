using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.Archive.Test
{
    public class BasicProvider_Ctor_Should
    {
        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheServiceProviderIsNotSupplied()
        {
            Assert.Throws<DependencyNotFoundException>(() => new Archive.BasicProvider(null));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheHomePageTemplateIsNotSupplied()
        {
            var container = new ServiceCollection();
            Assert.Throws<TemplateNotFoundException>(() => new Archive.BasicProvider(container.BuildServiceProvider()));
        }

    }
}
