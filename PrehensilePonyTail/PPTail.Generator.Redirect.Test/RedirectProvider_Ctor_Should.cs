using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.Redirect.Test
{
    public class RedirectProvider_Ctor_Should
    {
        [Fact]
        public void ThrowAnArgumentNullExceptionIfTheServiceProviderIsNotProvided()
        {
            Assert.Throws<ArgumentNullException>(() => new RedirectProvider(null));
        }

        [Fact]
        public void ReturnTheProperParameterNameIfTheServiceProviderIsNotProvided()
        {
            String parameterName = "serviceProvider";
            try
            {
                var target = new RedirectProvider(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal(parameterName, ex.ParamName);
            }
        }

        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheRedirectTemplateIsNotProvided()
        {
            var templates = new List<Template>();
            Assert.Throws<TemplateNotFoundException>(() => (null as IRedirectProvider).Create(templates));
        }

    }
}
