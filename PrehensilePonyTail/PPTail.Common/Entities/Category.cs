using System;

namespace PPTail.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public String Description { get; set; }
        public String Name { get; set; }

        public override String ToString()
        {
            return $"{this.Name} ({this.Id}): {this.Description}";
        }
    }
}
