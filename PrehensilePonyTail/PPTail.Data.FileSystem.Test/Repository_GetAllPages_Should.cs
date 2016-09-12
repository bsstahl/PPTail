using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

namespace PPTail.Data.FileSystem.Test
{
    public class Repository_GetAllPages_Should
    {
        [Fact]
        public void ReturnAllPagesIfAllAreValid()
        {
            var files = new List<string>();
            files.Add("28C65CCD-D504-44D3-A54B-9E3DBB163D43.xml");
            files.Add("8EE89C80-760E-4980-B980-5A4B70A563E2.xml");
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");

            var fileSystem = new MockFileSystem();
            fileSystem.Files = files;
            fileSystem.FileText = "<page/>";

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem);

            var target = (null as IContentRepository).Create(serviceProvider);
            var pages = target.GetAllPages();

            Assert.Equal(files.Count(), pages.Count());
        }

        [Fact]
        public void IgnoreFilesWithoutXmlExtension()
        {
            var files = new List<string>();
            files.Add("82B52DBC-9D33-4C9E-A933-AF515E4FF140");
            files.Add("28C65CCD-D504-44D3-A54B-9E3DBB163D43.xml");
            files.Add("0F716B73-9A2F-46D9-A576-3CA03EB10327.ppt");
            files.Add("8EE89C80-760E-4980-B980-5A4B70A563E2.xml");
            files.Add("39836B5E-C330-4670-9897-1CBF0851AB5B.txt");
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");
            files.Add("86F29FA4-29CD-4292-8000-CEAFEA7A2315.com");

            var fileSystem = new MockFileSystem();
            fileSystem.Files = files;
            fileSystem.FileText = "<page/>";

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem);

            var target = (null as IContentRepository).Create(serviceProvider);
            var pages = target.GetAllPages();

            Assert.Equal(3, pages.Count());
        }

        [Fact]
        public void RequestFilesFromThePagesFolder()
        {
            var files = new List<string>();
            files.Add("68AA2FE5-58F9-421A-9C1B-02254B953BC5.xml");

            var fileSystem = new MockFileSystem();
            fileSystem.Files = files;
            fileSystem.FileText = "<page/>";

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddSingleton<IFileSystem>(fileSystem);

            string rootDirectory = $"c:\\{string.Empty.GetRandom()}";
            string expected = rootDirectory + "\\pages";

            var target = (null as IContentRepository).Create(serviceProvider);
            var pages = target.GetAllPages();

            Assert.Equal(expected, fileSystem.PathLastEnumerated);
        }

        [Fact(Skip = "NotImplemented")]
        public void SkipUnpublishedPage()
        {

        }

        [Fact(Skip = "NotImplemented")]
        public void SkipPagesWithInvalidSchema()
        {

        }

    }
}
