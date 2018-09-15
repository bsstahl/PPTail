using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TODO: Harden this
            var sourceDataPath = args[0];
            var outputPath = args[1];

            var settings = (null as ISettings).Create(sourceDataPath, outputPath);
            var templates = (null as IEnumerable<Template>).Create("..\\..\\..\\..");

            var container = (null as IServiceCollection).Create(settings, templates);
            var serviceProvider = container.BuildServiceProvider();

            // TODO: Move data load here -- outside of the build process

            var siteBuilder = serviceProvider.GetService<ISiteBuilder>();
            var sitePages = siteBuilder.Build();

            var outputRepo = serviceProvider.GetService<Interfaces.IOutputRepository>();
            outputRepo.Save(sitePages);
        }

    }
}
