using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;
using System;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.FileSystem.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ReadRepository_Ctor_Should
    {
        const string _defaultConnection = "Provider=PPTail.Templates.FileSystem.ReadRepository;FilePath=c:\\";

        [Fact]
        public void SucceedIfAllDependenciesSupplied()
        {
            var serviceProvider = new ServiceCollection()
                .AddFileService()
                .BuildServiceProvider();
            var target = new ReadRepository(serviceProvider, _defaultConnection);
        }

        [Fact]
        public void ThrowArgumentNullExceptionIfServiceProviderNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ReadRepository(null, _defaultConnection));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheFileServiceDependencyNotProvided()
        {
            var serviceProvider = new ServiceCollection()
                .BuildServiceProvider();
            Assert.Throws<DependencyNotFoundException>(() => new ReadRepository(serviceProvider, _defaultConnection));
        }

        [Fact]
        public void ThrowArgumentExceptionIfFilePathNotProvidedInConnectionString()
        {
            string templatePath = string.Empty.GetRandom();
            string connection = $"Provider=PPTail.Templates.FileSystem.ReadRepository";

            var serviceProvider = new ServiceCollection()
                .AddFileService()
                .BuildServiceProvider();
            Assert.Throws<ArgumentException>(() => _ = new ReadRepository(serviceProvider, templatePath));
        }

    }
}
