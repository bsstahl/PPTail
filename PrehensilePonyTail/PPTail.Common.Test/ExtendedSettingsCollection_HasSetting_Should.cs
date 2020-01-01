using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

namespace PPTail.Common.Test
{
    public class ExtendedSettingsCollection_HasSetting_Should
    {
        [Fact]
        public void ReturnFalseIfTheSettingDoesNotExist()
        {
            String settingName = string.Empty.GetRandom();
            var target = new ExtendedSettingsCollection();
            var actual = target.HasSetting(settingName);
            Assert.False(actual);
        }

        [Fact]
        public void ReturnTrueIfTheSettingExists()
        {
            String settingName = string.Empty.GetRandom();
            var target = new ExtendedSettingsCollection();
            target.Add(new Tuple<string, string>(settingName, string.Empty.GetRandom()));
            var actual = target.HasSetting(settingName);
            Assert.True(actual);
        }
    }
}
