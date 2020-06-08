using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace PPTail.Templates.Yaml.Test
{
    public class ReadRepository_Ctor_Should
    {
        [Fact]
        public void SucceedIfAllDependenciesSupplied()
        {
            var serviceProvider = new ServiceCollection()
                .BuildServiceProvider();
            var target = new ReadRepository(serviceProvider);
        }

        [Fact]
        public void ThrowArgumentNullExceptionIfServiceProviderNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ReadRepository(null));
        }
    }
}
