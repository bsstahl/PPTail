using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Interfaces;

namespace PPTail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            (var argsAreValid, var argumentErrrors) = args.ValidateArguments();

            if (argsAreValid)
            {
                var (sourceConnection, targetConnection, templateConnection) = args.ParseArguments();

                var settings = (null as ISettings).Create(sourceConnection, targetConnection, templateConnection);
                var templates = (null as IEnumerable<Template>).Create(templateConnection);

                var container = (null as IServiceCollection).Create(settings, templates);
                var serviceProvider = container.BuildServiceProvider();

                // TODO: Move data load here -- outside of the build process
                //var contentRepos = serviceProvider.GetServices<IContentRepository>();
                //var contentRepo = contentRepos.First(); // TODO: Implement properly


                var siteBuilder = serviceProvider.GetService<ISiteBuilder>();
                var sitePages = siteBuilder.Build();

                // TODO: Change this to use the named provider specified in the input args
                var outputRepo = serviceProvider.GetService<Interfaces.IOutputRepository>();
                outputRepo.Save(sitePages);
            }
            else
            {
                // TODO: Display argument errors to user
                throw new NotImplementedException();
            }
        }

    }
}
