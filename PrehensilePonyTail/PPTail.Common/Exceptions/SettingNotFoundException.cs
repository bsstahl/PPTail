using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Exceptions
{
    public class SettingNotFoundException:Exception
    {
        public string SettingName { get; set; }

        public SettingNotFoundException(string settingName):base($"Required setting '{settingName}' was not provided.")
        {
            this.SettingName = settingName;
        }
    }
}
