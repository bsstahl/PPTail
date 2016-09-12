using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string sourcePath = "C:\\Users\\bsstahl\\Documents\\CognitiveInheritance\\App_Data";
            string outputPath = "C:\\Users\\bsstahl\\Documents\\My Web Sites\\Generated";

            string dateTimeFormatSpecifier = "MM/dd/yy H:mm UTC";
            string itemSeparator = "<hr/>";

            string styleTemplatePath = "C:\\Users\\bsstahl\\Source\\Repos\\PPTail\\PrehensilePonyTail\\Style.template.css";
            string homePageTemplatePath = "C:\\Users\\bsstahl\\Source\\Repos\\PPTail\\PrehensilePonyTail\\HomePage.template.html";
            string contentPageTemplatePath = "C:\\Users\\bsstahl\\Source\\Repos\\PPTail\\PrehensilePonyTail\\ContentPage.template.html";
            string postPageTemplatePath = "C:\\Users\\bsstahl\\Source\\Repos\\PPTail\\PrehensilePonyTail\\PostPage.template.html";
            string itemTemplatePath = "C:\\Users\\bsstahl\\Source\\Repos\\PPTail\\PrehensilePonyTail\\ContentItem.template.html";

            string styleTemplate = System.IO.File.ReadAllText(styleTemplatePath);
            string homePageTemplate = System.IO.File.ReadAllText(homePageTemplatePath);
            string contentPageTemplate = System.IO.File.ReadAllText(contentPageTemplatePath);
            string postPageTemplate = System.IO.File.ReadAllText(postPageTemplatePath);
            string itemTemplate = System.IO.File.ReadAllText(itemTemplatePath);

            var repo = new PPTail.Data.FileSystem.Repository(sourcePath);
            var pageGenerator = new PPTail.Generator.T4Html.PageGenerator(styleTemplate, homePageTemplate, contentPageTemplate, postPageTemplate, itemTemplate, dateTimeFormatSpecifier, itemSeparator);
            var p = new PPTail.SiteGenerator.Builder(repo, pageGenerator, "html");
            var sitePages = p.Build();

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
