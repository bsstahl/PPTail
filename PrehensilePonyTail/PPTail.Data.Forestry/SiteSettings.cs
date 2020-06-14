using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.Forestry
{
    internal class SiteSettings
    {
        const string _defaultDateTimeFormatSpecifier = "yyyy-MM-dd H:mm UTC";
        const string _defaultDateFormatSpecifier = "yyyy-MM-dd";
        const string _defaultOutputFileExtension = "html";

        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 PostsPerPage { get; set; }
        public Int32 PostsPerFeed { get; set; }
        public String Theme { get; set; } = string.Empty;
        public String Copyright { get; set; }
        public String ContactEmail { get; set; }

        public bool UseAdditionalPagesDropdown { get; set; }
        public String AdditionalPagesDropdownLabel { get; set; }
        public bool DisplayTitleInNavbar { get; set; }
        public String DateTimeFormatSpecifier { get; set; }
        public String DateFormatSpecifier { get; set; }
        public String ItemSeparator { get; set; }
        public String OutputFileExtension { get; set; }

        public IEnumerable<String> AdditionalFilePaths { get; set; } = new List<String>();
        public IEnumerable<SiteVariable> SiteVariables { get; set; } = new List<SiteVariable>();

        // public IEnumerable<String> ExtendedSettings { get; set; }

        public Entities.SiteSettings AsEntity()
        {
            return new Entities.SiteSettings()
            {
                Title = this.Title,
                Description = this.Description,
                PostsPerFeed = this.PostsPerFeed,
                PostsPerPage = this.PostsPerPage,
                Theme = this.Theme ?? String.Empty,
                Copyright = this.Copyright,
                ContactEmail = this.ContactEmail,
                UseAdditionalPagesDropdown = this.UseAdditionalPagesDropdown,
                AdditionalPagesDropdownLabel = this.AdditionalPagesDropdownLabel,
                DisplayTitleInNavbar = this.DisplayTitleInNavbar,
                DateTimeFormatSpecifier = this.DateTimeFormatSpecifier ?? _defaultDateTimeFormatSpecifier,
                DateFormatSpecifier = this.DateFormatSpecifier ?? _defaultDateFormatSpecifier,
                ItemSeparator = this.ItemSeparator ?? String.Empty,
                OutputFileExtension = this.OutputFileExtension ?? _defaultOutputFileExtension,
                AdditionalFilePaths = this.AdditionalFilePaths,
                Variables = this.SiteVariables.Select(v => 
                    new Entities.SiteVariable()
                    { 
                        Name = v.VariableName,
                        Value = v.VariableValue
                    })
            };
        }
    }
}
