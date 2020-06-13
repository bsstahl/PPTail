using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Exceptions;
using PPTail.Interfaces;
using Moq;
using PPTail.Entities;

namespace PPTail.Output.FileSystem.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_Ctor_Should
    {
        const string _defaultConnectionString = "Provider=PPTail.Output.FileSystem.Repository;FilePath=c:\\";

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfServiceProviderIsNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new Repository(null, _defaultConnectionString));
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfFileProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            Assert.Throws<DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider(), _defaultConnectionString));
        }

        [Fact]
        public void ThrowWithTheProperInterfaceTypeNameIfFileProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());

            String expected = typeof(IFile).Name;
            try
            {
                var target = new Repository(container.BuildServiceProvider(), _defaultConnectionString);
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfDirectoryProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            Assert.Throws<DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider(), _defaultConnectionString));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfDirectoryProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());

            String expected = typeof(IDirectory).Name;
            try
            {
                var target = new Repository(container.BuildServiceProvider(), _defaultConnectionString);
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowArgumentNullExceptionIfConnectionStringIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            Assert.Throws<ArgumentNullException>(() => new Repository(container.BuildServiceProvider(), null));
        }

        [Fact]
        public void ThrowArgumentExceptionIfOutputPathIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());

            var connectionString = "Provider=a";

            Assert.Throws<ArgumentException>(() => new Repository(container.BuildServiceProvider(), connectionString));
        }

    }
}
