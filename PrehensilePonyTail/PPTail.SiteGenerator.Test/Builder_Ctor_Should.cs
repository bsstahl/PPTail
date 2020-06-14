using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PPTail.Interfaces;
using PPTail.Entities;
using PPTail.Extensions;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using PPTail.Exceptions;

namespace PPTail.SiteGenerator.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Builder_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheServiceProviderIsNotSupplied()
        {
            Assert.Throws<ArgumentNullException>(() => new Builder(null));
        }

        [Fact]
        public void ReturnTheCorrectParameterNameIfTheServiceProviderIsNotSupplied()
        {
            String expected = "serviceProvider";
            try
            {
                var target = new Builder(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal(expected, ex.ParamName);
            }
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheContentRepoIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<IContentRepository>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfTheContentRepoIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<IContentRepository>();
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfThePageGenIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<IPageGenerator>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfThePageGenIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<IPageGenerator>();
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheNavProviderIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<INavigationProvider>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfTheNavProviderIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<INavigationProvider>();
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheArchiveProviderIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<IArchiveProvider>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfTheArchiveProviderIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<IArchiveProvider>();
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheContactProviderIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<IContactProvider>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfTheContactProviderIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<IContactProvider>();
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSearchProviderIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<ISearchProvider>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfTheSearchProviderIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<ISearchProvider>();
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheRedirectProviderIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<IRedirectProvider>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfTheRedirectProviderIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<IRedirectProvider>();
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheSyndicationProviderIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<ISyndicationProvider>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfTheSyndicationProviderIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<ISyndicationProvider>();
        }

        [Fact]
        public void ThrowADependencyNotFoundExceptionIfTheContentItemPageGenIsNotSupplied()
        {
            ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<IContentItemPageGenerator>();
        }

        [Fact]
        public void ShouldReturnTheCorrectDependencyNameIfTheContentItemPageGenIsNotSupplied()
        {
            ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<IContentItemPageGenerator>();
        }


        private void ShouldThrowDependencyNotFoundExceptionIfDependencyIsRemoved<T>() where T: class
        {
            IServiceCollection container = (null as IServiceCollection).Create();
            container.RemoveDependency<T>();
            Assert.Throws<DependencyNotFoundException>(() => (null as Builder).Create(container));
        }

        private void ShouldReturnTheCorrectDependencyNameIfDependencyIsRemoved<T>() where T: class
        {
            IServiceCollection container = (null as IServiceCollection).Create();
            container.RemoveDependency<T>();
            String actual = string.Empty;
            try
            {
                var target = (null as Builder).Create(container);
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }
            Assert.Equal(typeof(T).Name, actual);
        }
    }
}
