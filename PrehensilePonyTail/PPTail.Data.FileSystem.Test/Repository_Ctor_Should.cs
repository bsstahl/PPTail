using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using Xunit;
using TestHelperExtensions;
using Moq;
using PPTail.Entities;

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
        public void ThrowADependencyNotFoundExceptionIfAFileSystemIsNotProvided()
        {
            var settings = new Settings();
            settings.ExtendedSettings.Set(_sourceDataPathSettingName, string.Empty.GetRandom());

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);

            Assert.Throws<Exceptions.DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider()));
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
    }
}
