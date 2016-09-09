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
        public void ReplaceATitlePlaceholderWithTheTitle()
        {
            var pageData = (null as ContentItem).Create();

            string template = "*******************************{Title}*******************************";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);
            Assert.Contains(pageData.Title, actual);
        }

        [Fact]
        public void ReplaceAllTitlePlaceholdersWithTheTitle()
        {
            var pageData = (null as ContentItem).Create();

            string template = "{Title}***\r\n************{Title}*********************\t\t****{Title}*****{Title}************{Title}";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(pageData.Title));
            Assert.Equal(5, actualCount);
        }
    }
}
