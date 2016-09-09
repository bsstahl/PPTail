using PPTail.Interfaces;
using System;
using Xunit;

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_Ctor_Should
    {

        [Fact]
        public void NotThrowAnException() 
        {
            var target = (null as IContentRepository).Create();
        }

    }
}
