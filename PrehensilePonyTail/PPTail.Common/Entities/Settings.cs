using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;

namespace PPTail.Entities
{
    public class Settings : ISettings
    {
        public String DateTimeFormatSpecifier { get; set; }
        public String DateFormatSpecifier { get; set; }
        public String ItemSeparator { get; set; }

        public String OutputFileExtension { get; set; }

        public ExtendedSettingsCollection ExtendedSettings { get; private set; }


        public Settings()
        {
            this.ExtendedSettings = new ExtendedSettingsCollection();
        }
    }
}
