using PPTail.Entities;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Templates.FileSystem
{
    public class ReadRepository : Interfaces.ITemplateRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public ReadRepository(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            throw new NotImplementedException();
        }
    }
}
