using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Exceptions;
using System;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Templates.Yaml.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ReadRepository_Ctor_Should
    {
        [Fact]
        public void SucceedIfAllDependenciesSupplied()
        {
            string templatePath = string.Empty.GetRandom();
            var serviceProvider = new ServiceCollection()
                .AddFileService()
                .BuildServiceProvider();
            var target = new ReadRepository(serviceProvider, templatePath);
        }

        [Fact]
        public void ThrowArgumentNullExceptionIfServiceProviderNotProvided()
        {
            string templatePath = string.Empty.GetRandom();
            IServiceProvider serviceProvider = null;
            Assert.Throws<ArgumentNullException>(() => _ = new ReadRepository(serviceProvider, templatePath));
        }

    }
}
