using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PPTail.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Web.Permalinks.Test
{
    public class DasBlogCompatibility_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheNextDelegateIsNotSupplied()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            Assert.Throws<ArgumentNullException>(() => new DasBlogCompatibility(null, serviceProvider));
        }

        [Fact]
        public void ReturnTheProperArgumentNameIfTheNextDelegateIsNotSupplied()
        {
            string actual = string.Empty;
            try
            {
                var serviceProvider = Mock.Of<IServiceProvider>();
                var target = new DasBlogCompatibility(null, serviceProvider);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }

            string expected = "next";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheServiceProviderIsNotSupplied()
        {
            var nextDelegate = Mock.Of<RequestDelegate>();
            Assert.Throws<ArgumentNullException>(() => new DasBlogCompatibility(nextDelegate, null));
        }

        [Fact]
        public void ReturnTheProperArgumentNameIfTheServiceProviderIsNotSupplied()
        {
            string actual = string.Empty;
            try
            {
                var nextDelegate = Mock.Of<RequestDelegate>();
                var target = new DasBlogCompatibility(nextDelegate, null);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }

            string expected = "serviceProvider";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThrowAnDependencyNotFoundExceptionIfThePostLocatorIsNotSupplied()
        {
            var nextDelegate = Mock.Of<RequestDelegate>();
            var serviceCollection = new ServiceCollection();
            Assert.Throws<DependencyNotFoundException>(() => new DasBlogCompatibility(nextDelegate, serviceCollection.BuildServiceProvider()));
        }

        [Fact]
        public void ReturnTheProperArgumentNameIfThePostLocatorIsNotSupplied()
        {
            string actual = string.Empty;
            try
            {
                var nextDelegate = Mock.Of<RequestDelegate>();
                var serviceCollection = new ServiceCollection();
                var target = new DasBlogCompatibility(nextDelegate, serviceCollection.BuildServiceProvider());
            }
            catch (DependencyNotFoundException ex)
            {
                actual = ex.InterfaceTypeName;
            }

            string expected = "IPostLocator";
            Assert.Equal(expected, actual);
        }
    }
}
