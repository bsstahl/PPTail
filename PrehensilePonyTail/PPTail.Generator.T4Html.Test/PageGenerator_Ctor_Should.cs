using Microsoft.Extensions.DependencyInjection;
using PPTail.Builders;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PageGenerator_Ctor_Should
    {
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
        public void ThrowADependencyNotFoundExceptionIfTheTemplatesAreNotProvided()
        {
            var container = new ServiceCollection();
            Assert.Throws<DependencyNotFoundException>(() => new PPTail.Generator.T4Html.PageGenerator(container.BuildServiceProvider()));
        }

    }
}
