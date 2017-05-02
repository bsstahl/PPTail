using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = (null as ISettings).Create();
            var templates = (null as IEnumerable<Template>).Create();

            var container = (null as IServiceCollection).Create(settings, templates);
            var serviceProvider = container.BuildServiceProvider();

            // TODO: Move data load here -- outside of the build process

            var siteBuilder = serviceProvider.GetService<PPTail.SiteGenerator.Builder>();
            var sitePages = siteBuilder.Build();

            var outputRepo = serviceProvider.GetService<Interfaces.IOutputRepository>();
            outputRepo.Save(sitePages);
        }

    }
}
