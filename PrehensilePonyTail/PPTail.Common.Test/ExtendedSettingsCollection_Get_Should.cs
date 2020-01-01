using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

namespace PPTail.Common.Test
{
    public class ExtendedSettingsCollection_Get_Should
    {
        [Fact]
        public void ReturnTheProperValueIfTheValueExists()
        {
            String settingName = string.Empty.GetRandom();
            String expected = string.Empty.GetRandom();
            var target = new ExtendedSettingsCollection();
            target.Add(new Tuple<string, string>(settingName, expected));
            var actual = target.Get(settingName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnAnEmptyStringIfTheValueDoesNotExist()
        {
            String settingName = string.Empty.GetRandom();
            String expected = string.Empty;
            var target = new ExtendedSettingsCollection();
            var actual = target.Get(settingName);
            Assert.Equal(expected, actual);
        }
    }
}
