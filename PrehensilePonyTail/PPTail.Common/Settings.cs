using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail
{
    public class Settings
    {
        public string DateTimeFormatSpecifier { get; set; }
        public string ItemSeparator { get; set; }

        public string outputFileExtension { get; set; }

        public ExtendedSettingsCollection ExtendedSettings { get; private set; }

        public Settings()
        {
            this.ExtendedSettings = new ExtendedSettingsCollection();
        }
    }
}
