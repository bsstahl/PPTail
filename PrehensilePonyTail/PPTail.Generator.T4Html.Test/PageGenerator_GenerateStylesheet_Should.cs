using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
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
            Assert.Throws<TemplateNotFoundException>(() => target.GenerateStylesheet());
        }

        [Fact]
        public void ThrowWithTheProperTemplateTypeIfTheStyleTemplateIsNotProvided()
        {
            var target = (null as IPageGenerator).Create(Enumerations.TemplateType.Style);

            TemplateType expected = TemplateType.Style;
            try
            {
                var actual = target.GenerateStylesheet();
            }
            catch (TemplateNotFoundException ex)
            {
                Assert.Equal(expected, ex.TemplateType);
            }
        }

        [Fact]
        public void ReturnTheOriginalStyleTemplateIfThereAreNoReplacementFieldsPresent()
        {
            String styleTemplate = string.Empty.GetRandom();
            var target = (null as IPageGenerator).Create(string.Empty, string.Empty, styleTemplate);
            var actual = target.GenerateStylesheet();
            Assert.Equal(styleTemplate, actual);
        }
    }
}
