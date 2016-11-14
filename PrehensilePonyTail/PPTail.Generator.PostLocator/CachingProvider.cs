using PPTail.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.PostLocator
{
    public class CachingProvider: Interfaces.IPostLocator
    {
        public CachingProvider(IServiceProvider serviceProvider)
        {
        }

        public string GetUrlByPostId(Guid id)
        {
            // TODO: Implement for real
            if (id == Guid.Parse("2f823b30-355a-49e1-8ecf-121cff1b3547"))
                return "Posts/Exception-Handling-Block.html";
            else
                throw new PostNotFoundException(id);
        }
    }
}
