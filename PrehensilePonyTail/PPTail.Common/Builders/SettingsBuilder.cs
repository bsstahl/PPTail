using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Common.Builders
{
    public class SettingsBuilder: Entities.Settings
    {
        public ISettings Build()
        {
            return this;
        }

        public new SettingsBuilder DateFormatSpecifier(string dateFormatSpecifier)
        {
            base.DateFormatSpecifier = dateFormatSpecifier;
            return this;
        }

        public new SettingsBuilder DateTimeFormatSpecifier(string dateTimeFormatSpecifier)
        {
            base.DateTimeFormatSpecifier = dateTimeFormatSpecifier;
            return this;
        }

        public new SettingsBuilder ItemSeparator (string itemSeparator)
        {
            base.ItemSeparator = itemSeparator;
            return this;
        }

        public new SettingsBuilder OutputFileExtension(string outputFileExtension)
        {
            base.OutputFileExtension = outputFileExtension;
            return this;
        }

        public new SettingsBuilder SourceConnection(string sourceConnection)
        {
            base.SourceConnection = sourceConnection;
            return this;
        }

        public new SettingsBuilder TargetConnection(string TargetConnection)
        {
            base.TargetConnection = TargetConnection;
            return this;
        }

        public new SettingsBuilder TemplateConnection(string templateConnection)
        {
            base.TemplateConnection = templateConnection;
            return this;
        }

        public SettingsBuilder AddExtendedSetting(string name, string value)
        {
            this.ExtendedSettings.Add(new Tuple<string, string>(name, value));
            return this;
        }
    }
}
