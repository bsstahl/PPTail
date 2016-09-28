using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Generator.Contact
{
    public class TemplateProvider: IContactProvider
    {
        public TemplateProvider(IServiceProvider serviceProvider)
        {
        }

        public string GenerateContactPage(string navigationContent, string sidebarContent, string pathToRoot)
        {
            throw new NotImplementedException();
        }
    }
}
