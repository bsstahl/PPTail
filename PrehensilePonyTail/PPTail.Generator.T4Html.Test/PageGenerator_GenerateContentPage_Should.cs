using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PPTail.Generator.T4Html.Test
{
    public class PageGenerator_GenerateContentPage_Should
    {
        [Fact]
        public void SetTheContentTitleAsTitleOfThePage()
        {
            var target = (null as IPageGenerator).Create();
            var pageData = (null as ContentItem).Create();

            string expected = $"<title>{pageData.Title}</title>";
            var actual = target.GenerateContentPage(pageData);
            Assert.Contains(expected, actual);
        }
    }
}
