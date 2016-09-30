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

        [Theory]
        [InlineData(1, "smallest")]
        [InlineData(2, "small")]
        [InlineData(3, "medium")]
        [InlineData(5, "big")]
        public void ItemsUsedWithinOnceOfEachOtherShouldNotBeMoreThanOneSizeDifferent(int n, string expected)
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);

            // Tag 1 should be used once, Tag 2 twice, etc...
            var tagUnderTest = string.Empty;
            var tags = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                var tag = string.Empty.GetRandom();
                if (n == (i + 1))
                    tagUnderTest = tag;

                for (int j = 0; j <= i; j++)
                    tags.Add(tag);
            }

            var actualStyles = styler.GetStyles(tags);
            var actualStyle = actualStyles.Single(s => s.Item1 == tagUnderTest).Item2;
            Assert.Equal(expected, actualStyle);
        }

        [Fact]
        public void ANormallyDistributedTagListShouldProduceSomeSmallSizes()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tagList = (null as IEnumerable<string>).GetTagList();

            // Each tag should be used a random # of times
            var tags = new List<string>();
            for (int i = 0; i < 300.GetRandom(100); i++)
                tags.Add(tagList.GetRandom());

            var actual = styler.GetStyles(tags);
            Assert.True(actual.Count(i => i.Item2 == "small") > 0);
        }

        [Fact]
        public void ANormallyDistributedTagListShouldProduceSomeMediumSizes()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tagList = (null as IEnumerable<string>).GetTagList();

            // Each tag should be used a random # of times
            var tags = new List<string>();
            for (int i = 0; i < 300.GetRandom(100); i++)
                tags.Add(tagList.GetRandom());

            var actual = styler.GetStyles(tags);
            Assert.True(actual.Count(i => i.Item2 == "medium") > 0);
        }

        [Fact]
        public void ANormallyDistributedTagListShouldProduceSomeBigSizes()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tagList = (null as IEnumerable<string>).GetTagList();

            // Each tag should be used a random # of times
            var tags = new List<string>();
            for (int i = 0; i < 300.GetRandom(100); i++)
                tags.Add(tagList.GetRandom());

            var actual = styler.GetStyles(tags);
            Assert.True(actual.Count(i => i.Item2 == "big") > 0);
        }

        [Fact]
        public void ATagThatHasSuperHighUsageShouldReturnABiggestSize()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tagList = (null as IEnumerable<string>).GetTagList();
            var tagUnderTest = string.Empty.GetRandom();

            // Each tag should be used a random # of times
            var tags = new List<string>();
            for (int i = 0; i < 300.GetRandom(100); i++)
            {
                tags.Add(tagList.GetRandom());
                tags.Add(tagUnderTest);
            }

            var actual = styler.GetStyles(tags);
            var actualStyle = actual.Single(s => s.Item1 == tagUnderTest).Item2;
            Assert.Equal("biggest", actualStyle);
        }

        [Fact]
        public void ANormallyDistributedTagListShouldProduceMoreMediumSizesThanBigSizes()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var styler = new DeviationStyler(serviceProvider);
            var tagList = (null as IEnumerable<string>).GetTagList();

            // Each tag should be used a random # of times
            var tags = new List<string>();
            for (int i = 0; i < 300.GetRandom(200); i++)
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
            var tagList = (null as IEnumerable<string>).GetTagList();

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
