using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Builders
{
    public class SiteSettingsBuilder: SiteSettings
    {
        public SiteSettings Build()
        {
            return this;
        }

        public new SiteSettingsBuilder Title(String title)
        {
            base.Title = title;
            return this;
        }

        public new SiteSettingsBuilder Description(String description)
        {
            base.Description = description;
            return this;
        }

        public new SiteSettingsBuilder PostsPerPage(Int32 postsPerPage)
        {
            base.PostsPerPage = postsPerPage;
            return this;
        }

        public new SiteSettingsBuilder PostsPerFeed(Int32 postsPerFeed)
        {
            base.PostsPerFeed = postsPerFeed;
            return this;
        }

        public new SiteSettingsBuilder Theme(String theme)
        {
            base.Theme = theme;
            return this;
        }

        public new SiteSettingsBuilder Copyright(String copyright)
        {
            base.Copyright = copyright;
            return this;
        }

    }
}
