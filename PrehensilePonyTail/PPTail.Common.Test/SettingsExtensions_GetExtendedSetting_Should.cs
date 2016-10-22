using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TestHelperExtensions;
using PPTail.Interfaces;
using PPTail.Extensions;
using Moq;

namespace PPTail.Common.Test
{
    public class SettingsExtensions_GetExtendedSetting_Should
    {
        [Fact]
        public void ReturnAnEmptyStringIfSettingsIsNull()
        {
            string actual = (null as ISettings).GetExtendedSetting(string.Empty);
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ReturnAnEmptyStringIfExtendedSettingsIsNull()
        {
            var settings = Mock.Of<ISettings>();
            string actual = settings.GetExtendedSetting(string.Empty);
            Assert.Equal(string.Empty, actual);
        }

    }
}
