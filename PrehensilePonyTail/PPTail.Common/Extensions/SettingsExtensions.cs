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

        public static void Validate(this ISettings settings, Func<ISettings, string> setting, string settingName)
        {
            if (settings == null || string.IsNullOrWhiteSpace(setting.Invoke(settings)))
                throw new Exceptions.SettingNotFoundException(settingName);
        }

        public static void AddExtendedSetting(this ISettings settings, string settingName, string settingValue)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (settings.ExtendedSettings == null)
                throw new ArgumentNullException(nameof(settings.ExtendedSettings));

            settings.ExtendedSettings.Add(new Tuple<string, string>(settingName, settingValue));
        }

        public static string GetExtendedSetting(this ISettings settings, string settingName)
        {
            string result = string.Empty;
            if (settings != null && settings.ExtendedSettings != null)
                result = settings.ExtendedSettings.Get(settingName);
            return result;
        }
    }
}
