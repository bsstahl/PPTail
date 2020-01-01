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

namespace PPTail.Generator.Links.Test
{
    public class LinkProvider_Ctor_Should
    {
        [Fact]
        public void NotThrowAnExceptionIfAllDependenciesAreProvided()
        {
            var target = (null as ILinkProvider).Create();
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheContainerIsNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new PPTail.Generator.Links.LinkProvider(null));
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSettingsAreNotProvided()
        {
            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ISettings>();
            Assert.Throws<DependencyNotFoundException>(() => (null as ILinkProvider).Create(container));
        }

        [Fact]
        public void ReturnTheProperDependencyNameIfTheSettingsAreNotProvided()
        {
            String expected = "ISettings";

            var container = (null as IServiceCollection).Create();
            container.RemoveDependency<ISettings>();

            String actual = string.Empty;
            try
            {
                var target = (null as ILinkProvider).Create(container);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }
            Assert.Equal(expected, actual);
        }
    }
}
