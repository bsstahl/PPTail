using Moq;
using PPTail.Entities;
using PPTail.Exceptions;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_GenerateStylesheet_Should
    {
        [Fact]
        public void ThrowATemplateNotFoundExceptionIfTheStyleTemplateIsNotProvided()
        {
            var target = (null as IPageGenerator).Create(Enumerations.TemplateType.Style);
            Assert.Throws<TemplateNotFoundException>(() => target.GenerateStylesheet(Mock.Of<SiteSettings>()));
        }

        [Fact]
        public void ReturnTheOriginalStyleTemplateIfThereAreNoReplacementFieldsPresent()
        {
            string styleTemplate = string.Empty.GetRandom();
            var target = (null as IPageGenerator).Create(string.Empty, string.Empty, styleTemplate);
            var actual = target.GenerateStylesheet(Mock.Of<SiteSettings>());
            Assert.Equal(styleTemplate, actual);
        }
    }
}
