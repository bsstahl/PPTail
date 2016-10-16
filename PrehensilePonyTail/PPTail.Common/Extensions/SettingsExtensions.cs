using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class SettingsExtensions
    {
        public static void Validate(this ISettings settings, string extendedSettingName)
        {
            if (settings == null || settings.ExtendedSettings == null || !settings.ExtendedSettings.HasSetting(extendedSettingName))
                throw new Exceptions.SettingNotFoundException(extendedSettingName);
        }

    }
}
