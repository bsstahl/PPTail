using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;

namespace PPTail.Common.Test
{
    public class ExtendedSettingsCollection_Set_Should
    {
        [Fact]
        public void AddTheSettingIfTheSettingDoesNotExist()
        {
            string settingName = string.Empty.GetRandom();
            string updatedValue = string.Empty.GetRandom();

            var target = new ExtendedSettingsCollection();
            target.Set(settingName, updatedValue);

            var actual = target.Single(t => t.Item1 == settingName).Item2;
            Assert.Equal(updatedValue, actual);
        }

        [Fact]
        public void UpdateTheValueInTheSettingIfTheSettingExists()
        {
            string settingName = string.Empty.GetRandom();
            string originalValue = string.Empty.GetRandom();
            string updatedValue = string.Empty.GetRandom();

            var target = new ExtendedSettingsCollection();
            target.Add(new Tuple<string, string>(settingName, originalValue));

            target.Set(settingName, updatedValue);

            var actual = target.Single(t => t.Item1 == settingName).Item2;
            Assert.Equal(updatedValue, actual);
        }

        [Fact]
        public void ReturnTheSettingWithTheProperName()
        {
            string settingName = string.Empty.GetRandom();
            string updatedValue = string.Empty.GetRandom();

            var target = new ExtendedSettingsCollection();
            var actual = target.Set(settingName, updatedValue);

            Assert.Equal(settingName, actual.Item1);
        }

        [Fact]
        public void ReturnTheSettingWithTheProperValue()
        {
            string settingName = string.Empty.GetRandom();
            string updatedValue = string.Empty.GetRandom();

            var target = new ExtendedSettingsCollection();
            var actual = target.Set(settingName, updatedValue);

            Assert.Equal(updatedValue, actual.Item2);
        }
    }
}
