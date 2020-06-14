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

        public new SettingsBuilder DateFormatSpecifier(String dateFormatSpecifier)
        {
            base.DateFormatSpecifier = dateFormatSpecifier;
            return this;
        }

        public new SettingsBuilder DateTimeFormatSpecifier(String dateTimeFormatSpecifier)
        {
            base.DateTimeFormatSpecifier = dateTimeFormatSpecifier;
            return this;
        }

        public new SettingsBuilder ItemSeparator (String itemSeparator)
        {
            base.ItemSeparator = itemSeparator;
            return this;
        }

        public new SettingsBuilder OutputFileExtension(String outputFileExtension)
        {
            base.OutputFileExtension = outputFileExtension;
            return this;
        }

        public SettingsBuilder AddExtendedSetting(String name, String value)
        {
            this.ExtendedSettings.Add(new Tuple<string, string>(name, value));
            return this;
        }
    }
}
