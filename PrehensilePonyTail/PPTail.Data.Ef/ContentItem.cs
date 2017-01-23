using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Data.Ef
{
    public class ContentItem
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }


        public PPTail.Entities.ContentItem AsEntity()
        {
            return new PPTail.Entities.ContentItem()
            {
                Id = this.Id,
                Title = this.Title
            };
        }
    }
}
