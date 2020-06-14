using PPTail.Entities;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public new SiteSettingsBuilder Theme(String value)
        {
            base.Theme = value;
            return this;
        }

        public new SiteSettingsBuilder Copyright(String value)
        {
            base.Copyright = value;
            return this;
        }

        public new SiteSettingsBuilder ContactEmail(String value)
        {
            base.Copyright = value;
            return this;
        }

        public new SiteSettingsBuilder UseAdditionalPagesDropdown(bool value)
        {
            base.UseAdditionalPagesDropdown = value;
            return this;
        }

        public new SiteSettingsBuilder DisplayTitleInNavbar(bool value)
        {
            base.DisplayTitleInNavbar = value;
            return this;
        }

        public new SiteSettingsBuilder AdditionalFilePaths(IEnumerable<String> value)
        {
            base.AdditionalFilePaths = value;
            return this;
        }

        public SiteSettingsBuilder AddAdditionalFilePaths(IEnumerable<String> value)
        {
            var additionalPathsResult = new List<String>();

            if (base.AdditionalFilePaths.IsNotNull() && base.AdditionalFilePaths.Any())
                additionalPathsResult.AddRange(base.AdditionalFilePaths);

            if (value.IsNotNull() && value.Any())
                additionalPathsResult.AddRange(value);

            return this.AdditionalFilePaths(additionalPathsResult);
        }

    }
}
