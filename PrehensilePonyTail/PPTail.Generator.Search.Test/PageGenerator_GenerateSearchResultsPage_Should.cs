using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestHelperExtensions;
using PPTail.Entities;
using PPTail.Interfaces;
using System.Text.RegularExpressions;

namespace PPTail.Generator.Search.Test
{
    public class PageGenerator_GenerateSearchResultsPage_Should
    {
        [Fact]
        public void ReturnThePostTitleIfThePostHasTheTag()
        {
            var tag = string.Empty.GetRandom();
            var post = (null as ContentItem).Create(tag);
            var posts = (null as IEnumerable<ContentItem>).Create(post);

            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create();
            var target = (null as ISearchProvider).Create(templates);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            Assert.Contains(post.Title, actual);
        }

        [Fact]
        public void ReturnThePostContentIfThePostHasTheTag()
        {
            var tag = string.Empty.GetRandom();
            var post = (null as ContentItem).Create(tag);
            var posts = (null as IEnumerable<ContentItem>).Create(post);

            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create();
            var settings = (null as Settings).Create();
            var siteSettings = Mock.Of<SiteSettings>();

            var target = (null as ISearchProvider).Create(templates, settings, siteSettings);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            Assert.Contains(post.Content, actual);
        }

        [Fact]
        public void ReturnThePostTitleIfThePostHasTheCategory()
        {
            var allCategories = (null as IEnumerable<Category>).Create();
            var postCategory = allCategories.GetRandom();
            var post = (null as ContentItem).Create(postCategory.Id);
            var posts = (null as IEnumerable<ContentItem>).Create(post);

            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create();
            var target = (null as ISearchProvider).Create(templates, allCategories);
            var actual = target.GenerateSearchResultsPage(postCategory.Name, posts, navigationContent, sidebarContent, pathToRoot);

            Assert.Contains(post.Title, actual);
        }

        [Fact]
        public void ReturnThePostOnceEvenIfItHasBothTheTagAndCategory()
        {
            var allCategories = (null as IEnumerable<Category>).Create();
            var postCategory = allCategories.GetRandom();
            var post = (null as ContentItem).Create(postCategory.Id, new List<string>() { postCategory.Name });
            var posts = (null as IEnumerable<ContentItem>).Create(post);

            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create();
            var target = (null as ISearchProvider).Create(templates, allCategories);
            var actual = target.GenerateSearchResultsPage(postCategory.Name, posts, navigationContent, sidebarContent, pathToRoot);

            var pos = actual.IndexOf(post.Title);
            Assert.DoesNotContain(post.Title, actual.Substring(pos + 1));
        }

        [Fact]
        public void ReturnTheSidebarContentOnce()
        {
            var tag = string.Empty.GetRandom();
            var post = (null as ContentItem).Create(tag);
            var posts = (null as IEnumerable<ContentItem>).Create(post);

            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create();
            var settings = (null as Settings).Create();
            var siteSettings = Mock.Of<SiteSettings>();

            var target = (null as ISearchProvider).Create(templates, settings, siteSettings);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            Assert.Equal(1, Regex.Matches(actual, sidebarContent).Count);
        }

        [Fact]
        public void ReturnTheNavigationContentOnce()
        {
            var tag = string.Empty.GetRandom();
            var post = (null as ContentItem).Create(tag);
            var posts = (null as IEnumerable<ContentItem>).Create(post);

            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create();
            var settings = (null as Settings).Create();
            var siteSettings = Mock.Of<SiteSettings>();

            var target = (null as ISearchProvider).Create(templates, settings, siteSettings);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            Assert.Equal(1, Regex.Matches(actual, navigationContent).Count);
        }

        [Fact]
        public void ReturnTheTagPageTitleOnce()
        {
            var tag = string.Empty.GetRandom();
            var post = (null as ContentItem).Create(tag);
            var posts = (null as IEnumerable<ContentItem>).Create(post);

            string navigationContent = string.Empty.GetRandom();
            string sidebarContent = string.Empty.GetRandom();
            string pathToRoot = string.Empty.GetRandom();

            IEnumerable<Template> templates = (null as IEnumerable<Template>).Create();
            var settings = (null as Settings).Create();
            var siteSettings = Mock.Of<SiteSettings>();

            var target = (null as ISearchProvider).Create(templates, settings, siteSettings);
            var actual = target.GenerateSearchResultsPage(tag, posts, navigationContent, sidebarContent, pathToRoot);

            Assert.Equal(1, Regex.Matches(actual, $"Tag: {tag}").Count);
        }
    }
}
