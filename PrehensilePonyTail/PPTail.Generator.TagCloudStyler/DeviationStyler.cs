using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.TagCloudStyler
{
    public class DeviationStyler : Interfaces.ITagCloudStyler
    {
        public DeviationStyler(IServiceProvider serviceProvider)
        {
        }

        public IEnumerable<Tuple<string, string>> GetStyles(IEnumerable<string> tags)
        {
            throw new NotImplementedException();
        }
    }
}
