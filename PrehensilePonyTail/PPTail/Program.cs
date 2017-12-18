using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail
{
    public class Program
    {
        const string _templatePathSettingName = "templatePath";

        public static void Main(string[] args)
        {
            var settings = (null as ISettings).Create();

            string templatePath = settings.ExtendedSettings.Get(_templatePathSettingName) ?? "..";
            var templates = (null as IEnumerable<Template>).Create(templatePath);

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
