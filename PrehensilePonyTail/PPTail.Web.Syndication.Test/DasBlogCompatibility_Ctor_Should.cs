using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Web.Syndication.Test
{
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
            string actual = string.Empty;
            try
            {
                var serviceProvider = Mock.Of<IServiceProvider>();
                var target = new DasBlogCompatibility(null);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }

            string expected = "next";
            Assert.Equal(expected, actual);
        }


    }
}
