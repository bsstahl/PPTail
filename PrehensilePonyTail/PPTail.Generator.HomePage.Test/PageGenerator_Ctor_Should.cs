using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Generator.HomePage.Test
{
    public class PageGenerator_Ctor_Should
    {
        const string _defaultDateTimeSpecifier = "MM/dd/yyyy";

        [Fact]
        public void NotThrowAnExceptionIfAllDependenciesAreProvided()
        {
            var target = (null as IHomePageGenerator).Create();
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheContainerIsNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new PPTail.Generator.HomePage.HomePageGenerator(null));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSettingsAreNotProvided()
        {
            var container = new ServiceCollection();

            var templates = (null as IEnumerable<Template>).CreateBlankTemplates();
            container.AddSingleton<IEnumerable<Template>>(templates);

            Assert.Throws<DependencyNotFoundException>(() => new PPTail.Generator.HomePage.HomePageGenerator(container.BuildServiceProvider()));
        }

    }
}
