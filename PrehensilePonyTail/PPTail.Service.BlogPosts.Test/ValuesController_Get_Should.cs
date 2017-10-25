using System;
using Xunit;

namespace PPTail.Service.BlogPosts.Test
{
    public class ValuesController_Get_Should
    {
        [Fact]
        public void ReturnTrue()
        {
            var controller = new PPTail.Service.BlogPosts.ValuesController();
            var actual = controller.Get();
            Assert.Equal(true, actual);
        }
    }
}
