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
        [Fact]
        public void NotThrowAnExceptionIfAllDependenciesAreSupplied() 
        {
            var settings = new SettingsBuilder()
                .SourceConnection(string.Empty.GetRandom())
                .Build();

            var fileSystem = Mock.Of<IFile>();
            var directory = Mock.Of<IDirectory>();

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IFile>(fileSystem);
            container.AddSingleton<IDirectory>(directory);

            var serviceProvider = container.BuildServiceProvider();
            var target = new Repository(serviceProvider);
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
        public void ThrowADependencyNotFoundExceptionIfAFileSystemIsNotProvided()
        {
            var settings = new Settings() { SourceConnection = string.Empty.GetRandom() };

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);

            Assert.Throws<Exceptions.DependencyNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperInterfaceTypeNameIfFileSystemIsNotProvided()
        {
            var settings = new Settings() { SourceConnection = string.Empty.GetRandom() };

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);

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
        public void ThrowASettingNotFoundExceptionIfTheSourceConnectionIsNotSpecified()
        {
            var settings = new Settings();
            var fileSystem = Mock.Of<IFile>();
            var directory = Mock.Of<IDirectory>();

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IFile>(fileSystem);
            container.AddSingleton<IDirectory>(directory);

            Assert.Throws<Exceptions.SettingNotFoundException>(() => new Repository(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowWithProperSettingNameIfTheSourceConnectionIsNotSpecified()
        {
            var settings = new Settings();
            var fileSystem = Mock.Of<IFile>();
            var directory = Mock.Of<IDirectory>();

            var container = new ServiceCollection();
            container.AddSingleton<ISettings>(settings);
            container.AddSingleton<IFile>(fileSystem);
            container.AddSingleton<IDirectory>(directory);

            String expected = nameof(settings.SourceConnection);
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
