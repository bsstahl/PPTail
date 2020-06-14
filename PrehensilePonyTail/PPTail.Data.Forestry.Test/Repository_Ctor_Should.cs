using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using Xunit;
using TestHelperExtensions;
using Moq;
using PPTail.Entities;
using PPTail.Exceptions;

namespace PPTail.Data.Forestry.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_Ctor_Should
    {
        const string _defaultConnection = "Provider=this;FilePath=c:\\";

        [Fact]
        public void NotThrowAnExceptionIfAllDependenciesAreSupplied() 
        {
            var fileSystem = Mock.Of<IFile>();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);

            var target = new Repository(container.BuildServiceProvider(), _defaultConnection);
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfAFileSystemIsNotProvided()
        {
            var sourceConnection = string.Empty.GetRandom();
            var container = new ServiceCollection();
            Assert.Throws<Exceptions.DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider(), sourceConnection));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfFileSystemIsNotProvided()
        {
            var sourceConnection = string.Empty.GetRandom();
            var container = new ServiceCollection();

            String expected = typeof(IFile).Name;
            try
            {
                var target = new Repository(container.BuildServiceProvider(), sourceConnection);
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheSourceConnectionIsNotSpecified()
        {
            string sourceConnection = null;

            var fileSystem = Mock.Of<IFile>();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);

            Assert.Throws<ArgumentNullException>(() => new Repository(container.BuildServiceProvider(), sourceConnection));
        }

        [Fact]
        public void ThrowAnArgumentExceptionIfTheFilePathIsNotSpecifiedInTheSourceConnection()
        {
            string sourceConnection = "Provider=this";

            var fileSystem = Mock.Of<IFile>();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);

            Assert.Throws<ArgumentException>(() => new Repository(container.BuildServiceProvider(), sourceConnection));
        }
    }
}
