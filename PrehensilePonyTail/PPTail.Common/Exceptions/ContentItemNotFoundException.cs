using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Exceptions
{
    public class ContentItemNotFoundException: System.Exception
    {
        public Guid Id { get; set; } = Guid.Empty;
        public String Slug { get; set; } = string.Empty;

        public ContentItemNotFoundException(Guid Id)
            : base($"Unable to locate item with Id {Id.ToString()}") => this.Id = Id;

        public ContentItemNotFoundException(string slug)
            : base($"Unable to locate item with Slug '{slug}'") => this.Slug = slug;
    }
}
