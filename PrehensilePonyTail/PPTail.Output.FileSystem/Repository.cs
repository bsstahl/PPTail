using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail.Output.FileSystem
{
    public class Repository: IOutputRepository
    {
        public Repository(IServiceProvider serviceProvider)
        {
        }

        public void Save(IEnumerable<SiteFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
