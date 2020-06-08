using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;
using System;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.FileSystem.Test
{
    public class ReadRepository_Ctor_Should
    {
        [Fact]
        public void SucceedIfAllDependenciesSupplied()
        {
            var serviceProvider = new ServiceCollection()
                .AddDirectoryService()
                .BuildServiceProvider();
            var target = new ReadRepository(serviceProvider, String.Empty.GetRandom());
        }

        [Fact]
        public void ThrowArgumentNullExceptionIfServiceProviderNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ReadRepository(null, String.Empty.GetRandom()));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheDirectoryServiceDependencyNotProvided()
        {
            string templatePath = string.Empty.GetRandom();
            var serviceProvider = new ServiceCollection()
                .BuildServiceProvider();
            Assert.Throws<DependencyNotFoundException>(() => new ReadRepository(serviceProvider, templatePath));
        }

    }
}
