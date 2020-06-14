using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Builders
{
    public class SettingsBuilder: Entities.Settings
    {
        public ISettings Build()
        {
            return this;
        }

        public SettingsBuilder AddExtendedSetting(String name, String value)
        {
            this.ExtendedSettings.Add(new Tuple<string, string>(name, value));
            return this;
        }
    }
}
