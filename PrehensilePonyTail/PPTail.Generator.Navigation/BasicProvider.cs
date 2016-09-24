using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.Navigation
{
    public class BasicProvider: Interfaces.INavigationProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public BasicProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string CreateNavigation(IEnumerable<ContentItem> pages, string currentUrl)
        {
            throw new NotImplementedException();
        }
    }
}
