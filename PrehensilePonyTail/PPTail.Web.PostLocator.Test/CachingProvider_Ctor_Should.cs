using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Web.PostLocator.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CachingProvider_Ctor_Should
    {
        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheFileProviderIsNotSupplied()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IFile>();
            var serviceProvider = container.BuildServiceProvider();
            Assert.Throws<DependencyNotFoundException>(() => new CachingProvider(serviceProvider));
        }

        [Fact]
        public void ReturnTheProperDependencyNameIfTheFileProviderIsNotSupplied()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IFile>();
            var serviceProvider = container.BuildServiceProvider();

            String actual = string.Empty;
            try
            {
                var target = new CachingProvider(serviceProvider);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            Assert.Equal("IFile", actual);
        }
    }
}
