using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;

namespace PPTail.Entities
{
    public class Settings : ISettings
    {
        public string DateTimeFormatSpecifier { get; set; }
        public string DateFormatSpecifier { get; set; }
        public string ItemSeparator { get; set; }
        public string OutputFileExtension { get; set; }

        public ExtendedSettingsCollection ExtendedSettings { get; private set; }

        public Settings()
        {
            this.ExtendedSettings = new ExtendedSettingsCollection();
        }
    }
}
