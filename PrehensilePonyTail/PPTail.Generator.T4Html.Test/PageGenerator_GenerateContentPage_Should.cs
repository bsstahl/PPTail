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

        [Fact]
        public void RemoveThePlaceholderTextIfTheTitleDataValueIsNull()
        {
            const string placeholderText = "{Title}";

            var pageData = (null as ContentItem).Create();
            pageData.Title = null;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(placeholderText));
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void ReplaceAContentPlaceholderWithTheContent()
        {
            const string placeholderText = "{Content}";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.Content;

            string template = $"*******************************{placeholderText}*******************************";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);
            Assert.Contains(expectedData, actual);
        }

        [Fact]
        public void ReplaceAllContentPlaceholdersWithTheContent()
        {
            const string placeholderText = "{Content}";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.Content;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(expectedData));
            Assert.Equal(6, actualCount);
        }

        [Fact]
        public void RemoveThePlaceholderTextIfTheContentDataValueIsNull()
        {
            const string placeholderText = "{Content}";

            var pageData = (null as ContentItem).Create();
            pageData.Content = null;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(placeholderText));
            Assert.Equal(0, actualCount);
        }

        [Fact]
        public void ReplaceAllAuthorPlaceholdersWithTheAuthor()
        {
            const string placeholderText = "{Author}";

            var pageData = (null as ContentItem).Create();
            var expectedData = pageData.Author;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(expectedData));
            Assert.Equal(6, actualCount);
        }

        [Fact]
        public void RemoveThePlaceholderTextIfTheAuthorDataValueIsNull()
        {
            const string placeholderText = "{Author}";

            var pageData = (null as ContentItem).Create();
            pageData.Author = null;

            string template = $"{placeholderText}*******{placeholderText}******\r\n****{placeholderText}*********\t\t****{placeholderText}*****{placeholderText}************{placeholderText}";
            var target = (null as IPageGenerator).Create(template, string.Empty);

            var actual = target.GenerateContentPage(pageData);
            Console.WriteLine(actual);

            int actualCount = actual.Select((c, i) => actual.Substring(i)).Count(sub => sub.StartsWith(placeholderText));
            Assert.Equal(0, actualCount);
        }
    }
}
