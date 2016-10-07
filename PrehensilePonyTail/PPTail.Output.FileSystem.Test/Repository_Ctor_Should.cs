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

namespace PPTail.Output.FileSystem.Test
{
    public class Repository_Ctor_Should
    {
        [Fact]
        public void ThrowDependencyNotFoundExceptionIfServiceProviderIsNotProvided()
        {
            Assert.Throws(typeof(DependencyNotFoundException), () => new Repository(null));
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfFileProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            container.AddSingleton<Settings>((null as Settings).Create());
            Assert.Throws(typeof(DependencyNotFoundException), () => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfDirectoryProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<Settings>((null as Settings).Create());
            Assert.Throws(typeof(DependencyNotFoundException), () => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfSettingsAreNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            Assert.Throws(typeof(DependencyNotFoundException), () => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowSettingNotFoundExceptionIfOutputPathIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            container.AddSingleton<Settings>(new Settings());
            Assert.Throws(typeof(SettingNotFoundException), () => new Repository(container.BuildServiceProvider()));
        }

    }
}
