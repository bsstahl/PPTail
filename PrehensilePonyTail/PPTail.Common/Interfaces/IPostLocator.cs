using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IPostLocator
    {
        string GetUrlByPostId(Guid id);
    }
}
