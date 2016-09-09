using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PPTail.Interfaces;

namespace PPTail.SiteGenerator.Test
{
    public class Builder_Build_Should
    {
        [Fact]
        public void RequestAllPagesFromTheRepository()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            contentRepo.Verify(c => c.GetAllPages(), Times.AtLeastOnce());
        }

        [Fact]
        public void RequestAllPostsFromTheRepository()
        {
            var pageGen = Mock.Of<IPageGenerator>();
            var contentRepo = new Mock<IContentRepository>();

            var target = (null as Builder).Create(contentRepo.Object, pageGen);
            var actual = target.Build();

            contentRepo.Verify(c => c.GetAllPosts(), Times.AtLeastOnce());
        }
    }
}
