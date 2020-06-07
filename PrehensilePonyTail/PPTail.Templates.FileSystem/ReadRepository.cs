using PPTail.Entities;
using System;
using System.Collections.Generic;

namespace PPTail.Templates.FileSystem
{
    public class ReadRepository : Interfaces.ITemplateRepository
    {
        public ReadRepository(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Template> GetAllTemplates() => throw new NotImplementedException();
    }
}
