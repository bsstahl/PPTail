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
    public class HomePageGenerator_Ctor_Should
    {
        const String _defaultDateTimeSpecifier = "MM/dd/yyyy";

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
        public void ThrowADependencyNotFoundExceptionIfTheContentRepositoryIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<IContentRepository>();
            Assert.Throws<DependencyNotFoundException>(() => new PPTail.Generator.HomePage.HomePageGenerator(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheTemplateProcessorIsNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ITemplateProcessor>();
            Assert.Throws<DependencyNotFoundException>(() => new PPTail.Generator.HomePage.HomePageGenerator(container.BuildServiceProvider()));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheHomePageTemplateIsNotProvided()
        {
            Assert.Throws<TemplateNotFoundException>(() => (null as IHomePageGenerator).Create(Enumerations.TemplateType.HomePage));
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheItemPageTemplateIsNotProvided()
        {
            Assert.Throws<TemplateNotFoundException>(() => (null as IHomePageGenerator).Create(Enumerations.TemplateType.Item));
        }

    }
}
