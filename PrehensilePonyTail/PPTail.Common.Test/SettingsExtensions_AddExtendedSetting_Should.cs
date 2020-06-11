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
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SettingsExtensions_AddExtendedSetting_Should
    {
        [Fact]
        public void ThrowArgumentNullExceptionIfSettingsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => (null as ISettings).AddExtendedSetting(string.Empty, string.Empty));
        }

        [Fact]
        public void ReturnTheCorrectArgumentNameIfSettingsIsNull()
        {
            String actual = string.Empty;
            try
            {
                (null as ISettings).AddExtendedSetting(string.Empty, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }
            Assert.Equal("settings", actual);
        }

        [Fact]
        public void ThrowArgumentNullExceptionIfExtendedSettingsIsNull()
        {
            var settings = Mock.Of<ISettings>();
            Assert.Throws<ArgumentNullException>(() => settings.AddExtendedSetting(string.Empty, string.Empty));
        }

        [Fact]
        public void ReturnTheCorrectArgumentNameIfExtendedSettingsIsNull()
        {
            String actual = string.Empty;
            try
            {
                var settings = Mock.Of<ISettings>();
                settings.AddExtendedSetting(string.Empty, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                actual = ex.ParamName;
            }
            Assert.Equal("ExtendedSettings", actual);
        }
    }
}
