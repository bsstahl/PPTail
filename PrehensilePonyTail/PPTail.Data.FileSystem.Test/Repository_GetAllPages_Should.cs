using Microsoft.Extensions.DependencyInjection;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
