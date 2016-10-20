using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IRedirectProvider
    {
        string GenerateRedirect(string redirectToUrl);
    }
}
