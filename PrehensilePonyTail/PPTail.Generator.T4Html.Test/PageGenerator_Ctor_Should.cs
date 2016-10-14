using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_Ctor_Should
    {
        const string _defaultDateTimeSpecifier = "MM/dd/yyyy";

        [Fact]
        public void NotThrowAnExceptionIfAllDependenciesAreProvided()
        {
            var target = (null as IPageGenerator).Create();
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheContainerIsNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new PPTail.Generator.T4Html.PageGenerator(null));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSettingsAreNotProvided()
        {
            var container = new ServiceCollection();

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            container.AddSingleton<IEnumerable<Template>>(templates);

            Assert.Throws<DependencyNotFoundException>(() => new PPTail.Generator.T4Html.PageGenerator(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheTemplatesAreNotProvided()
        {
            var container = new ServiceCollection();

            var settings = new Settings();
            settings.DateTimeFormatSpecifier = _defaultDateTimeSpecifier;
            container.AddSingleton<ISettings>(settings);

            Assert.Throws<DependencyNotFoundException>(() => new PPTail.Generator.T4Html.PageGenerator(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheNavigationProviderIsNotProvided()
        {
            var container = new ServiceCollection();

            var settings = new Settings();
            settings.DateTimeFormatSpecifier = _defaultDateTimeSpecifier;
            container.AddSingleton<ISettings>(settings);

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            container.AddSingleton<IEnumerable<Template>>(templates);

            Assert.Throws<DependencyNotFoundException>(() => new PPTail.Generator.T4Html.PageGenerator(container.BuildServiceProvider()));
        }
    }
}
