using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class SettingsExtensions
    {
        public static void Validate(this ISettings settings, String extendedSettingName)
        {
            if (settings == null || settings.ExtendedSettings == null || !settings.ExtendedSettings.HasSetting(extendedSettingName))
                throw new Exceptions.SettingNotFoundException(extendedSettingName);
        }

        public static void Validate(this ISettings settings, Func<ISettings, string> setting, String settingName)
        {
            if (settings == null || string.IsNullOrWhiteSpace(setting?.Invoke(settings)))
                throw new Exceptions.SettingNotFoundException(settingName);
        }

        public static void AddExtendedSetting(this ISettings settings, String settingName, String settingValue)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (settings.ExtendedSettings == null)
                throw new ArgumentNullException(nameof(settings.ExtendedSettings));

            settings.ExtendedSettings.Add(new Tuple<string, string>(settingName, settingValue));
        }

        public static String GetExtendedSetting(this ISettings settings, String settingName)
        {
            String result = string.Empty;
            if (settings != null && settings.ExtendedSettings != null)
                result = settings.ExtendedSettings.Get(settingName);
            return result;
        }
    }
}
