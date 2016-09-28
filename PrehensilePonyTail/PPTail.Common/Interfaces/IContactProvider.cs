using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IContactProvider
    {
        string GenerateContactPage(string navigationContent, string sidebarContent, string pathToRoot);
    }
}
