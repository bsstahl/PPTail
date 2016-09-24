using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Extensions;
using Xunit;

namespace PPTail.Common.Test
{
    public class IntegerExtensions_StdDev_Should
    {
        int[] _intData = new int[] { -26292, 30514, -4049, 26186, 29502, 4955, -5713, -1531, -2395, -32235, 19604, 579, -25724, -8913, -14237, -1329, 8934, 24172, 29399, -9403, 25830, -8296, 30229, 5877, 32274, 9709, 3299, 18683, 141, -10233, 19725, -25332, 20029, 14129, 5790, -8398, -11898, 6247, 6452, 21190, 14534, -19250, 4543, -1 };


        [Fact]
        public void ReturnZeroIfNoValuesAreProvided()
        {
            var expected = 0.0;
            var actual = (new List<int>()).StdDev();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnTheValueIfOneValueIsProvided()
        {
            var expected = 5730;
            var actual = (new List<int>() { expected }).StdDev();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CorrectlyCalculateThePopulationStandardDeviationOfAListOfIntegers()
        {
            var expected = 16959.71675;
            var actual = System.Math.Round(_intData.StdDev(), 5);
            Assert.Equal(expected, actual);
        }


    }
}
