using PPTail.Exceptions;
using PPTail.Interfaces;
using PPTail.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace PPTail.Common.Test
{
    public class SettingsExtensions_Validate_Should
    {
        [Fact]
        public void ThrowASettingsNotFoundExceptionIfSettingsIsNull()
        {
            ISettings settings = null;
            string settingName = string.Empty;
            Assert.Throws<SettingNotFoundException>(() => settings.Validate(settingName));
        }

        [Fact]
        public void ThrowASettingsNotFoundExceptionIfExtendedSettingsPropertyIsNull()
        {
            ExtendedSettingsCollection extendedSettings = null;
            var mockSettings = new Mock<ISettings>();
            mockSettings.SetupGet(s => s.ExtendedSettings).Returns(extendedSettings);
            var settings = mockSettings.Object;

            string settingName = string.Empty;
            Assert.Throws<SettingNotFoundException>(() => settings.Validate(settingName));
        }
    }
}
