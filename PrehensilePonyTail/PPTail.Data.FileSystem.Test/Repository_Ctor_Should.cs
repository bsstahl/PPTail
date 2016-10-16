using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using Xunit;
using TestHelperExtensions;
using Moq;
using PPTail.Entities;
using PPTail.Exceptions;

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_Ctor_Should
    {
        const string _sourceDataPathSettingName = "sourceDataPath";

        [Fact]
        public void NotThrowAnExceptionIfAllDependenciesAreSupplied() 
        {
            var settings = new Settings();
            settings.ExtendedSettings.Set(_sourceDataPathSettingName, string.Empty.GetRandom());

            var fileSystem = Mock.Of<IFile>();

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IFile>(fileSystem);

            var target = new Repository(container.BuildServiceProvider());
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfSettingsAreNotProvided()
        {
            var fileSystem = Mock.Of<IFile>();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);

            Assert.Throws<Exceptions.DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfSettingsAreNotProvided()
        {
            var fileSystem = Mock.Of<IFile>();

            var container = new ServiceCollection();
            container.AddSingleton<IFile>(fileSystem);

            string expected = typeof(ISettings).Name;
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
        public void ThrowADependencyNotFoundExceptionIfAFileSystemIsNotProvided()
        {
            var settings = new Settings();
            settings.ExtendedSettings.Set(_sourceDataPathSettingName, string.Empty.GetRandom());

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);

            Assert.Throws<Exceptions.DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfFileSystemIsNotProvided()
        {
            var settings = new Settings();
            settings.ExtendedSettings.Set(_sourceDataPathSettingName, string.Empty.GetRandom());

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);

            string expected = typeof(IFile).Name;
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
        public void ThrowASettingNotFoundExceptionIfTheSourceDataPathIsNotSpecified()
        {
            var settings = new Settings();
            var fileSystem = Mock.Of<IFile>();

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IFile>(fileSystem);

            Assert.Throws<Exceptions.SettingNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperSettingNameIfTheSourceDataPathIsNotSpecified()
        {
            var settings = new Settings();
            var fileSystem = Mock.Of<IFile>();

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IFile>(fileSystem);

            string expected = _sourceDataPathSettingName;
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
