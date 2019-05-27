using Newtonsoft.Json;
using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    public class SiteSettingsFileBuilder : SiteSettings
    {
        public string Build()
        {
            var settings = (this as SiteSettings);
            return JsonConvert.SerializeObject(settings);
        }

        public new SiteSettingsFileBuilder Title(string title)
        {
            base.Title = title;
            return this;
        }

        public new SiteSettingsFileBuilder Description(string description)
        {
            base.Description = description;
            return this;
        }

        public new SiteSettingsFileBuilder PostsPerPage(int postsPerPage)
        {
            base.PostsPerPage = postsPerPage;
            return this;
        }

        public new SiteSettingsFileBuilder PostsPerFeed(int postsPerFeed)
        {
            base.PostsPerFeed = postsPerFeed;
            return this;
        }

        public new SiteSettingsFileBuilder Theme(string theme)
        {
            base.Theme = theme;
            return this;
        }

        public new SiteSettingsFileBuilder Copyright(string copyright)
        {
            base.Copyright = copyright;
            return this;
        }

        public SiteSettingsFileBuilder UseRandomValues()
        {
            return this
                .Copyright(string.Empty.GetRandom())
                .Description(string.Empty.GetRandom())
                .PostsPerFeed(25.GetRandom(5))
                .PostsPerPage(25.GetRandom(5))
                .Theme(string.Empty.GetRandom())
                .Title(string.Empty.GetRandom());
        }
    }
}
