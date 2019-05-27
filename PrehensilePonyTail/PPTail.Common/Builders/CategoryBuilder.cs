using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Common.Builders
{
    public class CategoryBuilder: Category
    {
        public Category Build()
        {
            return this;
        }

        public new CategoryBuilder Id(Guid id)
        {
            base.Id = id;
            return this;
        }

        public new CategoryBuilder Name(string name)
        {
            base.Name = name;
            return this;
        }

        public new CategoryBuilder Description(string description)
        {
            base.Description = description;
            return this;
        }

    }
}
