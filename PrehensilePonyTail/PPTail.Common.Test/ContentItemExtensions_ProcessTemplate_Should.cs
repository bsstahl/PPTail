using PPTail.Entities;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHelperExtensions;
using Xunit;

namespace PPTail.Common.Test
{
    public class ContentItemExtensions_ProcessTemplate_Should
    {
        [Fact]
        public void ProcessEarliestPostLastIfNoPostLimitSpecified()
        {
            const int postsPerPage = 0;
            const char itemSeparator = ';';

            string pageTemplateContent = "{Content}";
            string itemTemplateContent = "{Content}";

            var settings = new Settings() { ItemSeparator = itemSeparator.ToString() };
            var siteSettings = new SiteSettings() { Title = string.Empty.GetRandom(), Description = string.Empty.GetRandom(), PostsPerPage = postsPerPage };
            var categories = (null as IEnumerable<Category>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            int maxPostCount = postsPerPage;

            var pageTemplate = new Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.HomePage };
            var itemTemplate = new Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };

            var posts = new List<ContentItem>();
            var earliestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-35));
            var middlePost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-25));
            var latestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-15));

            posts.Add(middlePost);
            posts.Add(latestPost);
            posts.Add(earliestPost);

            var actual = posts.ProcessTemplate(settings, siteSettings, categories, pageTemplate, itemTemplate, sidebarContent, navContent, pageTitle, maxPostCount, string.Empty, itemSeparator.ToString(), false);
            var actualPosts = actual.Split(itemSeparator);
            Assert.Equal(earliestPost.Content, actualPosts.Last());
        }

        [Fact]
        public void ProcessEarliestPostLastIfAPostLimitIsSpecified()
        {
            const int postsPerPage = 3;
            const char itemSeparator = ';';

            string pageTemplateContent = "{Content}";
            string itemTemplateContent = "{Content}";

            var settings = new Settings() { ItemSeparator = itemSeparator.ToString() };
            var siteSettings = new SiteSettings() { Title = string.Empty.GetRandom(), Description = string.Empty.GetRandom(), PostsPerPage = postsPerPage };
            var categories = (null as IEnumerable<Category>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            int maxPostCount = postsPerPage;

            var pageTemplate = new Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.HomePage };
            var itemTemplate = new Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };

            var posts = new List<ContentItem>();
            var earliestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-35));
            var middlePost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-25));
            var latestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-15));

            posts.Add(middlePost);
            posts.Add(latestPost);
            posts.Add(earliestPost);

            var actual = posts.ProcessTemplate(settings, siteSettings, categories, pageTemplate, itemTemplate, sidebarContent, navContent, pageTitle, maxPostCount, string.Empty, itemSeparator.ToString(), false);
            var actualPosts = actual.Split(itemSeparator);
            Assert.Equal(earliestPost.Content, actualPosts.Last());
        }

        [Fact]
        public void ProcessLatestPostFirstIfNoPostLimitSpecified()
        {
            const int postsPerPage = 0;
            const char itemSeparator = ';';

            string pageTemplateContent = "{Content}";
            string itemTemplateContent = "{Content}";

            var settings = new Settings() { ItemSeparator = itemSeparator.ToString() };
            var siteSettings = new SiteSettings() { Title = string.Empty.GetRandom(), Description = string.Empty.GetRandom(), PostsPerPage = postsPerPage };
            var categories = (null as IEnumerable<Category>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            int maxPostCount = postsPerPage;

            var pageTemplate = new Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.HomePage };
            var itemTemplate = new Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };

            var posts = new List<ContentItem>();
            var earliestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-35));
            var middlePost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-25));
            var latestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-15));

            posts.Add(middlePost);
            posts.Add(earliestPost);
            posts.Add(latestPost);

            var actual = posts.ProcessTemplate(settings, siteSettings, categories, pageTemplate, itemTemplate, sidebarContent, navContent, pageTitle, maxPostCount, string.Empty, itemSeparator.ToString(), false);
            var actualPosts = actual.Split(itemSeparator);
            Assert.Equal(latestPost.Content, actualPosts.First());
        }

        [Fact]
        public void ProcessLatestPostFirstIfAPostLimitIsSpecified()
        {
            const int postsPerPage = 3;
            const char itemSeparator = ';';

            string pageTemplateContent = "{Content}";
            string itemTemplateContent = "{Content}";

            var settings = new Settings() { ItemSeparator = itemSeparator.ToString() };
            var siteSettings = new SiteSettings() { Title = string.Empty.GetRandom(), Description = string.Empty.GetRandom(), PostsPerPage = postsPerPage };
            var categories = (null as IEnumerable<Category>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            int maxPostCount = postsPerPage;

            var pageTemplate = new Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.HomePage };
            var itemTemplate = new Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };

            var posts = new List<ContentItem>();
            var earliestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-35));
            var middlePost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-25));
            var latestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-15));

            posts.Add(earliestPost);
            posts.Add(latestPost);
            posts.Add(middlePost);

            var actual = posts.ProcessTemplate(settings, siteSettings, categories, pageTemplate, itemTemplate, sidebarContent, navContent, pageTitle, maxPostCount, string.Empty, itemSeparator.ToString(), false);
            var actualPosts = actual.Split(itemSeparator);
            Assert.Equal(latestPost.Content, actualPosts.First());
        }

        [Fact]
        public void NotProcessAPostOutsideThePostLimit()
        {
            const int postsPerPage = 2;
            const char itemSeparator = ';';

            string pageTemplateContent = "{Content}";
            string itemTemplateContent = "{Content}";

            var settings = new Settings() { ItemSeparator = itemSeparator.ToString() };
            var siteSettings = new SiteSettings() { Title = string.Empty.GetRandom(), Description = string.Empty.GetRandom(), PostsPerPage = postsPerPage };
            var categories = (null as IEnumerable<Category>).Create();

            string sidebarContent = string.Empty.GetRandom();
            string navContent = string.Empty.GetRandom();
            string pageTitle = string.Empty.GetRandom();
            int maxPostCount = postsPerPage;

            var pageTemplate = new Template() { Content = pageTemplateContent, TemplateType = Enumerations.TemplateType.HomePage };
            var itemTemplate = new Template() { Content = itemTemplateContent, TemplateType = Enumerations.TemplateType.Item };

            var posts = new List<ContentItem>();
            var earliestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-35));
            var middlePost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-25));
            var latestPost = (null as ContentItem).Create(DateTime.UtcNow.AddHours(-15));

            posts.Add(middlePost);
            posts.Add(earliestPost);
            posts.Add(latestPost);

            var actual = posts.ProcessTemplate(settings, siteSettings, categories, pageTemplate, itemTemplate, sidebarContent, navContent, pageTitle, maxPostCount, string.Empty, "<hr/>", false);
            Assert.DoesNotContain(earliestPost.Content, actual);
        }


    }
}
