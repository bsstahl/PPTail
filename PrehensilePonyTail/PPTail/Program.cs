using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PPTail.Entities;

namespace PPTail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // string sourceDataPath = "C:\\Users\\bsstahl\\Documents\\CognitiveInheritance\\App_Data";
            string sourceDataPath = "E:\\CognitiveInheritance\\App_Data";
            string outputPath = "C:\\Users\\bsstahl\\Documents\\My Web Sites\\Generated";
            string dateTimeFormatSpecifier = "MM/dd/yy H:mm UTC";
            string itemSeparator = "<hr/>";

            var settings = (null as Settings).Create(sourceDataPath, outputPath, dateTimeFormatSpecifier, itemSeparator);

            string styleTemplatePath = "..\\Style.template.css";
            string homePageTemplatePath = "..\\HomePage.template.html";
            string contentPageTemplatePath = "..\\ContentPage.template.html";
            string postPageTemplatePath = "..\\PostPage.template.html";
            string itemTemplatePath = "..\\ContentItem.template.html";

            string contentPageTemplate = System.IO.File.ReadAllText(contentPageTemplatePath);
            string styleTemplate = System.IO.File.ReadAllText(styleTemplatePath);
            string homePageTemplate = System.IO.File.ReadAllText(homePageTemplatePath);
            string postPageTemplate = System.IO.File.ReadAllText(postPageTemplatePath);
            string itemTemplate = System.IO.File.ReadAllText(itemTemplatePath);

            var container = (null as IServiceCollection).Create(settings);
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
