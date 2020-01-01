using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TestHelperExtensions;

namespace PPTail.Data.FileSystem.Test
{
    public class SettingsFileBuilder: SiteSettings
    {
        const String _siteSettingsXmlTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?><settings><name>{0}</name><description>{1}</description><postsperpage>{2}</postsperpage><postsperfeed>{3}</postsperfeed><theme>{4}</theme></settings>";

        const String _defaultTitle = "My Blog";
        const String _defaultDescription = "This is the description of my blog";
        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 10;
        const String _defaultTheme = "MyBlogTheme";

        private bool _removeTitle = false;
        private bool _removeDescription = false;
        private bool _removePostsPerPage = false;
        private bool _removePostsPerFeed = false;
        private bool _removeTheme = false;

        public String Build()
        {
            var node = new XElement(XName.Get("settings"));
            return node.ConditionalAddNode(!_removeTitle, "name", base.Title)
                .ConditionalAddNode(!_removeDescription, "description", base.Description)
                .ConditionalAddNode(!_removePostsPerPage, "postsperpage", base.PostsPerPage.ToString())
                .ConditionalAddNode(!_removePostsPerFeed, "postsperfeed", base.PostsPerFeed.ToString())
                .ConditionalAddNode(!_removeTheme, "theme", base.Theme)
                .ToString();
        }

        public SettingsFileBuilder UseDefaultValues()
        {
            return this.Title(_defaultTitle)
                .Description(_defaultDescription)
                .PostsPerPage(_defaultPostsPerPage)
                .PostsPerFeed(_defaultPostsPerFeed)
                .Theme(_defaultTheme);
        }

        public SettingsFileBuilder UseRandomValues()
        {
            return this.Title(string.Empty.GetRandom(8))
                .Description(string.Empty.GetRandom(25))
                .PostsPerPage(10.GetRandom())
                .PostsPerFeed(25.GetRandom())
                .Theme(string.Empty.GetRandom(15));
        }

        public new SettingsFileBuilder Title(String title)
        {
            base.Title = title;
            return this;
        }

        public SettingsFileBuilder RemoveTitle()
        {
            _removeTitle = true;
            return this;
        }

        public new SettingsFileBuilder Description(String description)
        {
            base.Description = description;
            return this;
        }

        public SettingsFileBuilder RemoveDescription()
        {
            _removeDescription = true;
            return this;
        }

        public new SettingsFileBuilder PostsPerPage(Int32 postsPerPage)
        {
            base.PostsPerPage = postsPerPage;
            return this;
        }

        public SettingsFileBuilder RemovePostsPerPage()
        {
            _removePostsPerPage = true;
            return this;
        }

        public new SettingsFileBuilder PostsPerFeed(Int32 postsPerFeed)
        {
            base.PostsPerFeed = postsPerFeed;
            return this;
        }

        public SettingsFileBuilder RemovePostsPerFeed()
        {
            _removePostsPerFeed = true;
            return this;
        }

        public new SettingsFileBuilder Theme(String theme)
        {
            base.Theme = theme;
            return this;
        }

        public SettingsFileBuilder RemoveTheme()
        {
            _removeTheme = true;
            return this;
        }

    }
}
