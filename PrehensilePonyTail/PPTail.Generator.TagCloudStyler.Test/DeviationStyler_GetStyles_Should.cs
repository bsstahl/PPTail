using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace PPTail.Generator.TagCloudStyler.Test
{
    public class DeviationStyler_GetStyles_Should
    {
        [Fact]
        public void ReturnOneResultPerTag()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tag1 = string.Empty.GetRandom();
            var tag2 = string.Empty.GetRandom();
            var tags = new List<string>() { tag1, tag1, tag2 };
            var actual = styler.GetStyles(tags);
            Assert.Equal(1, actual.Count(s => s.Item1 == tag1));
            Assert.Equal(1, actual.Count(s => s.Item1 == tag2));
        }

        [Fact]
        public void IfAllItemsUsedTheSameAmountAllShouldReturnSmallest()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tag1 = string.Empty.GetRandom();
            var tag2 = string.Empty.GetRandom();
            var tag3 = string.Empty.GetRandom();

            // Each tag should be used twice
            var tags = new List<string>() { tag1, tag1, tag2, tag3, tag2, tag3 };
            var actual = styler.GetStyles(tags);

            foreach (var actualValue in actual)
                Assert.Equal("smallest", actualValue.Item2);
        }

        [Fact]
        public void ANormallyDistributedTagListShouldProduceOutputInAllSizeRanges()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tagList = new List<string>() { string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom() };

            // Each tag should be used a random # of times
            var tags = new List<string>();
            for (int i = 0; i < 300.GetRandom(100); i++)
                tags.Add(tagList.GetRandom());

            var actual = styler.GetStyles(tags);
            Assert.True(actual.Count(i => i.Item2 == "smallest") > 0);
            Assert.True(actual.Count(i => i.Item2 == "small") > 0);
            Assert.True(actual.Count(i => i.Item2 == "medium") > 0);
            Assert.True(actual.Count(i => i.Item2 == "big") > 0);
            Assert.True(actual.Count(i => i.Item2 == "biggest") > 0);
        }

        [Fact]
        public void ANormallyDistributedTagListShouldProduceMoreMediumSizesThanBigSizes()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tagList = new List<string>() { string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom() };

            // Each tag should be used a random # of times
            var tags = new List<string>();
            for (int i = 0; i < 300.GetRandom(100); i++)
                tags.Add(tagList.GetRandom());

            var actual = styler.GetStyles(tags);
            var smallestCount = actual.Count(i => i.Item2 == "smallest");
            var smallCount = actual.Count(i => i.Item2 == "small");
            var mediumCount = actual.Count(i => i.Item2 == "medium");
            var bigCount = actual.Count(i => i.Item2 == "big");
            var biggestCount = actual.Count(i => i.Item2 == "biggest");

            Assert.True(mediumCount > bigCount);
        }

        [Fact]
        public void ANormallyDistributedTagListShouldProduceMoreMediumSizesThanBiggestSizes()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tagList = new List<string>() { string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom(), string.Empty.GetRandom() };

            // Each tag should be used a random # of times
            var tags = new List<string>();
            for (int i = 0; i < 300.GetRandom(100); i++)
                tags.Add(tagList.GetRandom());

            var actual = styler.GetStyles(tags);
            var smallestCount = actual.Count(i => i.Item2 == "smallest");
            var smallCount = actual.Count(i => i.Item2 == "small");
            var mediumCount = actual.Count(i => i.Item2 == "medium");
            var bigCount = actual.Count(i => i.Item2 == "big");
            var biggestCount = actual.Count(i => i.Item2 == "biggest");

            Assert.True(mediumCount > biggestCount);
        }
    }
}
