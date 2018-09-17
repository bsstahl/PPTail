using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using PPTail.Extensions;

namespace PPTail.Common.Test
{
    public class StringExtensions_GetConnectionStringValue_Should
    {

        [Fact]
        public void ReturnTheProperProviderValue()
        {
            string expectedProvider = $"{string.Empty.GetRandom()}.{string.Empty.GetRandom()}.{string.Empty.GetRandom()}";
            string expectedFilePath = $"c:\\{string.Empty.GetRandom()}";
            var target = $"Provider={expectedProvider};FilePath={expectedFilePath}";
            var actual = target.GetConnectionStringValue("Provider");
            Assert.Equal(expectedProvider, actual);
        }

        [Fact]
        public void ReturnTheProperProviderValueCaseInsensitive()
        {
            string expectedProvider = $"{string.Empty.GetRandom()}.{string.Empty.GetRandom()}.{string.Empty.GetRandom()}";
            string expectedFilePath = $"c:\\{string.Empty.GetRandom()}";
            var target = $"provider={expectedProvider};FilePath={expectedFilePath}";
            var actual = target.GetConnectionStringValue("Provider");
            Assert.Equal(expectedProvider, actual);
        }

        [Fact]
        public void ReturnTheProperFilePathValue()
        {
            string expectedProvider = $"{string.Empty.GetRandom()}.{string.Empty.GetRandom()}.{string.Empty.GetRandom()}";
            string expectedFilePath = $"c:\\{string.Empty.GetRandom()}";
            var target = $"Provider={expectedProvider};FilePath={expectedFilePath}";
            var actual = target.GetConnectionStringValue("FilePath");
            Assert.Equal(expectedFilePath, actual);
        }

        [Fact]
        public void ReturnTheProperFilePathValueCaseInsensitive()
        {
            string expectedProvider = $"{string.Empty.GetRandom()}.{string.Empty.GetRandom()}.{string.Empty.GetRandom()}";
            string expectedFilePath = $"c:\\{string.Empty.GetRandom()}";
            var target = $"provider={expectedProvider};FilePath={expectedFilePath}";
            var actual = target.GetConnectionStringValue("filepath");
            Assert.Equal(expectedFilePath, actual);
        }
    }
}
