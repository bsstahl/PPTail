using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_Ctor_Should
    {
        [Fact]
        public void NotThrowAnException()
        {
            var target = new PPTail.Generator.T4Html.PageGenerator();
        }
    }
}
