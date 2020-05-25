using PPTail.Entities;
using System;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.Forestry.Test
{
    public class SettingsFileBuilder: SiteSettings
    {
        const String _defaultTitle = "My Blog";
        const String _defaultDescription = "This is the description of my blog";
        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 10;
        const String _defaultTheme = "MyBlogTheme";
        const String _defaultCopyright = "Copyright &copy; Now by Me. All Rights Reserved.";

        private bool _removeTitle = false;
        private bool _removeDescription = false;
        private bool _removePostsPerPage = false;
        private bool _removePostsPerFeed = false;
        private bool _removeTheme = false;
        private bool _removeCopyright = false;

        public String Build()
        {
            var node = new StringBuilder();
            return node.AppendLine("---")
                .ConditionalAppendLine(!_removeTitle, "title", base.Title)
                .ConditionalAppendLine(!_removeDescription, "description", base.Description)
                .ConditionalAppendLine(!_removePostsPerPage, "postsperpage", base.PostsPerPage.ToString())
                .ConditionalAppendLine(!_removePostsPerFeed, "postsperfeed", base.PostsPerFeed.ToString())
                .ConditionalAppendLine(!_removeTheme, "theme", base.Theme)
                .ConditionalAppendLine(!_removeCopyright, "copyright", base.Copyright)
                .AppendLine("")
                .AppendLine("---")
                .AppendLine("")
                .ToString();
        }

        public SettingsFileBuilder UseDefaultValues()
        {
            return this.Title(_defaultTitle)
                .Description(_defaultDescription)
                .PostsPerPage(_defaultPostsPerPage)
                .PostsPerFeed(_defaultPostsPerFeed)
                .Copyright(_defaultCopyright)
                .Theme(_defaultTheme);
        }

        public SettingsFileBuilder UseRandomValues()
        {
            return this.Title(string.Empty.GetRandom(8))
                .Description(string.Empty.GetRandom(25))
                .PostsPerPage(10.GetRandom())
                .PostsPerFeed(25.GetRandom())
                .Copyright(string.Empty.GetRandom(30))
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

        public new SettingsFileBuilder Copyright(String copyright)
        {
            base.Copyright = copyright;
            return this;
        }

        public SettingsFileBuilder RemoveCopyright()
        {
            _removeCopyright = true;
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
