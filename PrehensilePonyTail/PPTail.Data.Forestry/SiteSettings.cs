using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.Forestry
{
    internal class SiteSettings
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 PostsPerPage { get; set; }
        public Int32 PostsPerFeed { get; set; }
        public String Theme { get; set; } = string.Empty;
        public String Copyright { get; set; }
        public String ContactEmail { get; set; }
        public IEnumerable<String> ExtendedSettings { get; set; }
        public IEnumerable<SiteVariable> SiteVariables { get; set; } = new List<SiteVariable>();

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
