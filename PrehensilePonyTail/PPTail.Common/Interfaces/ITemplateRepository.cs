using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Interfaces
{
    public interface ITemplateRepository
    {
        IEnumerable<Template> GetAllTemplates();
    }
}
