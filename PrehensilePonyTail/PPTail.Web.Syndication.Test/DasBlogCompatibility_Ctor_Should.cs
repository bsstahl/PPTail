using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Web.Syndication.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DasBlogCompatibility_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheNextDelegateIsNotSupplied()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            Assert.Throws<ArgumentNullException>(() => new DasBlogCompatibility(null));
        }

        [Fact]
        public void ReturnTheProperArgumentNameIfTheNextDelegateIsNotSupplied()
        {
            String actual = string.Empty;
            try
            {
                var serviceProvider = Mock.Of<IServiceProvider>();
                var target = new DasBlogCompatibility(null);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }

            String expected = "next";
            Assert.Equal(expected, actual);
        }


    }
}
