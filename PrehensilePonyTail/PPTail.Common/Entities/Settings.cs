using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PPTail.Interfaces;

namespace PPTail.Entities
{
    public class Settings : ISettings
    {
        public ExtendedSettingsCollection ExtendedSettings { get; private set; }


        public Settings()
        {
            this.ExtendedSettings = new ExtendedSettingsCollection();
        }
    }
}
