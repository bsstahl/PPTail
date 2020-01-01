using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IContactProvider
    {
        String GenerateContactPage(String navigationContent, String sidebarContent, String pathToRoot);
    }
}
