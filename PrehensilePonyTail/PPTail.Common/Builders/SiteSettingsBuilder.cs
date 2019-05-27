using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Common.Builders
{
    public class SiteSettingsBuilder: SiteSettings
    {
        public SiteSettings Build()
        {
            return this;
        }

        public new SiteSettingsBuilder Title(string title)
        {
            base.Title = title;
            return this;
        }

        public new SiteSettingsBuilder Description(string description)
        {
            base.Description = description;
            return this;
        }

        public new SiteSettingsBuilder PostsPerPage(int postsPerPage)
        {
            base.PostsPerPage = postsPerPage;
            return this;
        }

        public new SiteSettingsBuilder PostsPerFeed(int postsPerFeed)
        {
            base.PostsPerFeed = postsPerFeed;
            return this;
        }

        public new SiteSettingsBuilder Theme(string theme)
        {
            base.Theme = theme;
            return this;
        }

        public new SiteSettingsBuilder Copyright(string copyright)
        {
            base.Copyright = copyright;
            return this;
        }

    }
}
