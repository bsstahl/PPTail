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
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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

    }
}
