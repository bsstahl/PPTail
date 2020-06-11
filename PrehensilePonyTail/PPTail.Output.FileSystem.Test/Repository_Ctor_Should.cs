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
        [Fact]
        public void ThrowDependencyNotFoundExceptionIfServiceProviderIsNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new Repository(null));
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfFileProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            container.AddSingleton<ISettings>((null as ISettings).Create());
            Assert.Throws<DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithTheProperInterfaceTypeNameIfFileProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            container.AddSingleton<ISettings>((null as ISettings).Create());

            String expected = typeof(IFile).Name;
            try
            {
                var target = new Repository(container.BuildServiceProvider());
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
            container.AddSingleton<ISettings>((null as ISettings).Create());
            Assert.Throws<DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfDirectoryProviderIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<ISettings>((null as ISettings).Create());

            String expected = typeof(IDirectory).Name;
            try
            {
                var target = new Repository(container.BuildServiceProvider());
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowDependencyNotFoundExceptionIfSettingsAreNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            Assert.Throws<DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfSettingsAreNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());

            String expected = typeof(ISettings).Name;
            try
            {
                var target = new Repository(container.BuildServiceProvider());
            }
            catch (DependencyNotFoundException ex)
            {
                Assert.Equal(expected, ex.InterfaceTypeName);
            }
        }

        [Fact]
        public void ThrowSettingNotFoundExceptionIfOutputPathIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());
            container.AddSingleton<ISettings>(new Settings());
            Assert.Throws<SettingNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperSettingNameIfTargetConnectionIsNotProvided()
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFile>(Mock.Of<IFile>());
            container.AddSingleton<IDirectory>(Mock.Of<IDirectory>());

            var settings = new Settings();
            container.AddSingleton<ISettings>(settings);

            String expected = nameof(settings.TargetConnection);

            try
            {
                var target = new Repository(container.BuildServiceProvider());
            }
            catch (SettingNotFoundException ex)
            {
                Assert.Equal(expected, ex.SettingName);
            }
        }

    }
}
