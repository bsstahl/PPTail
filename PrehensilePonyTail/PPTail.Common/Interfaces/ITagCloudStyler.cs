using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface ITagCloudStyler
    {
        IEnumerable<Tuple<string, string>> GetStyles(IEnumerable<string> tags);
    }
}
