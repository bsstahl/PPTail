using PPTail.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.Forestry.Test
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SettingsFileBuilder: Entities.SiteSettings
    {
        const String _defaultTitle = "My Blog";
        const String _defaultDescription = "This is the description of my blog";
        const Int32 _defaultPostsPerPage = 3;
        const Int32 _defaultPostsPerFeed = 10;
        const String _defaultTheme = "MyBlogTheme";
        const String _defaultCopyright = "Copyright &copy; Now by Me. All Rights Reserved.";

        const String _defaultVariablesJoined = "TwitterLink:[@mytwittername](https://twitter.com/mytwittername);FacebookLink:[My Facebook](https://facebook.com/myfacebook)";

        const String _defaultContactEmail = "notarealaddress@prehensileponytail.com";
        const String _defaultAdditionalPagesDropdownLabel = "DropdownMenu";
        const bool _defaultUseAdditionalPagesDropdown = true;
        const bool _defaultDisplayTitleInNavbar = true;

        const String _defaultDateTimeFormatSpecifier = "yyyyMMddTHmm";
        const String _defaultDateFormatSpecifier = "yyyyMMdd";
        const String _defaultItemSeparator = "**--**--";
        const String _defaultOutputFileExtension = "txt";

        const String _defaultAdditionalFilePathsJoined = "Photos;GoodStuff";

        private bool _removeTitle = false;
        private bool _removeDescription = false;
        private bool _removePostsPerPage = false;
        private bool _removePostsPerFeed = false;
        private bool _removeTheme = false;
        private bool _removeCopyright = false;
        private bool _removeVariables = false;

        private bool _removeContactEmail = false;
        private bool _removeAdditionalPagesDropdownLabel = false;
        private bool _removeUseAdditionalPagesDropdown = false;
        private bool _removeDisplayTitleInNavbar = false;

        private bool _removeDateTimeFormatSpecifier = false;
        private bool _removeDateFormatSpecifier = false;
        private bool _removeItemSeparator = false;
        private bool _removeOutputFileExtension = false;

        private bool _removeAdditionalFilePaths = false;

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
                .ConditionalAppendLine(!_removeContactEmail, "contactemail", base.ContactEmail)
                .ConditionalAppendLine(!_removeAdditionalPagesDropdownLabel, "additionalpagesdropdownlabel", base.AdditionalPagesDropdownLabel)
                .ConditionalAppendLine(!_removeUseAdditionalPagesDropdown, "useadditionalpagesdropdown", base.UseAdditionalPagesDropdown.ToString().ToLower())
                .ConditionalAppendLine(!_removeDisplayTitleInNavbar, "displaytitleinnavbar", base.DisplayTitleInNavbar.ToString().ToLower())
                .ConditionalAppendLine(!_removeDateTimeFormatSpecifier, "datetimeformatspecifier", base.DateTimeFormatSpecifier)
                .ConditionalAppendLine(!_removeDateFormatSpecifier, "dateformatspecifier", base.DateFormatSpecifier)
                .ConditionalAppendLine(!_removeItemSeparator, "itemseparator", base.ItemSeparator)
                .ConditionalAppendLine(!_removeOutputFileExtension, "outputfileextension", base.OutputFileExtension)
                .ConditionalAppendSiteVariables(!_removeVariables, "sitevariables", "- variablename", "  variablevalue", base.Variables)
                .ConditionalAppendList(!_removeAdditionalFilePaths, "additionalfilepaths", base.AdditionalFilePaths)
                .AppendLine("")
                .AppendLine("---")
                .AppendLine("")
                .ToString();
        }

        public SettingsFileBuilder UseDefaultValues()
        {
            var defaultVariables = _defaultVariablesJoined.Split(';');
            var defaultSiteVariables = defaultVariables
                .Select(v => new SiteVariable()
                { 
                    Name = v.Split(":")[0],
                    Value = v.Split(":")[1]
                });

            return this
                .Title(_defaultTitle)
                .Description(_defaultDescription)
                .PostsPerPage(_defaultPostsPerPage)
                .PostsPerFeed(_defaultPostsPerFeed)
                .Copyright(_defaultCopyright)
                .Theme(_defaultTheme)
                .Variables(defaultSiteVariables)
                .ContactEmail(_defaultContactEmail)
                .AdditionalPagesDropdownLabel(_defaultAdditionalPagesDropdownLabel)
                .DateTimeFormatSpecifier(_defaultDateTimeFormatSpecifier)
                .DateFormatSpecifier(_defaultDateFormatSpecifier)
                .ItemSeparator(_defaultItemSeparator)
                .OutputFileExtension(_defaultOutputFileExtension)
                .UseAdditionalPagesDropdown(_defaultUseAdditionalPagesDropdown)
                .DisplayTitleInNavbar(_defaultDisplayTitleInNavbar)
                .AdditionalFilePaths(_defaultAdditionalFilePathsJoined.Split(";"));
        }

        public SettingsFileBuilder UseRandomValues()
        {
            return this
                .Title(string.Empty.GetRandom(8))
                .Description(string.Empty.GetRandom(25))
                .PostsPerPage(10.GetRandom())
                .PostsPerFeed(25.GetRandom())
                .Copyright(string.Empty.GetRandom(30))
                .Theme(string.Empty.GetRandom(15))
                .Variables((null as IEnumerable<SiteVariable>).CreateRandom())
                .ContactEmail(string.Empty.GetRandomEmailAddress())
                .AdditionalPagesDropdownLabel(string.Empty.GetRandom())
                .DateTimeFormatSpecifier($"yyMMddHmm {string.Empty.GetRandom(3)}")
                .DateFormatSpecifier($"yyMMdd {string.Empty.GetRandom(3)}")
                .ItemSeparator(string.Empty.GetRandom())
                .OutputFileExtension(string.Empty.GetRandom(3))
                .UseAdditionalPagesDropdown(true.GetRandom())
                .DisplayTitleInNavbar(true.GetRandom())
                .AdditionalFilePaths(new[] { string.Empty.GetRandom(), string.Empty.GetRandom() });
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

        public new SettingsFileBuilder ContactEmail(String value)
        {
            base.ContactEmail = value;
            return this;
        }

        public SettingsFileBuilder RemoveContactEmail()
        {
            _removeContactEmail = true;
            return this;
        }

        public new SettingsFileBuilder AdditionalPagesDropdownLabel(String value)
        {
            base.AdditionalPagesDropdownLabel = value;
            return this;
        }

        public SettingsFileBuilder RemoveAdditionalPagesDropdownLabel()
        {
            _removeAdditionalPagesDropdownLabel = true;
            return this;
        }

        public new SettingsFileBuilder DateTimeFormatSpecifier(String value)
        {
            base.DateTimeFormatSpecifier = value;
            return this;
        }

        public SettingsFileBuilder RemoveDateTimeFormatSpecifier()
        {
            _removeDateTimeFormatSpecifier = true;
            return this;
        }

        public new SettingsFileBuilder DateFormatSpecifier(String value)
        {
            base.DateFormatSpecifier = value;
            return this;
        }

        public SettingsFileBuilder RemoveDateFormatSpecifier()
        {
            _removeDateFormatSpecifier = true;
            return this;
        }

        public new SettingsFileBuilder ItemSeparator(String value)
        {
            base.ItemSeparator = value;
            return this;
        }

        public SettingsFileBuilder RemoveItemSeparator()
        {
            _removeItemSeparator = true;
            return this;
        }

        public new SettingsFileBuilder OutputFileExtension(String value)
        {
            base.OutputFileExtension = value;
            return this;
        }

        public SettingsFileBuilder RemoveOutputFileExtension()
        {
            _removeOutputFileExtension = true;
            return this;
        }

        public new SettingsFileBuilder UseAdditionalPagesDropdown(bool value)
        {
            base.UseAdditionalPagesDropdown = value;
            return this;
        }

        public SettingsFileBuilder RemoveUseAdditionalPagesDropdown()
        {
            _removeUseAdditionalPagesDropdown = true;
            return this;
        }

        public new SettingsFileBuilder DisplayTitleInNavbar(bool value)
        {
            base.DisplayTitleInNavbar = value;
            return this;
        }

        public SettingsFileBuilder RemoveDisplayTitleInNavbar()
        {
            _removeDisplayTitleInNavbar = true;
            return this;
        }

        public new SettingsFileBuilder Variables(IEnumerable<Entities.SiteVariable> value)
        {
            base.Variables = value;
            return this;
        }

        public SettingsFileBuilder RemoveVariables()
        {
            _removeVariables = true;
            return this;
        }

        public new SettingsFileBuilder AdditionalFilePaths(IEnumerable<String> value)
        {
            base.AdditionalFilePaths = value;
            return this;
        }

        public SettingsFileBuilder RemoveAdditionalFilePaths()
        {
            _removeAdditionalFilePaths = true;
            return this;
        }
    }
}
