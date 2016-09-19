using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;
using PPTail.Enumerations;

namespace PPTail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string _sourceDataPathSettingName = "sourceDataPath";
            const string _outputPathSettingName = "outputPath";

            string outputFileExtension = "html";
            string dateTimeFormatSpecifier = "MM/dd/yy H:mm UTC";
            string itemSeparator = "<hr/>";

            string styleTemplatePath = "..\\Style.template.css";
            string bootstrapTemplatePath = "..\\bootstrap.min.css";
            string homePageTemplatePath = "..\\HomePage.template.html";
            string contentPageTemplatePath = "..\\ContentPage.template.html";
            string postPageTemplatePath = "..\\PostPage.template.html";
            string itemTemplatePath = "..\\ContentItem.template.html";


            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var config = builder.Build();

            string sourceDataPath = config[_sourceDataPathSettingName];
            string outputPath = config[_outputPathSettingName];

            var settings = (null as Settings).Create(sourceDataPath, outputPath, dateTimeFormatSpecifier, itemSeparator, outputFileExtension);
            var templates = (null as IEnumerable<Template>).Create(styleTemplatePath, bootstrapTemplatePath, homePageTemplatePath, contentPageTemplatePath, postPageTemplatePath, itemTemplatePath);

            var container = (null as IServiceCollection).Create(settings, templates);

            var serviceProvider = container.BuildServiceProvider();

            var siteBuilder = serviceProvider.GetService<PPTail.SiteGenerator.Builder>();
            var sitePages = siteBuilder.Build();

            foreach (var sitePage in sitePages)
            {
                string fullPath = System.IO.Path.Combine(outputPath, sitePage.RelativeFilePath);
                string folderPath = System.IO.Path.GetDirectoryName(fullPath);

                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);

                System.IO.File.WriteAllText(fullPath, sitePage.Content);
            }
        }

    }
}
