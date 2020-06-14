using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using Xunit;
using TestHelperExtensions;
using Moq;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Builders;

namespace PPTail.Data.MediaBlog.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Repository_Ctor_Should
    {
        const string _defaultConnection = "Provider=this;FilePath=c:\\";

        [Fact]
        public void NotThrowAnExceptionIfAllDependenciesAreSupplied()
        {
            var fileSystem = Mock.Of<IFile>();
            var directory = Mock.Of<IDirectory>();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);
            container.AddSingleton<IDirectory>(directory);

            var serviceProvider = container.BuildServiceProvider();
            var target = new Repository(serviceProvider, _defaultConnection);
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfAFileSystemIsNotProvided()
        {
            var container = new ServiceCollection();
            Assert.Throws<Exceptions.DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider(), _defaultConnection));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfFileSystemIsNotProvided()
        {
            var container = new ServiceCollection();
            String expected = typeof(IFile).Name;
            try
            {
                var target = new Repository(container.BuildServiceProvider(), _defaultConnection);
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheSourceConnectionIsNotSpecified()
        {
            string connection = null;

            var fileSystem = Mock.Of<IFile>();
            var directory = Mock.Of<IDirectory>();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);
            container.AddSingleton<IDirectory>(directory);

            Assert.Throws<ArgumentNullException>(() => new Repository(container.BuildServiceProvider(), connection));
        }

        [Fact]
        public void ThrowAnArgumentExceptionIfTheFilePathIsNotSpecifiedInTheSourceConnection()
        {
            string connection = "Provider=this";

            var fileSystem = Mock.Of<IFile>();
            var directory = Mock.Of<IDirectory>();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);
            container.AddSingleton<IDirectory>(directory);

            Assert.Throws<ArgumentException>(() => new Repository(container.BuildServiceProvider(), connection));
        }
    }
}
