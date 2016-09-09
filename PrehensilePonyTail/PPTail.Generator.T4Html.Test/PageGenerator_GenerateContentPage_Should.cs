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
            var pageData = (null as ContentItem).Create();

            string template = "*******************************{Title}*******************************";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);
            Assert.Contains(pageData.Title, actual);
        }
    }
}
